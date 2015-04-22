using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

public class FloorLayout : MonoBehaviour {

	public GameObject floorTilePrefab;

	public Tile[] tiles;

	public int width = 8;
	public int height = 8;
	public Vector2 tileSize = new Vector2(72, 72);

	private static FloorLayout instance;

	public static Tile GetTile(int x, int y){
		if(x < 0 || x >= instance.width || y < 0 || y >= instance.height){
			return null;
		}
		return instance.tiles[y*instance.width + x];
	}

	public static Tile[] GetTiles(){
		return instance.tiles;
	}

	public static int Width { get { return instance.width; }}
	public static int Height { get { return instance.height; }}

#if UNITY_EDITOR
	[ContextMenu("Layout Floor")]
	public void Layout() {
		tiles = new Tile[width * height];
		for(int x = 0; x < width; x++){
			for(int y = 0; y < height; y++){
				GameObject obj = PrefabUtility.InstantiatePrefab(floorTilePrefab) as GameObject;
				obj.transform.parent = transform;
				obj.transform.localPosition = new Vector3((x + 0.5f) * tileSize.x, (y + 0.5f) * tileSize.y, 0);
				var tile = obj.GetComponent<Tile>();
				tiles[y*width + x] = tile;
				tile.x = x;
				tile.y = y;
			}
		}
	}
#endif

	private void Awake(){
		instance = this;
	}
}
