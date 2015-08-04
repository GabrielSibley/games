using UnityEngine;
using System.Collections;

//Holds a single crate.
//Passes it unchanged from input to output.
public class PassRule : IMachineRule {

	public int NumInPorts{ get { return 1; } }
	public int NumOutPorts{ get { return 1; } }

	private Crate heldCrate;

	public bool TryPutCrate(Port port, Crate crate)
	{
		if(heldCrate == null)
		{
			heldCrate = crate;
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
		//no display
	}

	public IMachineRule FreshCopy()
	{
		return new PassRule();
	}
}
