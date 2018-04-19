using UnityEngine;
using Util;

public class PlaceOnBattlefield : MonoBehaviour
{
	private Transform _tr;
	private Collider2D _col;
	private SpriteRenderer _sprite;

	private void Awake()
	{
		_tr = transform;
		_col = GetComponent<Collider2D>();
		_sprite = GetComponent<SpriteRenderer>();
	}

	public void SetTargetable()
	{
		_col.enabled = true;
		_sprite.enabled = true;
	}

	public void SetUntargetable()
	{
		_col.enabled = false;
		_sprite.enabled = false;
	}

	private void OnMouseDown()
	{
		EventPool.Trigger(EventTypes.MoveTo, _tr);
	}
	
	public void PlaceUnitHere(Transform unitTransform)
	{
		unitTransform.position = _tr.position;
		unitTransform.rotation = _tr.rotation;
		unitTransform.parent = _tr;
	}
}