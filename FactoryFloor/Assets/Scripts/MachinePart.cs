using UnityEngine;
using System.Collections;

public class MachinePart {
	public Vector2i Offset;
	public Machine Machine;

	private MachinePartDisplay display;

	public bool CollisionEnabled{
		set
		{
			if(display != null && display.collider)
			{
				display.collider.enabled = value;
			}
		}
	}

	public void MoveToTile(Tile tile)
	{
		InitDisplay();
		display.MoveToTile(tile);
	}

	public void DisplayAtWithOffset(Vector2 position)
	{
		InitDisplay();
		display.transform.position = position + 
			Vector2.Scale (new Vector2(Offset.x, Offset.y), FloorLayout.TileSize);
	}

	public void AddInPort()
	{

	}

	public void AddOutPort()
	{
	}

	private void InitDisplay()
	{
		if(display == null)
		{
			display = GameObject.Instantiate(PrefabManager.MachinePartDisplay) as MachinePartDisplay;
			display.Part = this;
			display.InitSprite();
		}
	}
}
