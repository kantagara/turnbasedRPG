using UnityEngine;
using UnityEngine.UI;
using Util;

public class UIController : MonoBehaviour
{
	private void OnEnable()
	{
		EventPool.StartListening<bool>(EventTypes.EndBattle, EndBattle);
	}

	private void EndBattle(bool playerWon)
	{
		Text text = GetComponent<Text>();
		if (playerWon)
		{
			text.text = "Victory";
			text.color = Color.green;
		}
		else
		{
			text.text = "Defeat";
			text.color = Color.red;
		}
	}
}