using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MachinePart {
	public Vector2i Offset;
	public Machine Machine;
	public List<Port> Ports = new List<Port>();

	private MachinePartDisplay display;

	public bool CollisionEnabled{
		set
		{
			if(display != null && display.collider)
			{
				display.collider.enabled = value;
			}
		}
	}

	public void MoveToTile(Tile tile)
	{
		InitDisplay();
		display.MoveToTile(tile);
		for(int i = 0; i < Ports.Count; i++)
		{
			Ports[i].UpdateDisplay();
		}
	}

	public void DisplayAtWithOffset(Vector2 position)
	{
		InitDisplay();
		display.transform.position = position + 
			Vector2.Scale (new Vector2(Offset.x, Offset.y), FloorLayout.TileSize);
		for(int i = 0; i < Ports.Count; i++)
		{
			Ports[i].UpdateDisplay();
		}
	}

	public Port AddInPort()
	{
		Port newPort = new Port(PortType.In);
		newPort.Part = this;
		Ports.Add (newPort);
		return newPort;
	}

	public Port AddOutPort()
	{
		Port newPort = new Port(PortType.Out);
		newPort.Part = this;
		Ports.Add (newPort);
		return newPort;
	}

	private void InitDisplay()
	{
		if(display == null)
		{
			display = GameObject.Instantiate(PrefabManager.MachinePartDisplay) as MachinePartDisplay;
			display.Part = this;
			display.UpdateTile();
			display.UpdatePorts();
		}
	}
}
