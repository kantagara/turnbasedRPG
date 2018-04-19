using UnityEngine.Events;
using Util;

public abstract class TargetingAvatarAction : AvatarAction
{
	public UnityAction<UnitController> Target;

	protected override void Start()
	{
		base.Start();
		UnityAction oldAction = Action;
		Action = delegate
		{
			oldAction();
			EventPool.StartListening<UnitController>(EventTypes.Targeted, Target);
		};
		Target = delegate(UnitController unitController)
		{
			DefaultTarget(unitController);
			EventPool.StopListening<UnitController>(EventTypes.Targeted, Target);
		};
	}

	protected abstract void DefaultTarget(UnitController unitToTarget);
}