using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

public class FloorView : MonoBehaviour {

	public TileDisplay[] tileViews;
	public GameObject floorTilePrefab;

	public Vector2 tileSize = new Vector2(72, 72);

	public static Vector2 TileSize{ get { return instance.tileSize; } }

	public static TileDisplay GetTile(int x, int y){
		if(x < 0 || x >= Floor.width || y < 0 || y >= Floor.height){
				return null;
			}
		return instance.tileViews[y*Floor.width + x];
	}
	public static TileDisplay GetTile(Vector2i pos)
	{
		return GetTile(pos.x, pos.y);
	}
	private static FloorView instance;

#if UNITY_EDITOR
	[ContextMenu("Layout Floor")]
	public void Layout() {
		for(int i = 0; i < tileViews.Length; i++)
		{
			if(tileViews[i])
			{
				DestroyImmediate(tileViews[i].gameObject);
			}
		}
		tileViews = new TileDisplay[Floor.width * Floor.height];
		for(int x = 0; x < Floor.width; x++){
			for(int y = 0; y < Floor.height; y++){
				GameObject obj = PrefabUtility.InstantiatePrefab(floorTilePrefab) as GameObject;
				obj.transform.parent = transform;
				obj.transform.localPosition = new Vector3((x + 0.5f) * tileSize.x, (y + 0.5f) * tileSize.y, 0);
				var tile = obj.GetComponent<TileDisplay>();
				tileViews[y*Floor.width + x] = tile;
			}
		}
	}
#endif

	private void Awake(){
		instance = this;
		LinkTilesWithViews();
	}

	private void LinkTilesWithViews()
	{
		for(int x = 0; x < Floor.width; x++){
			for(int y = 0; y < Floor.height; y++){
				GetTile(x, y).Tile = Floor.GetTile (x, y);
			}
		}
	}
}
