using System.Collections.Generic;
using UnityEngine.Events;
using Util;

public class Precision : TargetingUnitAction
{
	public int Duration = 1;
	public float BlindSuccessRate = 0.3f;

	protected override void DefaultAction()
	{
		EventPool.Trigger(EventTypes.ActionEnemyTarget, ActionType.Precision, _unitController);
	}

	protected override void DefaultTarget(UnitController unitToTarget)
	{
		List<TargetingUnitAction> unitActions = unitToTarget.GetTargetActionsWithTag("weapon based");
		List<UnityAction<UnitController>> oldActions = new List<UnityAction<UnitController>>();
		
		foreach (TargetingUnitAction unitAction in unitActions)
		{
			UnityAction<UnitController> oldAction = unitAction.Target;
			unitAction.Target = delegate(UnitController unitController)
			{
				if (RandomUtil.RandomEvent(BlindSuccessRate))
				{
					oldAction(unitController);
				}
			};
			oldActions.Add(oldAction);
		}

		unitToTarget.StatusEffects.Add(new StatusEffect
		{
			Type = StatusEffectType.Blind,
			OwnTurns = Duration + 1,
			GlobalTurns = -1,
			RemoveEffect = delegate
			{
				for (int i = 0; i < unitActions.Count; i++)
				{
					unitActions[i].Target = oldActions[i];
				}
			}
		});
	}
}