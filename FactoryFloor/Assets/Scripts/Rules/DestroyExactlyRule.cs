using UnityEngine;
using System.Collections;

//Destroys matching crates that enter it.
public class DestroyExactlyRule : IMachineRule {

	public int NumInPorts{ get { return 1; } }
	public int NumOutPorts{ get { return 0; } }

	private Crate filter;
	private FourFeatureDisplay display;

	public DestroyExactlyRule(Crate filter)
	{
		this.filter = filter;
	}

	public bool TryPutCrate(Crate crate)
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
	public bool TryGetCrate(out Crate crate)
	{
		crate = null;
		return false;
	}
	public void UpdateDisplay(Machine machine, Vector2 machinePosition)
	{
		if(display == null)
		{
			display = Object.Instantiate(PrefabManager.FourFeatureDisplay) as FourFeatureDisplay;
		}
		display.Display (filter.Features, machinePosition);
	}

	public IMachineRule FreshCopy()
	{
		return new DestroyExactlyRule(new Crate(filter));
	}
}
