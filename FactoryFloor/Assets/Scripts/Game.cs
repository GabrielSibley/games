using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

	public SimulationView simView;

	Simulation sim;

	// Use this for initialization
	void Start () {
		sim = new Simulation();
	}
	
	// Update is called once per frame
	void Update () {
		sim.Update (Time.deltaTime);
		simView.Display(sim);
	}
}
