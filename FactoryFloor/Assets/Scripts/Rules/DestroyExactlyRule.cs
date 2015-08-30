using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	public void Process(Port port, Grabber grabber)
	{
		Crate crate = grabber.HeldCrate;
		bool match = true;
		if(crate.Features.Count == filter.Features.Count)
		{
			for(int i = 0; i < crate.Features.Count; i++)
			{
				if(crate.Features[i] != filter.Features[i])
				{
					match = false;
				}
			}
		}
		else
		{
			match = false;
		}
		if(match)
		{
			grabber.Dispatch(null, port);
		}
	}

	public void BindPorts(IList<Port> inPorts, IList<Port> outPorts)
	{
		inPorts[0].OnGrabberDocked += Process;
	}

	public IMachineRuleDisplay GetDisplay()
	{
		return Object.Instantiate(PrefabManager.DestroyExactlyRuleDisplay) as DestroyExactlyRuleDisplay;
	}

	public IMachineRule FreshCopy()
	{
		return new DestroyExactlyRule(new Crate(filter));
	}
}
