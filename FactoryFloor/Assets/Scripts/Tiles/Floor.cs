using UnityEngine;
using System.Collections;

public class Floor {

	//TODO: Remove statics, replace with sim context
	public static Tile GetTile(Vector2i? pos)
	{
		if(pos.HasValue)
		{
			return GetTile(pos.Value.x, pos.Value.y);
		}
		else
		{
			return null;
		}
	}

	public static Tile GetTile(int x, int y){
		if(x < 0 || x >= width || y < 0 || y >= height){
			return null;
		}
		return instance.tiles[y*width + x];
	}

	public static Tile[] GetTiles(){
		return instance.tiles;
	}

	private static Floor instance = new Floor();

	private Tile[] tiles;

	public const int width = 8;
	public const int height = 8;

	public Floor()
	{
		tiles = new Tile[width * height];
		for(int x = 0; x < width; x++){
			for(int y = 0; y < height; y++){
				Tile tile = new Tile();
				tiles[y*width + x] = tile;
				tile.Position = new Vector2i(x, y);
			}
		}
	}
}
