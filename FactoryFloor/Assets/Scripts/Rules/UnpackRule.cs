using UnityEngine;
using System.Collections;

//Takes one crate and produces two crates, one with the final feature of the input
//crate and one with the rest of the features.
//If the input crate has only one feature, the output crate goes into the "split off"
//output.
public class UnpackRule : IMachineRule {
	
	public int NumInPorts{ get { return 1; } }
	public int NumOutPorts{ get { return 2; } }

	public Port outputA, outputB;
	private Crate outputBufferA, outputBufferB;

	public bool TryPutCrate(Port port, Crate crate)
	{
		if(outputBufferA == null && outputBufferB == null)
		{
			// processing goes here
			return true;
		}
		return false;
	}
	public bool TryGetCrate(Port port, out Crate crate)
	{
		if(port == outputA && outputBufferA != null)
		{
			crate = outputBufferA;
			outputBufferA = null;
			return true;
		}
		if(port == outputB && outputBufferB != null)
		{
			crate = outputBufferB;
			outputBufferB = null;
			return true;
		}
		crate = null;
		return false;
	}
	public IMachineRuleDisplay GetDisplay()
	{
		return Object.Instantiate(PrefabManager.UnpackRuleDisplay) as UnpackRuleDisplay;
	}
	
	public IMachineRule FreshCopy()
	{
		return new UnpackRule();
	}
}
