using UnityEngine;
using System.Collections;

public class PortDisplay : MonoBehaviour {

	public Sprite InPortSprite;
	public Sprite OutPortSprite;
	public SpriteRenderer SpriteRenderer;

	public Port Port;

	public void UpdateDisplay()
	{
		if(Port == null)
		{
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
	}
}
