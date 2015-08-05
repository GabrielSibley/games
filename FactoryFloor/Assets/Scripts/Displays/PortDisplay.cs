using UnityEngine;
using System.Collections;

[PrefabManager]
public class PortDisplay : MonoBehaviour {

	private const float displayDepth = -25f;

	public Sprite InPortSprite;
	public Sprite OutPortSprite;
	public SpriteRenderer SpriteRenderer;

	public Port Port; 

	public void Display(Vector2 pos)
	{
		if(Port == null)
		{
			Debug.LogError ("Trying to display null port", this);
			SpriteRenderer.sprite = null;
		}
		else if(Port.Type == PortType.In)
		{
			SpriteRenderer.sprite = InPortSprite;
		}
		else
		{
			SpriteRenderer.sprite = OutPortSprite;
		}
		transform.position = new Vector3(pos.x + Port.Offset.x * FloorLayout.TileSize.x,
		                                 pos.y + Port.Offset.y * FloorLayout.TileSize.y,
		                                 displayDepth);

		//HACK: Make it so pipes update display when machines are dragged
		//TODO: Fix it
		if(Port.Pipe != null)
		{
			Port.Pipe.UpdateDisplay();
		}
	}
}
