using Util;

public class DefensiveTactics : AvatarAction
{
	protected override void DefaultAction()
	{
		BattlefieldUnits battlefieldUnits = InfoPool.Request<BattlefieldUnits>("Battlefield units");
		foreach (UnitController unit in battlefieldUnits.UnitsOf(_avatarController.Player))
		{
			UnitController sameStupidUnit = unit;
			sameStupidUnit.Defense += 1;
			sameStupidUnit.StatusEffects.Add(new StatusEffect
			{
				Type = StatusEffectType.DefUp,
				OwnTurns = -1,
				GlobalTurns = 1,
				RemoveEffect = delegate { sameStupidUnit.Defense -= 1; }
			});
		}
			
		EventPool.Trigger(EventTypes.ActionNoTarget, _avatarController.Player);
		//gameObject.SetActive(false);
	}
}
