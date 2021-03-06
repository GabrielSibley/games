﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Defines function and operation of a Machine
public interface IMachineRule {

	int NumInPorts{ get; }
	int NumOutPorts{ get; }
	IMachineRule FreshCopy(); //Return a copy of the rule with no process state information
	void BindPorts(IList<Dock> inPorts, IList<Dock> outPorts);
}
