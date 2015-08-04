using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Machine {
	public static Machine MachineOnCursor;
	public static List<Machine> PlacedMachines = new List<Machine>();

	public List<MachinePart> Parts = new List<MachinePart>();
	public IMachineRule Rule;

	private Tile rootTile;
	private Tile oldRootTile;

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
		foreach(MachinePart part in Parts)
		{
			part.CollisionEnabled = false;
		}
	}

	public void Drop(Tile tile)
	{
		if(MachineOnCursor != this)
		{
			Debug.LogError ("Was not carrying this machine on cursor");
		}
		GameMode.Current = GameMode.Mode.SelectMachine;
		MachineOnCursor = null;
		foreach(MachinePart part in Parts)
		{
			part.CollisionEnabled = true;
		}
		if(CanRootToTile(tile))
		{
			RootToTile(tile);
		}
		else{
			RootToTile(oldRootTile);
		}
	}

	public bool CanRootToTile(Tile tile)
	{
		if(tile == null)
		{
			return false;
		}
		foreach(MachinePart part in Parts)
		{
			Tile partTile = tile.GetTileAtOffset(part.Offset);
			//machine part would lie off board
			if(partTile == null)
			{
				return false;
			}
			//machine overlaps other, different thing
			if(partTile.features.Count > 0)
			{
				if(partTile.features[0] is MachinePartDisplay)
				{
					if(((MachinePartDisplay)partTile.features[0]).Machine != this)
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
		foreach(MachinePart part in Parts)
		{
			part.DisplayAtWithOffset(position);
		}
		Rule.UpdateDisplay(this, position);
	}

	public void AddPart(MachinePart part)
	{
		Parts.Add(part);
		part.Machine = this;
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
