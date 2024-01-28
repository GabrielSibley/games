using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {
	float startTime;

	void Start()
	{
		startTime = Time.time;
	}

	// Update is called once per frame
	void Update () {
		if(Winnput.HomeDown)
		{
			Application.Quit();
		}
		else if(Winnput.AnyButtonDown && Time.time > startTime + 1)
		{
			Vase.All.Clear ();
			Application.LoadLevel ("game");
		}
	}
}
