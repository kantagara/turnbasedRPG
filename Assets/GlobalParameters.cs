using UnityEngine;
using Util;

public class GlobalParameters : MonoBehaviour
{
	public int DefendBonus;

	private int GetDefendBonus()
	{
		return DefendBonus;
	}

	private void OnEnable()
	{
		InfoPool.Provide("defend bonus", GetDefendBonus);
	}

	private void OnDisable()
	{
		InfoPool.Unprovide("defend bonus", GetDefendBonus);
	}
}