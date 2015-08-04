using UnityEngine;
using System.Collections;

public class PipeDisplay : MonoBehaviour {

	public const float PipeSize = 12;
	
	public void Display(Pipe pipe)
	{
		Vector2 from = pipe.From.WorldPosition;
		Vector2 to = pipe.To.WorldPosition;
		Vector2 midpoint = (from + to) / 2;
		transform.position = new Vector3(midpoint.x, midpoint.y, -10);
		transform.rotation = Quaternion.Euler (0, 0, Mathf.Rad2Deg * Mathf.Atan2 ((to - midpoint).y, (to - midpoint).x));
		float repeats = Vector2.Distance(from, to) / PipeSize;
		transform.localScale = new Vector3(repeats * PipeSize, PipeSize, 1);
		GetComponent<Renderer>().material.mainTextureScale = new Vector2(repeats, 1);

		if(pipe.Paused)
		{
			GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);
		}
		else
		{
			GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
		}
	}

	private void Update()
	{
		Vector2 offset = GetComponent<Renderer>().material.mainTextureOffset;
		offset.x -= Time.deltaTime;
		GetComponent<Renderer>().material.mainTextureOffset = offset;
	}
}
