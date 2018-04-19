using Util;

public class Analyze : TargetingUnitAction
{
	public int Duration = 1;
	public int AmountToReduceDefense = 2;
	
	protected override void DefaultAction()
	{
		EventPool.Trigger(EventTypes.ActionEnemyTarget, ActionType.Analyze, _unitController);
	}

	protected override void DefaultTarget(UnitController unitToTarget)
	{
		int actualReduction = unitToTarget.ReduceDefenseBy(AmountToReduceDefense);
		
		unitToTarget.StatusEffects.Add(new StatusEffect
		{
			Type = StatusEffectType.DefDown,
			OwnTurns = -1,
			GlobalTurns = Duration,
			RemoveEffect = () => unitToTarget.Defense += actualReduction
		});
	}
}
