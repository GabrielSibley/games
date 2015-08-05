using UnityEngine;
using System.Collections;

[PrefabManager]
public class MatchReplaceRuleDisplay : MonoBehaviour, IMachineRuleDisplay {

	public SpriteRenderer matchRenderer;
	public SpriteRenderer replaceRenderer;

	public void Display(Machine machine, Vector2 position)
	{
		Display((MatchReplaceRule)machine.Rule, position);
	}

	public void Display(MatchReplaceRule rule, Vector2 position)
	{
		transform.position = new Vector3(position.x, position.y, -15);
		matchRenderer.sprite = rule.MatchFeature.GetSprite();
		replaceRenderer.sprite = rule.ReplaceFeature.GetSprite ();
	}
}
