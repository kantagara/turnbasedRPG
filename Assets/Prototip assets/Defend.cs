using Util;

public class Defend : UnitAction
{
	private int _defendBonus;

	protected override void Start()
	{
		base.Start();
		_defendBonus = InfoPool.Request<int>("defend bonus");
	}

	protected override void DefaultAction()
	{
		_unitController.Defense += _defendBonus;
		_unitController.StatusEffects.Add(new StatusEffect
		{
			Type = StatusEffectType.DefUp,
			OwnTurns = 1,
			GlobalTurns = -1,
			RemoveEffect = delegate { _unitController.Defense -= _defendBonus; }
		});
		EventPool.Trigger(EventTypes.ActionNoTarget, _unitController.PlayersArmy);
	}
}