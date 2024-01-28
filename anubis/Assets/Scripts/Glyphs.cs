using UnityEngine;
using System.Collections;

public class Glyphs : MonoBehaviour {

	public static Sprite GetGlyph(int index)
	{
		if(index < 0 || index >= instance.glyphs.Length)
		{
			return null;
		}
		return instance.glyphs[index];
	}
	private static Glyphs instance;

	public Sprite[] glyphs;

	private void Awake()
	{
		instance = this;
	}
}
