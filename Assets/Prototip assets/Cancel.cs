using UnityEngine;
using Util;

public class Cancel : MonoBehaviour
{
	private void OnMouseDown()
	{
		EventPool.Trigger(EventTypes.Cancel);
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<Collider2D>().enabled = false;
		transform.GetChild(0).gameObject.SetActive(false);
	}
}