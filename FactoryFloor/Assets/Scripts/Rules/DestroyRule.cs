using UnityEngine;
using System.Collections;

//Destroys all crates that enter it.
public class DestroyRule : IMachineRule {

	public int NumInPorts{ get { return 1; } }
	public int NumOutPorts{ get { return 0; } }

	public bool TryPutCrate(Port port, Crate crate)
	{
		return true;
	}
	public bool TryGetCrate(Port port, out Crate crate)
	{
		crate = null;
		return false;
	}
	public void UpdateDisplay(Machine machine, Vector2 machinePosition)
	{
		//no display
	}

	public IMachineRule FreshCopy()
	{
		return new DestroyRule();
	}
}
