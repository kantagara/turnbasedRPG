using UnityEngine.Events;
using Util;

public class DeepWounds : TargetingUnitAction
{
	public int Duration = 2;
	public int BleedDamage = 1;
	
	protected override void Start()
	{
		base.Start();
		Tags.Add("weapon based");
	}
	
	protected override void DefaultAction()
	{
		EventPool.Trigger(EventTypes.ActionEnemyTarget, ActionType.DeepWounds, _unitController);
	}

	protected override void DefaultTarget(UnitController unitToTarget)
	{
		UnityAction bleedAction = () => unitToTarget.Health -= BleedDamage;
		
		unitToTarget.PlayActions.Add(bleedAction);
		
		unitToTarget.StatusEffects.Add(new StatusEffect
		{
			Type = StatusEffectType.Bleed,
			OwnTurns = Duration,
			GlobalTurns = -1,
			RemoveEffect = () => unitToTarget.PlayActions.Remove(bleedAction)
		});
	}
}