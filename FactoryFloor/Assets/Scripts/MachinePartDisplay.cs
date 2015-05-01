using UnityEngine;
using System.Collections;

public class MachinePartDisplay : TileFeature {

	public MachinePart Part;
	public Machine Machine { get { return Part.Machine; } }

	public SpriteRenderer RendererUL;
	public SpriteRenderer RendererUR;
	public SpriteRenderer RendererLL;
	public SpriteRenderer RendererLR;
	public Sprite[] Sprites;

	public override void MoveToTile(Tile tile){
		if(CurrentTile != null){
			CurrentTile.features.Remove (this);
		}
		if(tile != null)
		{
			tile.features.Add (this);
			transform.position = tile.transform.position - Vector3.forward;
		}
		CurrentTile = tile;
	}

	public override void Remove()
	{
		if(CurrentTile != null)
		{
			CurrentTile.features.Remove (this);
		}
		CurrentTile = null;
	}

	public override void OnInputDown()
	{
		Machine.PickUp();
	}

	public void InitSprite()
	{
		bool[] joins = new bool[8];
		Vector2i[] offsets = {Vector2i.Up, Vector2i.Up+Vector2i.Left, Vector2i.Left,
			Vector2i.Left+Vector2i.Down, Vector2i.Down, Vector2i.Down+Vector2i.Right,
			Vector2i.Right, Vector2i.Right + Vector2i.Up};
		for(int i = 0; i < joins.Length; i++)
		{
			joins[i] = Machine.HasPartAt (Part.Offset + offsets[i]);
		}
		SetSubsprite(RendererUL, joins[0], joins[2], true, true, 3, joins[1]);
		SetSubsprite(RendererUR, joins[0], true, true, joins[6], 2, joins[7]);
		SetSubsprite(RendererLL, true, joins[2], joins[4], true, 1, joins[3]);
		SetSubsprite(RendererLR, true, true, joins[4], joins[6], 0, joins[5]);
	}

	private void SetSubsprite(SpriteRenderer subsprite, 
	                          bool connectNorth, 
	                          bool connectWest, 
	                          bool connectSouth,
	                          bool connectEast,
	                          int diagonalType,
	                          bool joinDiagonal)
	{
		int row, col;
		//Inner elbows
		if(connectNorth && connectWest && connectSouth && connectEast)
		{
			if(joinDiagonal)
			{
				row = col = 1;
			}
			else{
				row = diagonalType / 2;
				col = diagonalType % 2 + 3;
			}
		}
		//Everything else
		else
		{
			if(connectNorth)
			{
				row = connectSouth ? 1 : 2;
			}
			else
			{
				row = 0;
			}
			
			if(connectEast)
			{
				col = connectWest ? 1 : 0;
			}
			else
			{
				col = 2;
			}
		}
		subsprite.sprite = Sprites[row * 5 + col];
	}
}
