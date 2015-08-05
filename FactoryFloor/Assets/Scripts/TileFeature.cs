using UnityEngine;
using System.Collections;

public abstract class TileFeature {

	public Tile CurrentTile;

	public abstract void MoveToTile(Tile t);
	public abstract void RemoveFromTile();
}
