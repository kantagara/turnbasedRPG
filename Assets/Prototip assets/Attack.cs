using Util;

public class Attack : TargetingUnitAction
{
	protected override void Start()
	{
		base.Start();
		Tags.Add("weapon based");
	}

	protected override void DefaultAction()
	{
		EventPool.Trigger(EventTypes.ActionEnemyTarget, ActionType.Attack, _unitController);
	}

	protected override void DefaultTarget(UnitController unitToTarget)
	{
		unitToTarget.Hit(_unitController.Attack);
	}
}