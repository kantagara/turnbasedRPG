using UnityEngine.Events;
using Util;

public class Stalk : TargetingUnitAction
{
	public int Duration = 1;
	public float Strength = 2.2f;
	
	protected override void DefaultAction()
	{
		EventPool.Trigger(EventTypes.ActionEnemyTarget, ActionType.Stalk, _unitController);
	}

	protected override void DefaultTarget(UnitController unitToTarget)
	{
		UnityAction<int> oldHit = unitToTarget.Hit;
		unitToTarget.Hit = delegate(int incomingAttack)
		{
			oldHit((int) (Strength * incomingAttack));
		};
		
		unitToTarget.StatusEffects.Add(new StatusEffect
		{
			Type = StatusEffectType.Vulnerable,
			OwnTurns = -1,
			GlobalTurns = Duration,
			RemoveEffect = () => unitToTarget.Hit = oldHit
		});
	}
}