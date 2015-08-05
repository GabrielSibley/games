using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MachinePart : TileFeature {
	public Vector2i Offset;
	public Machine Machine;

	public override void MoveToTile(Tile tile){
		if(CurrentTile != null){
			CurrentTile.features.Remove (this);
		}
		if(tile != null)
		{
			tile.features.Add (this);
		}
		CurrentTile = tile;
	}

	public override void RemoveFromTile()
	{
		if(CurrentTile != null)
		{
			CurrentTile.features.Remove (this);
		}
		CurrentTile = null;
	}	
}
