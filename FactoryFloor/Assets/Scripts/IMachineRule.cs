using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Defines function and operation of a Machine
public interface IMachineRule {

	int NumInPorts{ get; }
	int NumOutPorts{ get; }
	IMachineRuleDisplay GetDisplay();
	IMachineRule FreshCopy(); //Return a copy of the rule with no process state information
	void BindPorts(IList<Port> inPorts, IList<Port> outPorts);
}
