using UnityEngine;

namespace Util
{
	public class DeactivateMethod : MonoBehaviour
	{
		private void Deactivate()
		{
			gameObject.SetActive(false);
		}
	}
}