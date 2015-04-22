using UnityEngine;
using System.Collections;

public class CrateManager : MonoBehaviour {

	private static CrateManager instance;

	public Crate cratePrefab;


	public static void CreateCrate(Conveyor target){
		if(target.NextGiven == null){
			Crate crate = Instantiate(instance.cratePrefab) as Crate;
			crate.UpdatePosition(target);
			target.TakeCrate(crate);
		}
	}

	private void Awake(){
		instance = this;
	}
}
