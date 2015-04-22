using UnityEngine;
using System.Collections;

public class ConveyorCreatorOverlay : MonoBehaviour {

	public SpriteRenderer north, east, south, west;
	public Sprite move, delete;

	public void Show(Tile targetTile)
	{
		gameObject.SetActive (true);
		Vector3 newPos = targetTile.transform.position;
		newPos.z = transform.position.z;
		transform.position = newPos;
	}

	public void Hide()
	{
		gameObject.SetActive (false);
	}
}
