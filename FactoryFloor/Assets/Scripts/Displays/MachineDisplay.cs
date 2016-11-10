using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[PrefabManager()]
public class MachineDisplay : MonoBehaviour, IBeginDragHandler, IDragHandler {

    Machine machine;
	List<MachinePartDisplay> machinePartDisplays = new List<MachinePartDisplay>();
	List<DockDisplay> portDisplays = new List<DockDisplay>();
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

        if(machine == null)
        {
            return;
        }

		foreach(var part in mach.Parts)
		{
			var display = Object.Instantiate(PrefabManager.MachinePartDisplay);
			display.Part = part;
            display.transform.SetParent(transform);
            display.transform.localPosition = FloorView.FloorToWorldVector(part.Offset);
			display.UpdateSubrenderers();
			machinePartDisplays.Add (display);
		}
			
		foreach(var port in mach.Ports)
		{
			var display = Object.Instantiate(PrefabManager.DockDisplay);
            display.Dock = port;
            display.transform.SetParent(transform);            
            display.transform.localPosition = (Vector3)FloorView.FloorToWorldVector(port.Offset) + new Vector3(0, 0, ZLayer.DockOffset);
			portDisplays.Add (display);
		}
			
		ruleDisplay = Object.Instantiate(MachineRuleDisplayUtil.GetDisplayPrefabForRule(mach.Rule)) as MachineRuleDisplay; //TODO: Kind of broken (?)
        ruleDisplay.transform.SetParent(transform);
        ruleDisplay.transform.localPosition = (Vector3)FloorView.FloorToWorldVector(mach.Parts[0].Offset) + new Vector3(0, 0, ZLayer.RuleOffset);
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
            ruleDisplay = null;
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
        if (!oldOrigin.HasValue)
        {
            throw new System.Exception("Dropped machine with no old origin -- what now?");
        }
        machine.AddToFloor(oldOrigin.Value);
        CollisionEnabled = true;
        if (GameMode.Current == GameMode.Mode.MoveMachine)
        {
            GameMode.Current = GameMode.Mode.SelectMachine;
        }
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
		foreach(DockDisplay port in portDisplays)
		{
			port.Display();
		}
		if(ruleDisplay != null){
			ruleDisplay.Display(machine);
		}
	}

    public void OnBeginDrag(PointerEventData eventData)
    {
        SimulationView.Instance.CarriedMachine = machine;        
    }

    public void OnDrag(PointerEventData eventData)
    {
        //do nothing
    }
}
