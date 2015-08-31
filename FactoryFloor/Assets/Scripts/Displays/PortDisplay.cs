using UnityEngine;
using System.Collections;

[PrefabManager]
public class PortDisplay : MonoBehaviour {

	private const float displayDepth = -25f;

	public Sprite InPortSprite;
	public Sprite InPortFirstSprite;
	public Sprite InPortLastSprite;
	public Sprite OutPortSprite;
	public Sprite OutPortFirstSprite;
	public Sprite OutPortLastSprite;

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
			if(Port.Effect == PortEffect.First){
				SpriteRenderer.sprite = InPortFirstSprite;
			}
			else if(Port.Effect == PortEffect.Last){
				SpriteRenderer.sprite = InPortLastSprite;
			}
			else{
				SpriteRenderer.sprite = InPortSprite;
			}
		}
		else
		{
			if(Port.Effect == PortEffect.First){
				SpriteRenderer.sprite = OutPortFirstSprite;
			}
			else if(Port.Effect == PortEffect.Last){
				SpriteRenderer.sprite = OutPortLastSprite;
			}
			else{
				SpriteRenderer.sprite = OutPortSprite;
			}
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
