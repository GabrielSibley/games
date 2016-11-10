using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Destroys all crates that enter it.
public class DestroyRule : IMachineRule {

	public int NumInPorts{ get { return 1; } }
	public int NumOutPorts{ get { return 0; } }

	public void Process(Dock port, Grabber grabber)
	{
		grabber.Dispatch(null, port);
	}

	public IMachineRule FreshCopy()
	{
		return new DestroyRule();
	}

	public void BindPorts(IList<Dock> inPorts, IList<Dock> outPorts)
	{
		inPorts[0].OnGrabberDocked += Process;
	}
}
