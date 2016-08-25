using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[PrefabManager()]
public class MachineDisplay : MonoBehaviour, IDraggable {

	Machine machine;
	List<MachinePartDisplay> machinePartDisplays = new List<MachinePartDisplay>();
	List<PortDisplay> portDisplays = new List<PortDisplay>();
	MachineRuleDisplay ruleDisplay;

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

	public void Init(Machine mach)
	{
        if(machine != mach)
        {
            Cleanup();
        }
        machine = mach;

		foreach(var part in mach.Parts)
		{
			var display = Object.Instantiate(PrefabManager.MachinePartDisplay) as MachinePartDisplay;
			display.Part = part;
            display.transform.SetParent(transform);
            display.transform.localPosition = FloorView.FloorToWorldVector(part.Offset);
			display.UpdateSubrenderers();
			machinePartDisplays.Add (display);
		}
			
		foreach(var port in mach.Ports)
		{
			var display = Object.Instantiate(PrefabManager.PortDisplay) as PortDisplay;
			port.SetDisplay (display);
            display.transform.SetParent(transform);
            display.transform.localPosition = (Vector3)FloorView.FloorToWorldVector(port.Offset) + new Vector3(0, 0, -25);
			portDisplays.Add (display);
		}
			
		ruleDisplay = Object.Instantiate(MachineRuleDisplayUtil.GetDisplayPrefabForRule(mach.Rule)) as MachineRuleDisplay; //TODO: Kind of broken (?)
        ruleDisplay.transform.SetParent(transform);
        ruleDisplay.transform.localPosition = (Vector3)FloorView.FloorToWorldVector(mach.Parts[0].Offset) + new Vector3(0, 0, -10);
	}

    private void Cleanup()
    {
        foreach(var partDisplay in machinePartDisplays)
        {
            Destroy(partDisplay.gameObject);
        }
        machinePartDisplays.Clear();
        foreach(var portDisplay in portDisplays)
        {
            Destroy(portDisplay.gameObject);
        }
        portDisplays.Clear();
        if (ruleDisplay)
        {
            Destroy(ruleDisplay.gameObject);
        }
        machine = null;
    }

	public void OnDragStart(Vector2 inPos){
		oldOrigin = machine.Origin;
		GameMode.Current = GameMode.Mode.MoveMachine;
		machine.RemoveFromFloor();
		CollisionEnabled = false;
        transform.position = inPos;
        Display(machine);
	}

	//Failed drag -- return to old position
	public void OnDragEnd(Vector2 inPos)
	{
		if(!oldOrigin.HasValue)
		{
			throw new System.Exception("Dropped machine with no old origin -- what now?");
		}
		machine.AddToFloor(oldOrigin.Value);
		CollisionEnabled = true;
		if(GameMode.Current == GameMode.Mode.MoveMachine)
		{
			GameMode.Current = GameMode.Mode.SelectMachine;
		}
	}

	public void OnDragged(Vector2 inPos)
	{
        transform.position = inPos;
	}


	public void Display(Machine machine)
	{
        if(machine != this.machine)
        {
            Init(machine);
        }
		foreach(MachinePartDisplay part in machinePartDisplays)
		{
			part.Display();
		}
		foreach(PortDisplay port in portDisplays)
		{
			port.Display();
		}
		if(ruleDisplay != null){
			ruleDisplay.Display(machine);
		}
	}
}
