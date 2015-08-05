using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Machine {
	public static Machine MachineOnCursor;
	public static List<Machine> PlacedMachines = new List<Machine>(); //TODO: Remove from list on destroy

	public IMachineRule Rule;
	public List<MachinePart> Parts = new List<MachinePart>();
	public List<MachinePort> Ports = new List<MachinePort>();

	private Tile rootTile;
	private Tile oldRootTile;
	private MachineDisplay display;

	public bool HasPartAt(Vector2i offset)
	{
		return Parts.Exists (part => part.Offset == offset);
	}

	public void PickUp(){
		if(MachineOnCursor != null)
		{
			Debug.LogError ("Already carrying a machine on cursor!");
			return;
		}
		PlacedMachines.Remove (this);
		GameMode.Current = GameMode.Mode.MoveMachine;
		MachineOnCursor = this;
		oldRootTile = rootTile;
		RootToTile(null);
		DisplayAt(InputManager.InputWorldPos);
		display.CollisionEnabled = false;
	}

	public void Drop(Tile tile)
	{
		if(MachineOnCursor != this)
		{
			Debug.LogError ("Was not carrying this machine on cursor");
		}
		GameMode.Current = GameMode.Mode.SelectMachine;
		MachineOnCursor = null;

		if(CanRootToTile(tile))
		{
			RootToTile(tile);
		}
		else{
			RootToTile(oldRootTile);
		}

		display.CollisionEnabled = true;
	}

	public bool CanRootToTile(Tile tile)
	{
		if(tile == null)
		{
			return false;
		}
		foreach(MachinePart part in Parts)
		{
			Tile targetTile = tile.GetTileAtOffset(part.Offset);
			//machine part would lie off board
			if(targetTile == null)
			{
				return false;
			}
			//machine overlaps other, different thing
			if(targetTile.features.Count > 0)
			{
				if(targetTile.features[0] is MachinePart)
				{
					if(((MachinePart)targetTile.features[0]).Machine != this)
					{
						return false;
					}
				}
				else{
					return false;
				}
			}
		}
		return true;
	}

	public void RootToTile(Tile tile)
	{
		if(tile == null)
		{
			rootTile = tile;
			foreach(MachinePart part in Parts){
				part.MoveToTile(null);
			}
		}
		else if(!CanRootToTile(tile))
		{
			Debug.LogError ("Cannot root machine to tile " + tile);
			return;
		}
		else{
			PlacedMachines.Add (this);
			rootTile = tile;
			foreach(MachinePart part in Parts)
			{
				part.MoveToTile(tile.GetTileAtOffset(part.Offset));
			}
			DisplayAt(tile.transform.position);
		}
	}

	public void DisplayAt(Vector2 position)
	{
		if(display == null)
		{
			display = new MachineDisplay();
		}
		display.Display(this, position);
	}

	public void AddPart(MachinePart part)
	{
		Parts.Add(part);
		part.Machine = this;
	}

	public MachinePort AddInPort()
	{
		MachinePort newPort = new MachinePort(PortType.In);
		newPort.Machine = this;
		Ports.Add (newPort);
		return newPort;
	}
	
	public MachinePort AddOutPort()
	{
		MachinePort newPort = new MachinePort(PortType.Out);
		newPort.Machine = this;
		Ports.Add (newPort);
		return newPort;
	}

	Vector2[] portOffsets = new Vector2[]{
		new Vector2(-0.25f, 0.25f), //UL
		new Vector2(0.25f, -0.25f), //LR
		new Vector2(0.25f, 0.25f),  //UR
		new Vector2(-0.25f, -0.25f), //LL
	};
	public void LayoutPorts()
	{
		Dictionary<MachinePart, int> partsWithPorts = new Dictionary<MachinePart, int>();
		foreach(Port p in Ports)
		{
			int partIndex = Parts.Count > 1 ? Random.Range (1, Parts.Count-1) : 0;
			MachinePart randomPart = Parts[partIndex];
			if(!partsWithPorts.ContainsKey(randomPart))
			{
				partsWithPorts[randomPart] = 1;
			}
			else
			{
				partsWithPorts[randomPart]++;
			}
			p.Offset = randomPart.Offset + portOffsets[partsWithPorts[randomPart] - 1];
		}
	}

	public bool TryPutCrate(Port port, Crate crate)
	{
		if(Rule != null)
		{
			return Rule.TryPutCrate(port, crate);
		}
		Debug.LogError ("Machine has no rule");
		return false;
	}

	public bool TryGetCrate(Port port, out Crate crate)
	{
		if(Rule != null)
		{
			return Rule.TryGetCrate(port, out crate);
		}
		Debug.LogError ("Machine has no rule");
		crate = null;
		return false;
	}
}
