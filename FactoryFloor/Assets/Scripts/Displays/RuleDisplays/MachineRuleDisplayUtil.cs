using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MachineRuleDisplayUtil {

	private static Dictionary<System.Type, MachineRuleDisplay> displayForRule;

	public static MachineRuleDisplay GetDisplayPrefabForRule(IMachineRule rule)
	{
		if(rule == null)
		{
			throw new System.ArgumentNullException("rule");
		}
		if(displayForRule == null)
		{
			//Build dictionary
			displayForRule = new Dictionary<System.Type, MachineRuleDisplay>(){
				{typeof(PackRule), PrefabManager.PackRuleDisplay},
				{typeof(UnpackRule), PrefabManager.UnpackRuleDisplay},
				{typeof(MatchReplaceRule), PrefabManager.MatchReplaceRuleDisplay},
				{typeof(DestroyRule), PrefabManager.DestroyRuleDisplay},
			};
		}

		MachineRuleDisplay match;
		if(displayForRule.TryGetValue(rule.GetType(), out match))
		{
			return match;
		}
		Debug.LogError ("No display for rule of type " + rule.GetType());
		return null;
	}
}
