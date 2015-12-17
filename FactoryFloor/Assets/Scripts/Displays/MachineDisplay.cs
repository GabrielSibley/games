using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MachineDisplay : IDraggable {

	Machine machine;
	List<MachinePartDisplay> machinePartDisplays;
	List<PortDisplay> portDisplays;
	IMachineRuleDisplay ruleDisplay;
	bool initialized;

	Vector2i? oldOrigin;

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

	public MachineDisplay(Machine mach)
	{
		if(!initialized)
		{
			machine = mach;
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

	public void OnDragStart(Vector2 inPos){
		oldOrigin = machine.Origin;
		GameMode.Current = GameMode.Mode.MoveMachine;
		machine.Rooted = false;
		CollisionEnabled = false;
		Display(inPos);
	}

	//Failed drag -- return to old position
	public void OnDragEnd(Vector2 inPos)
	{
		if(!oldOrigin.HasValue)
		{
			throw new System.Exception("Dropped machine with no old origin -- what now?");
		}
		machine.Origin = oldOrigin;
		CollisionEnabled = true;
		if(GameMode.Current == GameMode.Mode.MoveMachine)
		{
			GameMode.Current = GameMode.Mode.SelectMachine;
		}
	}

	public void OnDragged(Vector2 inPos)
	{
		Display(inPos);
	}


	public void Display(Vector2 pos)
	{
		foreach(MachinePartDisplay part in machinePartDisplays)
		{
			part.Display(pos);
		}
		foreach(PortDisplay port in portDisplays)
		{
			port.Display(pos);
		}
		if(ruleDisplay != null){
			ruleDisplay.Display(machine, pos);
		}
	}

	public void DisplayAtMachinePosition()
	{
		if(machine.Origin.HasValue)
		{
			Display(InputManager.SimToWorld(machine.Origin.Value));
		}
	}
}
