using Util;

public class PotionOfPower : TargetingUnitAction
{
	public int Duration = 1;
	public int AmountToIncreaseAttack = 3;
	
	protected override void DefaultAction()
	{
		EventPool.Trigger(EventTypes.ActionFriendlyTarget, ActionType.PotionOfPower, _unitController);
	}

	protected override void DefaultTarget(UnitController unitToTarget)
	{
		unitToTarget.Attack += AmountToIncreaseAttack;
		
		unitToTarget.StatusEffects.Add(new StatusEffect
		{
			Type = StatusEffectType.AttackUp,
			OwnTurns = Duration + 1,
			GlobalTurns = -1,
			RemoveEffect = () => unitToTarget.Attack -= AmountToIncreaseAttack
		});
	}
}