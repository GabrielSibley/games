using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;


public enum FloorViewSpace
{
    TileCorner,
    TileCenter
}

//Class for handling floor graphics
public class FloorView : MonoBehaviour {
    public GameObject[] floorTiles;
	public GameObject floorTilePrefab;

	public Vector2 tileSize = new Vector2(72, 72);

	public static Vector2 TileSize{ get { return instance.tileSize; } }

    public static Vector2 FloorToWorldVector (Vector2 offset)
    {
        return Vector2.Scale(offset, TileSize);
    }

    public static Vector3 FloorToWorldPoint(Vector2 floorPos, FloorViewSpace space = FloorViewSpace.TileCorner)
    {
        if(space == FloorViewSpace.TileCenter)
        {
            floorPos += new Vector2(0.5f, 0.5f);
        }
        return instance.transform.TransformPoint(Vector2.Scale(floorPos, TileSize));
    }

    public static Vector2 WorldToFloorPoint(Vector3 worldPos)
    {
        Vector3 localPos = instance.transform.InverseTransformPoint(worldPos);
        Vector2 floorPos = new Vector2(localPos.x / TileSize.x, localPos.y / TileSize.y);
        return floorPos;
    }

	private static FloorView instance;

#if UNITY_EDITOR
	[ContextMenu("Layout Floor")]
	public void Layout() {
		if(floorTiles != null){
			for(int i = 0; i < floorTiles.Length; i++)
			{
				if(floorTiles[i])
				{
					DestroyImmediate(floorTiles[i]);
				}
			}
		}
		int width = Simulation.floorWidth;
		int height = Simulation.floorHeight;
		floorTiles = new GameObject[width * height];
		for(int x = 0; x < width; x++){
			for(int y = 0; y < height; y++){
				GameObject obj = PrefabUtility.InstantiatePrefab(floorTilePrefab) as GameObject;
				obj.transform.parent = transform;
				obj.transform.localPosition = new Vector3((x + 0.5f) * tileSize.x, (y + 0.5f) * tileSize.y, 0);
				floorTiles[y*width + x] = obj;
			}
		}
	}
#endif

	private void Awake(){
		instance = this;
	}
}
