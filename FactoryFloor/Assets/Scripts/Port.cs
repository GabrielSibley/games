using UnityEngine;
using System.Collections;

public enum PortType {In, Out};

public class Port {

	public PortType Type;
	public Pipe Pipe;
	public MachinePart Part;
	public Machine Machine { get { return Part.Machine; } }

	private PortDisplay display;

	public Vector2 WorldPosition{
		get {
			if(display)
			{
				return display.transform.position;
			}
			return Vector2.zero;
		}
	}

	public Port(PortType t)
	{
		Type = t;
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
