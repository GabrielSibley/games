using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {

	public float speed = 20;
	public VehicleColors colors;

	// Use this for initialization
	void Start () {
		renderer.material.color = colors.GetRandom ();
	}
	
	void FixedUpdate(){
		transform.position += transform.forward * speed * Time.deltaTime;
	}
}
