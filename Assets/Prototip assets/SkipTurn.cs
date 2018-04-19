using Util;

public class SkipTurn : UnitAction
{
	protected override void DefaultAction()
	{
		EventPool.Trigger(EventTypes.ActionNoTarget, _unitController.PlayersArmy);
	}
}