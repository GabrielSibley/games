using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Replaces all features that match a target to another feature
public class MatchReplaceRule : IMachineRule {
	//Gets a well-formed randomly built MatchReplaceRule
	public static MatchReplaceRule GetRandom()
	{
		//If match color/shape is specific, replace color/shape (resp) cannot be wild
		//Match color/shape cannot both be wild
		//Replace color/shape cannot both be wild
		//Replace color/shape cannot both be the same as Match color/shape
		MatchReplaceRule rule = new MatchReplaceRule();
		bool wildMatchShape = Random.value < 0.7f;
		bool wildMatchColor = Random.value < 0.7f;
		if(wildMatchShape && wildMatchColor)
		{
			if(Random.value < 0.5f)
			{
				wildMatchShape = false;
			}
			else
			{
				wildMatchColor = false;
			}
		}
		rule.MatchFeature = new CrateFeature(
			wildMatchColor ? CrateFeature.ColorWildcard : Random.Range (0, CrateFeature.NumNormalColors),
			wildMatchShape ? CrateFeature.ShapeWildcard : Random.Range (0, CrateFeature.NumNormalShapes)
		);
		bool wildReplaceShape = wildMatchShape && Random.value < 0.7f;
		bool wildReplaceColor = wildMatchColor && Random.value < 0.7f;
		//Enforce both replace cannot be wild:
		if(wildReplaceShape && wildReplaceColor)
		{
			if(Random.value < 0.5f)
			{
				wildReplaceShape = false;
			}
			else
			{
				wildReplaceColor = false;
			}
		}
		do{
			rule.ReplaceFeature = new CrateFeature(
				wildReplaceColor ? CrateFeature.ColorWildcard : Random.Range (0, CrateFeature.NumNormalColors),
				wildReplaceShape ? CrateFeature.ShapeWildcard : Random.Range (0, CrateFeature.NumNormalShapes)
			);
		}
		while(rule.MatchFeature == rule.ReplaceFeature); //reroll until not the same
		return rule;
	}

	public CrateFeature MatchFeature;	//This
	public CrateFeature ReplaceFeature; //is replaced by this

	public int NumInPorts{ get { return 1; } }
	public int NumOutPorts{ get { return 1; } }
	
	private Crate heldCrate;
	private MatchReplaceRuleDisplay display;

	public MatchReplaceRule()
	{
	}

	public MatchReplaceRule(CrateFeature match, CrateFeature replace)
	{
		MatchFeature = match;
		ReplaceFeature = replace;
	}

	public bool TryPutCrate(Port port, Crate crate)
	{
		if(heldCrate == null)
		{
			heldCrate = crate;
			Process(crate);
			return true;
		}
		else
		{
			return false;
		}
	}
	public bool TryGetCrate(Port port, out Crate crate)
	{
		if(heldCrate == null)
		{
			crate = null;
			return false;
		}
		else
		{
			crate = heldCrate;
			heldCrate = null;
			return true;
		}
	}
	public void UpdateDisplay(Machine machine, Vector2 machinePosition)
	{
		if(!display)
		{
			display = Object.Instantiate(PrefabManager.MatchReplaceRuleDisplay) as MatchReplaceRuleDisplay;
		}
		display.Display(this, machine, machinePosition);
	}

	public IMachineRule FreshCopy()
	{
		return new MatchReplaceRule(MatchFeature, ReplaceFeature);
	}

	public bool SameEffect(MatchReplaceRule other)
	{
		return this.MatchFeature == other.MatchFeature && this.ReplaceFeature == other.ReplaceFeature;
	}

	private void Process(Crate crate)
	{
		for(int i = 0; i < crate.Features.Count; i++)
		{
			if(crate.Features[i].Matches(MatchFeature))
			{
				CrateFeature newFeature = new CrateFeature(crate.Features[i]);
				if(ReplaceFeature.Color != CrateFeature.ColorWildcard)
				{
					newFeature.Color = ReplaceFeature.Color;
				}
				if(ReplaceFeature.Shape != CrateFeature.ShapeWildcard)
				{
					newFeature.Shape = ReplaceFeature.Shape;
				}
				crate.Features[i] = newFeature;
			}
		}
	}

}
