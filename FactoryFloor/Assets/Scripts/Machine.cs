using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Machine {
	public static Machine MachineOnCursor;

	public List<MachinePart> Parts = new List<MachinePart>();

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
			rootTile = tile;
			foreach(MachinePart part in Parts)
			{
				part.MoveToTile(tile.GetTileAtOffset(part.Offset));
			}
		}
	}

	public void DisplayAt(Vector2 position)
	{
		foreach(MachinePart part in Parts)
		{
			part.DisplayAtWithOffset(position);
		}
	}

	public void AddPart(MachinePart part)
	{
		Parts.Add(part);
		part.Machine = this;
	}
}
