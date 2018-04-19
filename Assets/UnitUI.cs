using System.Linq;
using UnityEngine;

public class UnitUI : MonoBehaviour
{
	public TextMesh AttackText;
	public TextMesh DefenseText;
	public TextMesh HealthText;
	public TextMesh InitiativeText;
	public TextMesh StatusEffectText;
	public SpriteRenderer UnitSpriteRenderer;
	private UnitController _unitController;

	private void Start()
	{
		_unitController = GetComponentInParent<UnitController>();
	}

	private void OnGUI()
	{
		AttackText.text = "Attack " + _unitController.Attack;
		DefenseText.text = "Defense " + _unitController.Defense;
		HealthText.text = "Health " + _unitController.Health + "/" + _unitController.MaxHealth;
		InitiativeText.text = "Initiative " + _unitController.Initiative;
		StatusEffectText.text = _unitController.StatusEffects.Aggregate("",
			(current, statusEffect) => current + statusEffect.Type.ToString() + System.Environment.NewLine);
		UnitSpriteRenderer.color = _unitController.HasPlayed ? Color.gray * 1.5f : Color.white;
		UnitSpriteRenderer.color -= _unitController.Targetable ? Color.red / 2 : Color.black;
	}
}