using Util;

public class UpliftingDance : UnitAction
{
	public int Duration = 1;
	public int Increase = 2;
	
	protected override void DefaultAction()
	{
		BattlefieldUnits battlefieldUnits = InfoPool.Request<BattlefieldUnits>("Battlefield units");
		foreach (UnitController unit in battlefieldUnits.UnitsOf(_unitController.PlayersArmy))
		{
			UnitController sameStupidUnit = unit;
			sameStupidUnit.Initiative += Increase;
			
			sameStupidUnit.StatusEffects.Add(new StatusEffect
			{
				Type = StatusEffectType.Eager,
				OwnTurns = Duration,
				GlobalTurns = -1,
				RemoveEffect = () => sameStupidUnit.Initiative -= Increase
			});
		}

		EventPool.Trigger(EventTypes.ActionNoTarget, _unitController.PlayersArmy);
	}
}