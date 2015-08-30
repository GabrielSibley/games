using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Takes one crate and produces two crates, one with the final feature of the input
//crate and one with the rest of the features.
//If the input crate has only one feature, the output crate goes into the "split off"
//output.
public class UnpackRule : IMachineRule {
	
	public int NumInPorts{ get { return 1; } }
	public int NumOutPorts{ get { return 2; } }

	private Port input, outputSingle, outputRemainder;
	
	private void OnGrabberDocked(Port port, Grabber grabber)
	{
		if(input.DockedGrabbers.Count > 0
		   && outputSingle.DockedGrabbers.Count > 0
		   && (input.DockedGrabbers[0].HeldCrate.Features.Count == 1 || outputRemainder.DockedGrabbers.Count > 0))
		{
			Crate inCrate = input.DockedGrabbers[0].HeldCrate;
			if(inCrate.Features.Count == 1)
			{
				outputSingle.DockedGrabbers[0].Dispatch(inCrate, outputSingle);
				input.DockedGrabbers[0].Dispatch(null, input);
			}
			else
			{
				Crate single = new Crate();
				int last = inCrate.Features.Count - 1;
				single.Features = new List<CrateFeature>(){inCrate.Features[last]};
				inCrate.Features.RemoveAt (last);
				input.DockedGrabbers[0].Dispatch(null, input);
				outputSingle.DockedGrabbers[0].Dispatch(single, outputSingle);
				outputRemainder.DockedGrabbers[0].Dispatch(inCrate, outputRemainder);
			}
		}
	}
	public IMachineRuleDisplay GetDisplay()
	{
		return Object.Instantiate(PrefabManager.UnpackRuleDisplay) as UnpackRuleDisplay;
	}
	
	public IMachineRule FreshCopy()
	{
		return new UnpackRule();
	}

	public void BindPorts(IList<Port> inPorts, IList<Port> outPorts)
	{
		input = inPorts[0];
		outputSingle = outPorts[0];
		outputRemainder = outPorts[1];
		input.OnGrabberDocked += OnGrabberDocked;
		outputSingle.OnGrabberDocked += OnGrabberDocked;
		outputRemainder.OnGrabberDocked += OnGrabberDocked;
	}
}
