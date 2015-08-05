using UnityEngine;
using System.Collections;

[PrefabManager]
public class ProduceRuleDisplay : MonoBehaviour, IMachineRuleDisplay {

	public FourFeatureDisplay featureDisplay;

	public void Display(Machine mach, Vector2 pos)
	{
		var rule = (ProduceRule)mach.Rule;
		featureDisplay.Display(rule.Production.Features, pos);
	}
}
