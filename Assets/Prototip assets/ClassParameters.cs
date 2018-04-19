using UnityEngine;

[CreateAssetMenu(menuName = "Class parameters")]
public class ClassParameters : ScriptableObject
{
	public enum UnitType
	{
		Melee,
		Ranged,
		Support
	}

	public string ClassName;
	public UnitType Type;
	public int Health;
	public int Range;
	public int Attack;
	public int Defense;
	public int Initiative;
}