using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile {

	public Vector2i Position;

	public List<TileFeature> features = new List<TileFeature>();

	public Tile GetTileAtOffset(Vector2i offset)
	{
		return Floor.GetTile(Position + offset);
	}
}
