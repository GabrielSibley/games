using UnityEngine;
using System.Collections;

public class RandomVehicleColor : MonoBehaviour {

	public VehicleColors colors;

	// Use this for initialization
	void Start () {
		renderer.material.color = colors.GetRandom();
	}
}
