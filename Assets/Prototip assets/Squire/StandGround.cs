using Util;

public class StandGround : UnitAction
{
	public int Duration = 2;
	public int Increase = 2;
	
	protected override void DefaultAction()
	{
		_unitController.Defense += Increase;
		_unitController.StatusEffects.Add(new StatusEffect
		{
			Type = StatusEffectType.DefUp,
			OwnTurns = Duration,
			GlobalTurns = -1,
			RemoveEffect = delegate { _unitController.Defense -= Increase; }
		});
		EventPool.Trigger(EventTypes.ActionNoTarget, _unitController.PlayersArmy);
	}
}
