using Util;

public class PotionOfHealth : TargetingAvatarAction
{
	public int Healing = 5;
	
	protected override void DefaultAction()
	{
		EventPool.Trigger(EventTypes.ActionFriendlyTargetAvatar, ActionType.PotionOfHealth, _avatarController);
		gameObject.SetActive(false);
	}
	
	protected override void DefaultTarget(UnitController unitToTarget)
	{
		unitToTarget.Health += 5;
	}
}
