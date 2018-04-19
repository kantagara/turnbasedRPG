using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Util;

public abstract class UnitAction : MonoBehaviour
{
	public string TipText;
	public bool Active;
	public UnityAction Action { get; set; }
	public List<string> Tags { get; private set; }

	protected UnitController _unitController;

	//protected Animator _anim;

	protected virtual void Start()
	{
		Tags = new List<string>();
		_unitController = GetComponentInParent<UnitController>();
		//_anim = GetComponentInParent<Animator>();
		Action = DefaultAction;
		_unitController.UnitActions.Add(this);
		TipText = TipText == "" ? name.Replace(" button", "") : TipText;
	}

	protected abstract void DefaultAction();

	private void OnMouseDown()
	{
		if (!Active) return;
		Action();
	}

	private void OnMouseEnter()
	{
		EventPool.Trigger(EventTypes.ActionTipSet, TipText);
	}

	private void OnMouseExit()
	{
		EventPool.Trigger(EventTypes.ActionTipReset, TipText);
	}
}