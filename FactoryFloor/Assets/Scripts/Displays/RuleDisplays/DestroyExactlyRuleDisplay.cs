using UnityEngine;
using System.Collections;

[PrefabManager]
public class DestroyExactlyRuleDisplay : MachineRuleDisplay {

	public FourFeatureDisplay featureDisplay;

	public override void Display(Machine mach)
	{
		var rule = (DestroyExactlyRule)mach.Rule;
		featureDisplay.Display(rule.Filter.Features);
	}
}
