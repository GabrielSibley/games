using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Produces an infinite supply of the given crate.
//Cannot take crates.
public class ProduceRule : IMachineRule {

	public Crate Production;

	public int NumInPorts{ get { return 0; } }
	public int NumOutPorts{ get { return 1; } }

	private FourFeatureDisplay display;

	public ProduceRule(Crate produces)
	{
		Production = produces;
	}

	private void Process(Port port, Grabber grabber)
	{
		grabber.Dispatch(new Crate(Production), port);
	}

	public IMachineRule FreshCopy()
	{
		return new ProduceRule(new Crate(Production));
	}

	public void BindPorts(IList<Port> inPorts, IList<Port> outPorts)
	{
		outPorts[0].OnGrabberDocked += Process;
	}
}
