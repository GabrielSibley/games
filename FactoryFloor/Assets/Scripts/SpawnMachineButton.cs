using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class SpawnMachineButton : MonoBehaviour, IInputReceiver {
	
	public static List<MatchReplaceRule> existingMatchReplaceRules = new List<MatchReplaceRule>();

	public enum RuleType { Produce, Destroy, MatchReplace, Pack };

	public RuleType type;

	private IMachineRule rule;

	public void Start()
	{
		if(type == RuleType.MatchReplace)
		{
			MatchReplaceRule newRule; 
			do{
				newRule = MatchReplaceRule.GetRandom();
			}
			while(existingMatchReplaceRules.Exists (x => x.SameEffect (newRule)));
			existingMatchReplaceRules.Add (newRule);
			rule = newRule;
			MatchReplaceRuleDisplay display = Instantiate(PrefabManager.MatchReplaceRuleDisplay) as MatchReplaceRuleDisplay;
			display.Display (rule as MatchReplaceRule, null, transform.position);
		}
		if(type == RuleType.Pack)
		{
			rule = new PackRule();
			PackRuleDisplay display = Instantiate(PrefabManager.PackRuleDisplay) as PackRuleDisplay;
			display.Display(null, transform.position);
		}
	}

	public void OnInputDown(){
		if(!GameMode.ModeLocked)
		{
			if(type == RuleType.Produce)
			{
				Crate crate = new Crate();
				crate.Features.Add (CrateFeature.RandomNonWild());
				crate.Features.Add (CrateFeature.RandomNonWild());
				rule = new ProduceRule(crate);
			}
			if(type == RuleType.Destroy)
			{
				Crate crate = new Crate();
				crate.Features.Add (CrateFeature.RandomNonWild());
				crate.Features.Add (CrateFeature.RandomNonWild());
				rule = new DestroyExactlyRule(crate);
			}

			Machine mach = MachineTestSpawn.GetRandomMachineWithRule(rule.FreshCopy());
			if(type == RuleType.Pack)
			{
				var packRule = (PackRule)mach.Rule;
				Port[] ports = mach.Parts.SelectMany(x => x.Ports).Where (port => port.Type == PortType.In).ToArray();
				packRule.inputA = ports[0];
				packRule.inputB = ports[1];
			}
			mach.PickUp();
		}
	}
}
