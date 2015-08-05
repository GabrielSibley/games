using UnityEngine;
using System.Collections;

[PrefabManager]
public class DestroyExactlyRuleDisplay : MonoBehaviour, IMachineRuleDisplay {

	public FourFeatureDisplay featureDisplay;

	public void Display(Machine mach, Vector2 pos)
	{
		var rule = (DestroyExactlyRule)mach.Rule;
		featureDisplay.Display(rule.Filter.Features, pos);
	}
}
