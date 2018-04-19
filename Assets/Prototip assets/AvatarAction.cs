using UnityEngine;
using UnityEngine.Events;
using Util;

public abstract class AvatarAction : MonoBehaviour
{
	public string TipText;
	public bool Active;
	public UnityAction Action { get; set; }
	
	protected AvatarController _avatarController;
	//protected Animator _anim;

	protected virtual void Start()
	{
		_avatarController = GetComponentInParent<AvatarController>();
		//_anim = GetComponentInParent<Animator>();
		Action = DefaultAction;
		TipText = TipText == "" ? name : TipText;
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