using UnityEngine;
using System.Collections;

public class RandomChallenge : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log ("Make " + RandomFeature() + " + " + RandomFeature());
	}

	string[] colors = {"green ", "yellow ", "red ", "blue "};
	string[] shapes = {"square", "circle", "triangle", "cross"};

	string RandomFeature()
	{
		return colors[Random.Range (0, colors.Length)] + shapes[Random.Range (0, shapes.Length)];
	}
}
