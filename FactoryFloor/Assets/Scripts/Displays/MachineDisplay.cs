using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MachineDisplay {

	List<MachinePartDisplay> machinePartDisplays;
	List<PortDisplay> portDisplays;
	IMachineRuleDisplay ruleDisplay;
	bool initialized;

	public bool CollisionEnabled{
		set
		{
			if(machinePartDisplays != null)
			{
				foreach(var partDisplay in machinePartDisplays)
				{
					partDisplay.GetComponent<Collider>().enabled = value;
				}
			}
		}
	}

	public void Display(Machine mach, Vector2 pos)
	{
		InitDisplay(mach);
		foreach(MachinePartDisplay part in machinePartDisplays)
		{
			part.Display(pos);
		}
		foreach(PortDisplay port in portDisplays)
		{
			port.Display(pos);
		}
		if(ruleDisplay != null){
			ruleDisplay.Display(mach, pos);
		}
	}

	private void InitDisplay(Machine mach)
	{
		if(!initialized)
		{
			machinePartDisplays = new List<MachinePartDisplay>();
			foreach(var part in mach.Parts)
			{
				var display = Object.Instantiate(PrefabManager.MachinePartDisplay) as MachinePartDisplay;
				display.Part = part;
				display.UpdateSubrenderers();
				machinePartDisplays.Add (display);
			}

			portDisplays = new List<PortDisplay>();
			foreach(var port in mach.Ports)
			{
				var display = Object.Instantiate(PrefabManager.PortDisplay) as PortDisplay;
				port.SetDisplay (display);
				portDisplays.Add (display);
			}

			ruleDisplay = mach.Rule.GetDisplay();
		}
		initialized = true;
	}
}
