using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Crate : MonoBehaviour {

	private static int nextCrateId;
	public static List<Crate> allCrates = new List<Crate>();

	private void Awake(){
		allCrates.Add(this);
		name = "Crate " + nextCrateId++;
	}

	private void OnDestroy(){
		allCrates.Remove(this);
	}
}
