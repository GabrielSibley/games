using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpawnMachineButton : MonoBehaviour, IInputReceiver {

	public enum RuleType { Produce, Destroy, MatchReplace };

	public RuleType type;

	private IMachineRule rule;

	public void Start()
	{
		if(type == RuleType.MatchReplace)
		{
			rule = MatchReplaceRule.GetRandom();
			MatchReplaceRuleDisplay display = Instantiate(PrefabManager.MatchReplaceRuleDisplay) as MatchReplaceRuleDisplay;
			display.Display (rule as MatchReplaceRule, null, transform.position);
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
			mach.PickUp();
		}
	}
}
