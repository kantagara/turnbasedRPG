using Util;

public class AdvanceOrders : AvatarAction
{
	protected override void DefaultAction()
	{
		BattlefieldUnits battlefieldUnits = InfoPool.Request<BattlefieldUnits>("Battlefield units");
		foreach (UnitController unit in battlefieldUnits.UnitsOf(_avatarController.Player))
		{
			UnitController sameStupidUnit = unit;
			sameStupidUnit.Attack += 1;
			sameStupidUnit.StatusEffects.Add(new StatusEffect
			{
				Type = StatusEffectType.AttackUp,
				OwnTurns = -1,
				GlobalTurns = 1,
				RemoveEffect = delegate { sameStupidUnit.Attack -= 1; }
			});
		}
			
		EventPool.Trigger(EventTypes.ActionNoTarget, _avatarController.Player);
		//gameObject.SetActive(false);
	}
}
