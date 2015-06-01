using UnityEngine;
using System.Collections;

public class MachinePort : Port {

	public MachinePart Part;
	public Machine Machine { get { return Part.Machine; } }

	private PortDisplay display;

	public MachinePort(PortType type) : base(type){
		//just call superconstructor
	}

	public override Vector2 WorldPosition{
		get {
			if(display)
			{
				return display.transform.position;
			}
			return Vector2.zero;
		}
	}

	public void UpdateDisplay()
	{
		display.UpdateDisplay ();
		if(Pipe != null)
		{
			Pipe.UpdateDisplay();
		}
	}
	
	public void SetDisplay(PortDisplay display)
	{
		this.display = display;
		display.Port = this;
	}
}
