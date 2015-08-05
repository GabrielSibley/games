using UnityEngine;
using System.Collections;

//Takes two crates and produces one crate with the combined contents.
//If the two crates have more than 4 features combined, two crates are produced
//one with the first 4 features, and a second with the remainder.
//taken from the "front" of the second crate.
public class PackRule : IMachineRule {
	
	public int NumInPorts{ get { return 2; } }
	public int NumOutPorts{ get { return 1; } }

	public Port inputA, inputB;
	private Crate inputBufferA, inputBufferB;
	private Crate outputOverflow;

	public bool TryPutCrate(Port port, Crate crate)
	{
		if(outputOverflow != null)
		{
			return false;
		}
		if(port == inputA && inputBufferA == null)
		{
			inputBufferA = crate;
			return true;
		}
		if(port == inputB && inputBufferB == null)
		{
			inputBufferB = crate;
			return true;
		}
		return false;
	}
	public bool TryGetCrate(Port port, out Crate crate)
	{
		if(outputOverflow != null)
		{
			crate = outputOverflow;
			outputOverflow = null;
			return true;
		}
		else if(inputBufferA != null && inputBufferB != null)
		{
			int featuresMoved = Mathf.Max (Crate.MaxFeatures - inputBufferA.Features.Count, inputBufferB.Features.Count);
			crate = inputBufferA;
			for(int i = 0; i < featuresMoved; i++)
			{
				crate.Features.Add(inputBufferB.Features[i]);
			}
			if(featuresMoved < inputBufferB.Features.Count)
			{
				inputBufferB.Features.RemoveRange (0, featuresMoved);
				outputOverflow = inputBufferB;
			}
			inputBufferA = null;
			inputBufferB = null;
			return true;
		}
		else
		{
			crate = null;
			return false;
		}
	}
	public IMachineRuleDisplay GetDisplay()
	{
		return Object.Instantiate(PrefabManager.PackRuleDisplay) as PackRuleDisplay;
	}
	
	public IMachineRule FreshCopy()
	{
		return new PackRule();
	}
}
