using UnityEngine;
using System.Collections;

//Destroys matching crates that enter it.
public class DestroyExactlyRule : IMachineRule {

	public int NumInPorts{ get { return 1; } }
	public int NumOutPorts{ get { return 0; } }
	public Crate Filter { get { return filter; } }

	private Crate filter;

	public DestroyExactlyRule(Crate filter)
	{
		this.filter = filter;
	}

	public bool TryPutCrate(Port port, Crate crate)
	{
		if(crate.Features.Count == filter.Features.Count)
		{
			for(int i = 0; i < crate.Features.Count; i++)
			{
				if(crate.Features[i] != filter.Features[i])
				{
					return false;
				}
			}
			return true;
		}
		return  false;

	}
	public bool TryGetCrate(Port port, out Crate crate)
	{
		crate = null;
		return false;
	}

	public IMachineRuleDisplay GetDisplay()
	{
		return Object.Instantiate(PrefabManager.DestroyExactlyRuleDisplay) as DestroyExactlyRuleDisplay;
	}

	/*public void UpdateDisplay(Machine machine, Vector2 machinePosition)
	{
		display.Display (filter.Features, machinePosition);
	}*/

	public IMachineRule FreshCopy()
	{
		return new DestroyExactlyRule(new Crate(filter));
	}
}
