using UnityEngine;
using System.Collections;

public class Suspicion {

	public GridSquare square;
	public float suspicion;
	
	public Suspicion(GridSquare square, float suspicion){
		this.square = square;
		this.suspicion = suspicion;
	}
}