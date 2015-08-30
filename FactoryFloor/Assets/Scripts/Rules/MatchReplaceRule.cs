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

	private Port inPort, outPort;

	public MatchReplaceRule()
	{
	}

	public MatchReplaceRule(CrateFeature match, CrateFeature replace)
	{
		MatchFeature = match;
		ReplaceFeature = replace;
	}

	//Todo: This could explode if grabber can round-trip in one time step
	public void Process(Port port, Grabber grabber)
	{
		if(inPort.DockedGrabbers.Count > 0 && outPort.DockedGrabbers.Count > 0)
		{
			Grabber inGrabber = inPort.DockedGrabbers[0];
			Grabber outGrabber = outPort.DockedGrabbers[0];
			Crate crate = inGrabber.HeldCrate;
			DoMatchReplace(crate);
			outGrabber.Dispatch(crate, outPort);
			inGrabber.Dispatch(null, inPort);
		}
	}

	public IMachineRuleDisplay GetDisplay()
	{
		return Object.Instantiate(PrefabManager.MatchReplaceRuleDisplay) as MatchReplaceRuleDisplay;
	}

	public IMachineRule FreshCopy()
	{
		return new MatchReplaceRule(MatchFeature, ReplaceFeature);
	}

	public bool SameEffect(MatchReplaceRule other)
	{
		return this.MatchFeature == other.MatchFeature && this.ReplaceFeature == other.ReplaceFeature;
	}

	private void DoMatchReplace(Crate crate)
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

	public void BindPorts(IList<Port> inPorts, IList<Port> outPorts)
	{
		inPort = inPorts[0];
		outPort = outPorts[0];
		inPorts[0].OnGrabberDocked += Process;
		outPorts[0].OnGrabberDocked += Process;
	}

}
