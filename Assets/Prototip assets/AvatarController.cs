using UnityEngine;

public abstract class AvatarController : MonoBehaviour
{
	public bool Player;
	
	public abstract void Play(UnitController unit);

	public abstract void AdvanceTurn();
}