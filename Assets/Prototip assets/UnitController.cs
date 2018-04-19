using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Util;

public class UnitController : MonoBehaviour
{
	#region Non-mutable fields
	
	public ClassParameters Parameters;
	public bool PlayersArmy { get; set; }
	private Animator _anim;
	
	#endregion

	#region Mutable fields
	
	public bool HasPlayed { get; set; }
	public bool Targetable { get; set; }
	public int MaxHealth { get; set; }
	private int _health;
	public int Health
	{
		get { return _health; }
		set
		{
			_health = Mathf.Clamp(value, 0, MaxHealth);
			if (_health == 0) Die();
		}
	}
	public int Attack { get; set; }
	public int Defense { get; set; }
	public int Initiative { get; set; }
	public List<UnityAction> PlayActions;
	//public UnityAction<ActionType, UnitController> Target;
	public UnityAction<int> Hit;
	public UnityAction GlobalTurn;
	public List<UnitAction> UnitActions { get; private set; }
	public HashSet<ActionType> ImmunitiesToActions { get; private set; }
	public List<StatusEffect> StatusEffects { get; private set; }
	
	#endregion

	#region Public methods

	public void Play()
	{
		for (int i = PlayActions.Count - 1; i >= 0; i--)
		{
			PlayActions[i]();
		}
	}

	public int ReduceAttackBy(int reduction)
	{
		int original = Attack;
		Attack -= reduction;
		return original - Attack;
	}
	
	public int ReduceDefenseBy(int reduction)
	{
		int original = Defense;
		Defense -= reduction;
		return original - Defense;
	}

	public List<TargetingUnitAction> GetTargetActionsWithTag(string actionTag)
	{
		/*
		 * Ajme mega debagaa...
		 * 
		 * if (UnitActions == null)
		{
			print("nemam akcije");
		}
		IEnumerable<UnitAction> ns = (from unitAction in UnitActions
			where unitAction.Tags.Contains(actionTag)
			select unitAction);
		foreach (var action in ns)
		{
			foreach (var tag1 in action.Tags)
			{
				print(tag1);
			}
		}*/
		return (from unitAction in UnitActions
			where unitAction.Tags.Contains(actionTag) && unitAction is TargetingUnitAction
			select (TargetingUnitAction) unitAction).ToList();
	}

	#endregion

	#region Private methods
	
	private void Awake()
	{
		_anim = GetComponent<Animator>();
		UnitActions = new List<UnitAction>();
		ImmunitiesToActions = new HashSet<ActionType>();
		StatusEffects = new List<StatusEffect>();

		MaxHealth = Parameters.Health;
		Health = Parameters.Health;
		Attack = Parameters.Attack;
		Defense = Parameters.Defense;
		Initiative = Parameters.Initiative;
		PlayActions = new List<UnityAction> {DefaultPlay};
		//Target = DefaultTarget;
		Hit = DefaultHit;
		GlobalTurn = DefaultGlobalTurn;
	}

	private void OnMouseDown()
	{
		if (!Targetable) return;
		EventPool.Trigger(EventTypes.Targeted, this);
	}

	private void DefaultPlay()
	{
		#region Handle status effect changes

		List<StatusEffect> obsoleteEffects = new List<StatusEffect>();
		foreach (StatusEffect statusEffect in StatusEffects)
		{
			if (--statusEffect.OwnTurns == 0)
			{
				statusEffect.RemoveEffect();
				obsoleteEffects.Add(statusEffect);
			}
		}
		foreach (StatusEffect obsoleteEffect in obsoleteEffects)
		{
			StatusEffects.Remove(obsoleteEffect);
		}

		#endregion

		HasPlayed = true;
		//_anim.SetTrigger("play");
	}
	
	/*private void DefaultTarget(ActionType actionType, UnitController unit = null)
	{
		switch (actionType)
		{
			case ActionType.Attack:
				int damage = Mathf.Max(0, unit.Attack - Defense);
				Health -= damage;
				break;
			case ActionType.DeepWounds:
				
				break;
			case ActionType.Precision:
				
				break;
			case ActionType.PotionOfHealth:
				Health += 5;
				break;
			default:
				print("Please implement reaction to following ability: " + actionType);
				break;
		}
	}*/

	private void DefaultHit(int incomingAttack)
	{
		int damage = Mathf.Max(0, incomingAttack - Defense);
		Health -= damage;
	}

	private void DefaultGlobalTurn()
	{
		#region Handle status effect changes

		List<StatusEffect> obsoleteEffects = new List<StatusEffect>();
		foreach (StatusEffect statusEffect in StatusEffects)
		{
			if (--statusEffect.GlobalTurns == 0)
			{
				statusEffect.RemoveEffect();
				obsoleteEffects.Add(statusEffect);
			}
		}
		foreach (StatusEffect obsoleteEffect in obsoleteEffects)
		{
			StatusEffects.Remove(obsoleteEffect);
		}

		#endregion

		HasPlayed = false;
	}

	private void Die()
	{
		EventPool.Trigger(EventTypes.UnitDeath, this);
		_anim.SetTrigger("die");
	}
	
	#endregion
}
