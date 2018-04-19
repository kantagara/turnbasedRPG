using UnityEngine.Events;
using Util;

public class Regenerate : TargetingUnitAction
{
	public int Duration = 2;
	public int Healing = 2;
	
	protected override void DefaultAction()
	{
		EventPool.Trigger(EventTypes.ActionFriendlyTarget, ActionType.Regenerate, _unitController);
	}

	protected override void DefaultTarget(UnitController unitToTarget)
	{
		UnityAction regenerate = () => unitToTarget.Health += Healing;
		unitToTarget.PlayActions.Add(regenerate);
		unitToTarget.StatusEffects.Add(new StatusEffect
		{
			Type = StatusEffectType.Regenerating,
			OwnTurns = Duration,
			GlobalTurns = -1,
			RemoveEffect = () => unitToTarget.PlayActions.Remove(regenerate)
		});
	}
}
