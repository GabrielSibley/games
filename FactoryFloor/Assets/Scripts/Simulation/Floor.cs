using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Floor {

	//Do not modify directly, use Add/RemoveMachine
	public HashSet<Machine> Machines = new HashSet<Machine>();
	private FloorSpace[] floorSpaces;

	public int Width {get; private set;}
	public int Height {get; private set;}
	public Simulation Simulation {get; private set;}
	
	public Floor(Simulation sim, int width, int height)
	{
		Simulation = sim;
		Width = width;
		Height = height;
		floorSpaces = new FloorSpace[width * height];
		for(int y = 0; y < height; y++)
		{
			for(int x = 0; x < width; x++)
			{
				floorSpaces[y*width + x] = new FloorSpace(new Vector2i(x, y));
			}
		}
	}

	private FloorSpace GetFloorSpace(Vector2i pos)
	{
		return GetFloorSpace(pos.x, pos.y);
	}

	private FloorSpace GetFloorSpace(int x, int y)
	{
		if(x < 0 || x >= Width || y < 0 || y >= Height)
		{
			return null;
		}
		return floorSpaces[y * Width + x];
	}

	public void AddMachine(Machine machine)
	{
		Machines.Add (machine);
        Simulation.Tick(machine);
		RecalcSpace();
	}
	
	public void RemoveMachine(Machine machine)
	{
		Machines.Remove (machine);
        Simulation.Untick(machine);
		RecalcSpace();
	}
	
	public bool IsOpenSpace(Vector2i pos)
	{
		FloorSpace sp = GetFloorSpace(pos);
		if(sp == null)
		{
			return false;
		}
		return !sp.ContainsMachine;
	}


	private void RecalcSpace()
	{
		//Clear
		foreach(FloorSpace space in floorSpaces)
		{
			space.ContainsMachine = false;
		}

		//Add
		foreach(Machine m in Machines)
		{
			foreach(MachinePart p in m.Parts)
			{
				Vector2i pos = m.Origin.Value + p.Offset;
				FloorSpace space = GetFloorSpace(pos);
				if(space.ContainsMachine)
				{
					Debug.LogError("Space " + pos + " already contains machine");
				}
				space.ContainsMachine = true;
			}
		}
	}
}
