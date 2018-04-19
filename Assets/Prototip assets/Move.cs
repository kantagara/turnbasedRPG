using UnityEngine;
using Util;

public class Move : MonoBehaviour
{
	private void OnMouseDown()
	{
		EventPool.Trigger(EventTypes.Move);
	}
}
