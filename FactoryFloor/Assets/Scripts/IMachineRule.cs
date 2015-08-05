using UnityEngine;
using System.Collections;

//Defines function and operation of a Machine
public interface IMachineRule {

	int NumInPorts{ get; }
	int NumOutPorts{ get; }
	bool TryPutCrate(Port port, Crate crate);
	bool TryGetCrate(Port port, out Crate crate);
	IMachineRuleDisplay GetDisplay();
	IMachineRule FreshCopy(); //Return a copy of the rule with no process state information
}
