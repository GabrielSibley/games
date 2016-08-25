using UnityEngine;
using System.Collections;

[PrefabManager]
public class ProduceRuleDisplay : MachineRuleDisplay {

	public FourFeatureDisplay featureDisplay;

	public override void Display(Machine mach)
	{
		var rule = (ProduceRule)mach.Rule;
		featureDisplay.Display(rule.Production.Features);
	}
}
