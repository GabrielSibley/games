using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Machine : IUpdatable {
	public static List<Machine> PlacedMachines = new List<Machine>(); //TODO: Remove from list on destroy

	public IMachineRule Rule;
	public List<MachinePart> Parts = new List<MachinePart>();
	public List<MachinePort> Ports = new List<MachinePort>();
	//True if machine is rooted (exists on the game grid).
	public bool Rooted {
		get { return Origin.HasValue; }
		set {
			if(!value)
			{
				Origin = null;
			}
			else
			{
				throw new System.ArgumentException ("Can't set true directly");
			}
		}
	}
	//Root point on game grid. Machines not on the game grid have a null Origin.
	public Vector2i? Origin
	{
		get { return _origin; }
		set {
			if(_origin.HasValue && value == null)
			{
				PlacedMachines.Remove (this);
			}
			if(!_origin.HasValue && value != null)
			{
				PlacedMachines.Add (this);
			}
			_origin = value;
			ConfigPartsForOriginTile(Floor.GetTile(value));
		}
	}
	private Vector2i? _origin;
	
	private MachineDisplay display;

	public Machine()
	{
		Ticker.Instance.Add (this);
	}

	public bool HasPartAt(Vector2i offset)
	{
		return Parts.Exists (part => part.Offset == offset);
	}

	public void Update(float deltaTime)
	{
		//Do nothing for now
	}

	public void InitSimDisplay()
	{
		display = new MachineDisplay(this);
	}

	public bool CanMoveToOrigin(Vector2i newOrigin)
	{
		Tile originTile = Floor.GetTile(newOrigin);
		if(originTile == null)
		{
			return false;
		}
		foreach(MachinePart part in Parts)
		{
			Tile targetTile = originTile.GetTileAtOffset(part.Offset);
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
		List<Port> inPorts = new List<Port>();
		List<Port> outPorts = new List<Port>();
		foreach(Port p in Ports)
		{
			if(p.Type == PortType.In)
			{
				inPorts.Add (p);
			}
			else if(p.Type == PortType.Out)
			{
				outPorts.Add (p);
			}
			else
			{
				Debug.LogError ("Unknown port type");
			}
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

		Rule.BindPorts(inPorts, outPorts);
	}


	private void ConfigPartsForOriginTile(Tile tile)
	{
		if(tile == null)
		{
			foreach(MachinePart part in Parts){
				part.MoveToTile(null);
			}
		}
		else{
			foreach(MachinePart part in Parts)
			{
				part.MoveToTile(tile.GetTileAtOffset(part.Offset));
			}
		}
	}
}
