using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour, IInputReceiver{

	public int x, y;

	public List<TileFeature> features = new List<TileFeature>();

	public Tile GetTileAtOffset(int xOffset, int yOffset)
	{
		return FloorLayout.GetTile(x + xOffset, y + yOffset);
	}

	public void OnInputDown()
	{
		if(Machine.MachineOnCursor != null){
			Machine.MachineOnCursor.Drop(this);
		}
	}
}
