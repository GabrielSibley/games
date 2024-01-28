using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vase : MonoBehaviour {

	public static List<Vase> All = new List<Vase>();

	public int glyph;

	private void Awake()
	{
		All.Add (this);
	}
}
