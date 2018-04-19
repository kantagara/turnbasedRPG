using UnityEngine;
using UnityEngine.UI;
using Util;

public class ActionTipText : MonoBehaviour
{
	private Text _text;

	private void Start()
	{
		_text = GetComponent<Text>();
	}

	private void OnEnable()
	{
		EventPool.StartListening<string>(EventTypes.ActionTipSet, SetTip);
		EventPool.StartListening(EventTypes.ActionTipReset, ResetTip);
	}
	
	private void OnDisable()
	{
		EventPool.StopListening<string>(EventTypes.ActionTipSet, SetTip);
		EventPool.StopListening(EventTypes.ActionTipReset, ResetTip);
	}

	private void SetTip(string tip)
	{
		_text.text = tip;
	}

	private void ResetTip()
	{
		_text.text = "";
	}
}