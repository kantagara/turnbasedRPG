using Util;

public class Bomb : AvatarAction
{
	protected override void DefaultAction()
	{
		BattlefieldUnits battlefieldUnits = InfoPool.Request<BattlefieldUnits>("Battlefield units");
		foreach (UnitController unit in battlefieldUnits.Units)
		{
			unit.Health -= 1;
		}

		EventPool.Trigger(EventTypes.ActionNoTarget, _avatarController.Player);
		gameObject.SetActive(false);
	}
}