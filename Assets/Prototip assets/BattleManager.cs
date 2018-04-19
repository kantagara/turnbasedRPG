using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class BattleManager : MonoBehaviour
{
	public AvatarController Player;
	public AvatarController Enemy;
	public float PauseBetweenUnits = 0.02f;
	public GameObject CancelButton;
	public BattlefieldUnits BattlefieldUnits;
	public UnitController ActiveUnit { get; set; }

	private void OnEnable()
	{
		EventPool.StartListening(EventTypes.Move, Move);
		EventPool.StartListening<Transform>(EventTypes.MoveTo, MoveTo);
		EventPool.StartListening<bool>(EventTypes.ActionNoTarget, NoTargetAction);
		EventPool.StartListening<ActionType, UnitController>(EventTypes.ActionEnemyTarget, TargetEnemy);
		EventPool.StartListening<ActionType, UnitController>(EventTypes.ActionFriendlyTarget, TargetFriend);
		EventPool.StartListening<ActionType, AvatarController>(EventTypes.ActionFriendlyTargetAvatar, TargetFriend);
		EventPool.StartListening(EventTypes.Cancel, CancelAction);
		EventPool.StartListening<UnitController>(EventTypes.Targeted, ExecuteTargetingAction);
		EventPool.StartListening<UnitController>(EventTypes.UnitDeath, RemoveUnit);
		InfoPool.Provide("Battlefield units", GetBattlefieldUnits);
	}
	
	private void OnDisable()
	{
		EventPool.StopListening(EventTypes.Move, Move);
		EventPool.StopListening<Transform>(EventTypes.MoveTo, MoveTo);
		EventPool.StopListening<bool>(EventTypes.ActionNoTarget, NoTargetAction);
		EventPool.StopListening<ActionType, UnitController>(EventTypes.ActionEnemyTarget, TargetEnemy);
		EventPool.StopListening<ActionType, UnitController>(EventTypes.ActionFriendlyTarget, TargetFriend);
		EventPool.StopListening<ActionType, AvatarController>(EventTypes.ActionFriendlyTargetAvatar, TargetFriend);
		EventPool.StopListening(EventTypes.Cancel, CancelAction);
		EventPool.StopListening<UnitController>(EventTypes.Targeted, ExecuteTargetingAction);
		EventPool.StopListening<UnitController>(EventTypes.UnitDeath, RemoveUnit);
		InfoPool.Unprovide("Battlefield units", GetBattlefieldUnits);
	}

	private BattlefieldUnits GetBattlefieldUnits()
	{
		return BattlefieldUnits;
	}

	private void Start()
	{
		BattlefieldUnits.SetUnitAllegiances();
		PlayerController player = Player as PlayerController;
		if (player != null)
		{
			player.SetUnits(BattlefieldUnits.UnitsOf(player.Player));
		}
		player = Enemy as PlayerController;
		if (player != null)
		{
			player.SetUnits(BattlefieldUnits.UnitsOf(player.Player));
		}
		AdvanceUnit();
	}

	#region Actions
	//TODO Nije potreban UnitController, jer sam ubacio ActiveUnit
	//TODO Možda svi ovi targeti ne treba da proveravaju imunitete, već jedinice to samo treba da urade (ali provera dometa?)

	private void Move()
	{
		IEnumerable<PlaceOnBattlefield> neighbouringPlaces = BattlefieldUnits.NeighbouringPlaces(ActiveUnit);
		foreach (PlaceOnBattlefield place in neighbouringPlaces)
		{
			place.SetTargetable();
		}
		IdleAnimations(ActiveUnit.PlayersArmy);
	}

	private void MoveTo(Transform placeTransform)
	{
		IEnumerable<PlaceOnBattlefield> neighbouringPlaces = BattlefieldUnits.NeighbouringPlaces(ActiveUnit);
		BattlefieldUnits.MoveTo(placeTransform, ActiveUnit);
		foreach (PlaceOnBattlefield place in neighbouringPlaces)
		{
			place.SetUntargetable();
		}
		AdvanceUnit();
	}
	
	private void NoTargetAction(bool playersArmy)
	{
		IdleAnimations(playersArmy);
		AdvanceUnit();
	}
	
	private void TargetEnemy(ActionType actionType, UnitController actingUnit)
	{
		IdleAnimations(actingUnit.PlayersArmy);

		//if (UnitsOnBattlefield.UnitsInRange(actingUnit) == null) print("Range out of range :D");
		foreach (UnitController unit in BattlefieldUnits.UnitsInRange(actingUnit))
		{
			unit.Targetable = !unit.ImmunitiesToActions.Contains(actionType);
		}
		CancelButton.GetComponent<SpriteRenderer>().enabled = true;
		CancelButton.GetComponent<Collider2D>().enabled = true;
		CancelButton.transform.GetChild(0).gameObject.SetActive(true);
	}

	private void TargetFriend(ActionType actionType, AvatarController avatar)
	{
		IdleAnimations(avatar.Player);

		foreach (UnitController unit in BattlefieldUnits.UnitsOf(avatar.Player))
		{
			unit.Targetable = !unit.ImmunitiesToActions.Contains(actionType);
		}
		CancelButton.GetComponent<SpriteRenderer>().enabled = true;
		CancelButton.GetComponent<Collider2D>().enabled = true;
		CancelButton.transform.GetChild(0).gameObject.SetActive(true);
	}

	private void TargetFriend(ActionType actionType, UnitController actingUnit)
	{
		IdleAnimations(actingUnit.PlayersArmy);

		foreach (UnitController unit in BattlefieldUnits.UnitsOf(actingUnit.PlayersArmy))
		{
			unit.Targetable = !unit.ImmunitiesToActions.Contains(actionType);
		}
		CancelButton.GetComponent<SpriteRenderer>().enabled = true;
		CancelButton.GetComponent<Collider2D>().enabled = true;
		CancelButton.transform.GetChild(0).gameObject.SetActive(true);
	}

	private void CancelAction()
	{
		foreach (UnitController unit in BattlefieldUnits.Units)
		{
			unit.Targetable = false;
		}
		
		if (ActiveUnit.PlayersArmy)
		{
			Player.Play(ActiveUnit);
		}
		else
		{
			Enemy.Play(ActiveUnit);
		}
	}

	private void ExecuteTargetingAction(UnitController unitController)
	{
		foreach (UnitController unit in BattlefieldUnits.Units)
		{
			unit.Targetable = false;
		}
		
		AdvanceUnit();
	}
	
	#endregion

	private void RemoveUnit(UnitController unit)
	{
		BattlefieldUnits.Remove(unit);
	}

	private void IdleAnimations(bool playersArmy)
	{
		PlayerController playerController = (playersArmy ? Player : Enemy) as PlayerController;
		if (playerController != null)
		{
			playerController.AnimationIdle();
		}
	}
	
	#region Battle flow control
	
	private void AdvanceUnit()
	{
		StartCoroutine(AdvanceUnitCoroutine());
	}

	private IEnumerator AdvanceUnitCoroutine()
	{
		yield return new WaitForSeconds(PauseBetweenUnits);
		if (NextUnit())
		{
			if (ActiveUnit.PlayersArmy)
			{
				Player.Play(ActiveUnit);
			}
			else
			{
				Enemy.Play(ActiveUnit);
			}
			CancelButton.GetComponent<SpriteRenderer>().enabled = false;
			CancelButton.GetComponent<Collider2D>().enabled = false;
			CancelButton.transform.GetChild(0).gameObject.SetActive(false);
		}
		else AdvanceTurn();
	}

	private void AdvanceTurn()
	{
		//foreach (UnitController unit in BattlefieldUnits.Units) { unit.HasPlayed = false; }
		Player.AdvanceTurn();
		Enemy.AdvanceTurn();
		AdvanceUnit();
	}

	private bool NextUnit()
	{
		List<UnitController> maxInitiativeUnits = new List<UnitController>();
		int maxInitiative = int.MinValue;
		//bool player = false, enemy = false;

		foreach (UnitController unit in BattlefieldUnits.Units)
		{
			if (unit.HasPlayed) continue;
			if (maxInitiative == unit.Initiative)
			{
				maxInitiativeUnits.Add(unit);
				//if (unit.PlayersArmy) player = true;
				//else enemy = true;
			}
			else if (maxInitiative < unit.Initiative)
			{
				maxInitiative = unit.Initiative;
				//player = unit.PlayersArmy;
				//enemy = !unit.PlayersArmy;
				maxInitiativeUnits = new List<UnitController> {unit};
			}
		}

		/*if (player && enemy)
		{
			List<Unit> newMaxInitiativeUnits = new List<Unit>();
			newMaxInitiativeUnits.AddRange(RandomUtil.RandomEvent(0.5)
				? maxInitiativeUnits.Where(unit => unit.PlayersArmy)
				: maxInitiativeUnits.Where(unit => !unit.PlayersArmy));
			maxInitiativeUnits = newMaxInitiativeUnits;
		}*/

		if (maxInitiativeUnits.Count == 0) return false;
		ActiveUnit = RandomUtil.RandomElement(maxInitiativeUnits);
		return true;
	}
	
	#endregion
}

[Serializable]
public class BattlefieldUnits
{
	#region Helper classes

	[Serializable]
	public class Place
	{
		public bool PlayersArmy;
		public Transform Transform;

		public enum BattlePosition
		{
			FrontTop,
			BackTop,
			FrontMiddle,
			BackBottom,
			FrontBottom
		}

		public BattlePosition Position;
	}

	[Serializable]
	public class BattlefieldUnit
	{
		public Place Place;
		public UnitController Unit;
	}

	#endregion

	public List<BattlefieldUnit> PlaceUnitList;

	public IEnumerable<UnitController> Units
	{
		get { return from placeUnit in PlaceUnitList where placeUnit.Unit != null select placeUnit.Unit; }
	}

	public IEnumerable<UnitController> UnitsOf(bool player)
	{
		return from placeUnit in PlaceUnitList
			where placeUnit.Unit != null && placeUnit.Place.PlayersArmy == player
			select placeUnit.Unit;
	}

	public IEnumerable<UnitController> UnitsInRange(UnitController unit)
	{
		int range = unit.Parameters.Range;
		Place place = (from placeUnit in PlaceUnitList where placeUnit.Unit == unit select placeUnit.Place).First();
		if (place.Position == Place.BattlePosition.BackBottom || place.Position == Place.BattlePosition.BackTop)
			range--;
		IEnumerable<BattlefieldUnit> enemyFirstLine = from placeUnit in PlaceUnitList
			where placeUnit.Place.PlayersArmy != unit.PlayersArmy &&
			      (placeUnit.Place.Position == Place.BattlePosition.FrontTop ||
			       placeUnit.Place.Position == Place.BattlePosition.FrontMiddle ||
			       placeUnit.Place.Position == Place.BattlePosition.FrontBottom)
			select placeUnit;
		if (!enemyFirstLine.Any())
		{
			range++;
		}
		switch (range)
		{
			case 0: return new List<UnitController>();
			case 1:
				return from placeUnit in PlaceUnitList
					where placeUnit.Unit != null &&
					      placeUnit.Place.PlayersArmy ^ place.PlayersArmy &&
					      (placeUnit.Place.Position == Place.BattlePosition.FrontBottom ||
					       placeUnit.Place.Position == Place.BattlePosition.FrontMiddle ||
					       placeUnit.Place.Position == Place.BattlePosition.FrontTop)
					select placeUnit.Unit;
			default:
				return from placeUnit in PlaceUnitList
					where placeUnit.Unit != null &&
					      placeUnit.Place.PlayersArmy ^ place.PlayersArmy
					select placeUnit.Unit;
		}
	}

	public void Remove(UnitController unit)
	{
		BattlefieldUnit battlefieldUnit =
			(from placeUnit in PlaceUnitList where placeUnit.Unit == unit select placeUnit).First();
		battlefieldUnit.Unit = null;

		#region Check if end of battle

		bool playersArmy = battlefieldUnit.Place.PlayersArmy;
		bool end = true;
		foreach (BattlefieldUnit placeUnit in PlaceUnitList)
		{
			if (placeUnit.Place.PlayersArmy == playersArmy && placeUnit.Unit != null)
			{
				end = false;
			}
		}

		if (end) EventPool.Trigger(EventTypes.EndBattle, !playersArmy);

		#endregion
	}

	public void SetUnitAllegiances()
	{
		foreach (BattlefieldUnit placeUnit in PlaceUnitList)
		{
			if (placeUnit.Unit != null)
				placeUnit.Unit.PlayersArmy = placeUnit.Place.PlayersArmy;
		}
	}

	public IEnumerable<PlaceOnBattlefield> NeighbouringPlaces(UnitController unit)
	{
		Place place = (from placeUnit in PlaceUnitList where placeUnit.Unit == unit select placeUnit.Place).First();
		List<Place.BattlePosition> neiPositions = null;
		switch (place.Position)
		{
			case Place.BattlePosition.FrontTop:
				neiPositions = new List<Place.BattlePosition> {Place.BattlePosition.BackTop};
				break;
			case Place.BattlePosition.BackTop:
				neiPositions = new List<Place.BattlePosition> {Place.BattlePosition.FrontTop, Place.BattlePosition.FrontMiddle};
				break;
			case Place.BattlePosition.FrontMiddle:
				neiPositions = new List<Place.BattlePosition> {Place.BattlePosition.BackTop, Place.BattlePosition.BackBottom};
				break;
			case Place.BattlePosition.BackBottom:
				neiPositions = new List<Place.BattlePosition> {Place.BattlePosition.FrontMiddle, Place.BattlePosition.FrontBottom};
				break;
			case Place.BattlePosition.FrontBottom:
				neiPositions = new List<Place.BattlePosition> {Place.BattlePosition.BackBottom};
				break;
		}
		return from placeUnit in PlaceUnitList
			where neiPositions.Contains(placeUnit.Place.Position) && placeUnit.Place.PlayersArmy == unit.PlayersArmy
			select placeUnit.Place.Transform.GetComponent<PlaceOnBattlefield>();
	}

	public void MoveTo(Transform placeTransform, UnitController unit)
	{
		BattlefieldUnit battlefieldUnit = (from placeUnit in PlaceUnitList
			where placeUnit.Place.Transform == placeTransform
			select placeUnit).First();
		UnitController otherUnit = battlefieldUnit.Unit;
		if (otherUnit != null)
		{
			BattlefieldUnit battlefieldUnitOrigin = (from placeUnit in PlaceUnitList
				where placeUnit.Unit == unit
				select placeUnit).First();
			battlefieldUnitOrigin.Unit = otherUnit;
			battlefieldUnitOrigin.Place.Transform.GetComponent<PlaceOnBattlefield>().PlaceUnitHere(otherUnit.transform);
		}
		battlefieldUnit.Unit = unit;
		placeTransform.GetComponent<PlaceOnBattlefield>().PlaceUnitHere(unit.transform);
	}
}