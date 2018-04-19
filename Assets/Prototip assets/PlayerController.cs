using System.Collections.Generic;
using UnityEngine;

public class PlayerController : AvatarController
{
	private Animator _anim;
	private Dictionary<UnitController, Animator> _unitAnimators;

	private void Awake()
	{
		_anim = GetComponent<Animator>();
	}

	public override void Play(UnitController unit)
	{
		unit.Play();
		_unitAnimators[unit].SetTrigger("play");
		_anim.SetTrigger("play");
	}

	public override void AdvanceTurn()
	{
		foreach (UnitController unit in _unitAnimators.Keys)
		{
			unit.GlobalTurn();
		}
	}

	#region Animation

	public void SetUnits(IEnumerable<UnitController> unitControllers)
	{
		_unitAnimators = new Dictionary<UnitController, Animator>();
		foreach (UnitController unitController in unitControllers)
		{
			_unitAnimators.Add(unitController, unitController.GetComponent<Animator>());
		}
	}
	/*
	private void OnEnable()
	{
		EventPool.StartListening(EventTypes.ActionNoTarget, AnimationIdle);
		EventPool.StartListening<ActionType, UnitController>(EventTypes.ActionEnemyTarget, AnimationIdle);
		EventPool.StartListening<ActionType, AvatarController>(EventTypes.ActionFriendlyTarget, AnimationIdle);
		EventPool.StartListening<Action<ActionType, UnitController>>(EventTypes.Targeted, AnimationIdle);
	}

	private void OnDisable()
	{
		EventPool.StopListening(EventTypes.ActionNoTarget, AnimationIdle);
		EventPool.StopListening<ActionType, UnitController>(EventTypes.ActionEnemyTarget, AnimationIdle);
		EventPool.StopListening<ActionType, AvatarController>(EventTypes.ActionFriendlyTarget, AnimationIdle);
		EventPool.StopListening<Action<ActionType, UnitController>>(EventTypes.Targeted, AnimationIdle);
	}

	private void AnimationIdle(Action<ActionType, UnitController> arg0)
	{
		AnimationIdle();
	}

	private void AnimationIdle(ActionType arg0, AvatarController arg1)
	{
		AnimationIdle();
	}

	private void AnimationIdle(ActionType arg0, UnitController arg1)
	{
		AnimationIdle();
	}
*/
	public void AnimationIdle()
	{
		_anim.SetTrigger("idle");
		foreach (Animator animator in _unitAnimators.Values)
		{
			animator.SetTrigger("skip turn");
		}
	} 
	
	#endregion
}