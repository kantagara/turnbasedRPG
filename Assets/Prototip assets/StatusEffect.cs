using UnityEngine.Events;

public class StatusEffect
{
	public StatusEffectType Type;
	public int OwnTurns;
	public int GlobalTurns;
	public UnityAction RemoveEffect;
}

public enum StatusEffectType
{
	DefUp,
	DefDown,
	Blind,
	AttackUp,
	Bleed,
	Vulnerable,
	Regenerating,
	Eager
}