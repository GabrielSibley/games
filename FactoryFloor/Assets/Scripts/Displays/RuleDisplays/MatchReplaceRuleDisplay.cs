using UnityEngine;
using System.Collections;

[PrefabManager]
public class MatchReplaceRuleDisplay : MachineRuleDisplay {

	public SpriteRenderer matchRenderer;
	public SpriteRenderer replaceRenderer;

	public override void Display(Machine machine)
	{
		Display((MatchReplaceRule)machine.Rule);
	}

	public void Display(MatchReplaceRule rule)
	{
		matchRenderer.sprite = rule.MatchFeature.GetSprite();
		replaceRenderer.sprite = rule.ReplaceFeature.GetSprite ();
	}
}
