using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour, IInputReceiver{

	public Vector2i Position;

	public List<TileFeature> features = new List<TileFeature>();

	public Tile GetTileAtOffset(Vector2i offset)
	{
		return FloorLayout.GetTile(Position + offset);
	}

	public void OnInputDown()
	{
		if(Machine.MachineOnCursor != null){
			Machine.MachineOnCursor.Drop(this);
		}
	}
}
