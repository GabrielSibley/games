using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Machine {

	//Doesn't need to add up to 1
	private static float[] sizeWeights = new float[]{
		1, //size 1
		4, //size 2
		8, //size 3
		10, //size 4
		4, //size 5
		1  //size 6
	};

	public IMachineRule Rule;
	public Simulation Simulation;
	public List<MachinePart> Parts = new List<MachinePart>();
	public List<MachinePort> Ports = new List<MachinePort>();

	//True if machine is placed on the floor
	public bool OnFloor {
		get { return Origin.HasValue; }
	}

	//Root point on floor (game grid). Machines not on the floor have a null Origin.
	public Vector2i? Origin
	{
		get { return origin; }
	}
	private Vector2i? origin;

	public Machine()
	{
	}

	public bool HasPartAt(Vector2i offset)
	{
		return Parts.Exists (part => part.Offset == offset);
	}

	public void Update(float deltaTime)
	{
		//Do nothing for now
	}

	public bool CanMoveToOrigin(Vector2i newOrigin)
	{
		if(OnFloor)
		{
			Debug.LogError ("Machine is already on floor!");
			return false;
		}
		foreach(MachinePart part in Parts)
		{
			if(!Simulation.Floor.IsOpenSpace(part.Offset + newOrigin))
			{
				return false;
			}
		}
		return true;
	}

	//Assumes CanMoveToOrigin has already passed
	public void AddToFloor(Vector2i newOrigin)
	{
		origin = newOrigin;
        Simulation.OnMachineMoved(this);
	}

	public void RemoveFromFloor()
	{
		origin = null;
        Simulation.OnMachineMoved(this);
		//TODO: Destroy crane links
	}

	public void GeneratePartLayout()
	{
		//Part layout
		int size = RandomEx.RandomIndexWeighted(sizeWeights) + 1;
		List<Vector2i> openLocations = new List<Vector2i>();
		openLocations.Add(new Vector2i(){x = 0, y = 0});
		int xMin = 0, xMax = 0, yMin = 0, yMax = 0;
		const int maxGridDimension = 5;
		for(int i = 0; i < size; i++)
		{
			if(openLocations.Count == 0)
			{
				Debug.LogError ("Machine tried to spawn too large!");
				break;
			}
			int locIndex = Random.Range (0, openLocations.Count);
			Vector2i loc = openLocations[locIndex];
			openLocations.RemoveAt(locIndex);
			int machWidth = xMax - xMin + 1;
			int machHeight = yMax - yMin + 1;
			if(HasPartAt(loc)
			   || (machWidth >= maxGridDimension && (loc.x < xMin || loc.x > xMax))
			   || (machHeight >= maxGridDimension && (loc.y < yMin || loc.y > yMax))
			   )
			{
				i--; //retry
			}
			else{
				MachinePart part = new MachinePart();
				part.Offset = loc;
				AddPart(part);
				
				xMin = Mathf.Min(loc.x, xMin);
				xMax = Mathf.Max (loc.x, xMax);
				yMin = Mathf.Min (loc.y, yMin);
				yMax = Mathf.Max (loc.y, yMax);
				
				var dirs = new Vector2i[]{Vector2i.Up, Vector2i.Down, Vector2i.Left, Vector2i.Right};
				for(int j = 0; j < dirs.Length; j++){
					Vector2i newOpenLoc = loc + dirs[j];
					if(!HasPartAt(newOpenLoc))
					{
						openLocations.Add (newOpenLoc);
					}
				}
			}
		}
		//Recenter parts
		Vector2i partCenterOffset = new Vector2i(xMin+(xMax-xMin) / 2, yMin+(yMax-yMin)/2);
		for(int i = 0; i < Parts.Count; i++)
		{
			Parts[i].Offset -= partCenterOffset;
		}
		//TODO: Will break if >4xMachineSize ports?
		for(int i = 0; i < Rule.NumInPorts; i++)
		{
			AddInPort();
		}
		for(int i = 0; i < Rule.NumOutPorts; i++)
		{
			AddOutPort();
		}
		LayoutPorts();
	}

	private void AddPart(MachinePart part)
	{
		Parts.Add(part);
		part.Machine = this;
	}

	private MachinePort AddInPort()
	{
		MachinePort newPort = new MachinePort(PortType.In);
		newPort.Machine = this;
		Ports.Add (newPort);
		return newPort;
	}
	
	private MachinePort AddOutPort()
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
	private void LayoutPorts()
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
}
