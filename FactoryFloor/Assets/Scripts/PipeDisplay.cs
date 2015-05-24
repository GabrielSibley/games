﻿using UnityEngine;
using System.Collections;

public class PipeDisplay : MonoBehaviour {

	public const float PipeSize = 18;

	public void DisplayPointToPoint(Vector2 from, Vector2 to)
	{
		Vector2 midpoint = (from + to) / 2;
		transform.position = new Vector3(midpoint.x, midpoint.y, -10);
		transform.rotation = Quaternion.Euler (0, 0, Mathf.Rad2Deg * Mathf.Atan2 ((to - midpoint).y, (to - midpoint).x));
		float repeats = Vector2.Distance(from, to) / PipeSize;
		transform.localScale = new Vector3(repeats * PipeSize, PipeSize, 1);
		renderer.material.mainTextureScale = new Vector2(repeats, 1);
	}

	private void Update()
	{
		Vector2 offset = renderer.material.mainTextureOffset;
		offset.x -= Time.deltaTime;
		renderer.material.mainTextureOffset = offset;
	}
}
