using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Takes two crates and produces one crate with the combined contents.
//If the two crates have more than 4 features combined, two crates are produced
//one with the first 4 features, and a second with the remainder.
//taken from the "front" of the second crate.
public class PackRule : IMachineRule {
	
	public int NumInPorts{ get { return 2; } }
	public int NumOutPorts{ get { return 1; } }

	private Dock inputA, inputB, output;
	private Crate outputOverflow;

	private void OnGrabberDocked(Dock port, Grabber grabber)
	{
		if(port == output && outputOverflow != null)
		{
			grabber.Dispatch(outputOverflow, port);
			outputOverflow = null;
		}
		else if(inputA.DockedGrabbers.Count > 0
		   && inputB.DockedGrabbers.Count > 0
		   && output.DockedGrabbers.Count > 0)
		{
			Crate inCrateA = inputA.DockedGrabbers[0].HeldCrate;
			Crate inCrateB = inputB.DockedGrabbers[0].HeldCrate;
			int featuresMoved = Mathf.Min (Crate.MaxFeatures - inCrateA.Features.Count, inCrateB.Features.Count);
			for(int i = 0; i < featuresMoved; i++)
			{
				inCrateA.Features.Add(inCrateB.Features[i]);
			}
			if(featuresMoved < inCrateB.Features.Count)
			{
				inCrateB.Features.RemoveRange (0, featuresMoved);
				outputOverflow = inCrateB;
			}
			inputA.DockedGrabbers[0].Dispatch(null, inputA);
			inputB.DockedGrabbers[0].Dispatch(null, inputB);
			output.DockedGrabbers[0].Dispatch(inCrateA, output);
		}
	}

	public IMachineRule FreshCopy()
	{
		return new PackRule();
	}

	public void BindPorts(IList<Dock> inPorts, IList<Dock> outPorts)
	{
		inputA = inPorts[0];
		inputB = inPorts[1];
		output = outPorts[0];
		inputA.OnGrabberDocked += OnGrabberDocked;
		inputB.OnGrabberDocked += OnGrabberDocked;
		output.OnGrabberDocked += OnGrabberDocked;
		inputA.Effect = DockEffect.First;
		inputB.Effect = DockEffect.Last;
	}
}
