using UnityEngine;
using System.Collections;

public class MachinePort : Port {
	
	public Machine Machine { get; set; }
	public override bool IsReal{get{return true;}}

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

	public void SetDisplay(PortDisplay display)
	{
		//TODO: Fix overwriting non null display
		this.display = display;
		display.Port = this;
	}
}
