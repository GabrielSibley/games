using UnityEngine;
using System.Collections;

public class MatchReplaceRuleDisplay : MonoBehaviour {

	public SpriteRenderer matchRenderer;
	public SpriteRenderer replaceRenderer;

	public void Display(MatchReplaceRule rule, Machine machine, Vector2 machinePosition)
	{
		transform.position = new Vector3(machinePosition.x, machinePosition.y, -15);
		matchRenderer.sprite = rule.MatchFeature.GetSprite();
		replaceRenderer.sprite = rule.ReplaceFeature.GetSprite ();
	}
}
