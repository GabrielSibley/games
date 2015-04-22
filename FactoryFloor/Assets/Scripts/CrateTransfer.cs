using UnityEngine;
using System.Collections;

public class CrateTransfer {

	public Crate Crate;
	public ICrateHolder From, To;
	
	public bool Executed;

	public void Execute(){
		if(Crate == null){
			Debug.LogError ("Cannot transfer null crate");
			return;
		}
		if(From == null){
			Debug.LogError("Cannot transfer from null holder");
			return;
		}
		if(To == null){
			Debug.LogError ("Cannot transfer to null holder");
			return;
		}
		if(!Executed){
			From.RemoveCrate(Crate);
			To.AddCrate(Crate);
			Executed = true;
		}
		else{
			Debug.LogWarning ("Crate transfer already executed");
		}
	}
}
