using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface ICrateTaker {

	string name { get; }

	List<ICrateGiver> TakesFrom {get;}
	//Crate NextTaken {get; set;}

	bool TryTakeCrateNext(Crate crate);
	void ApplyCrateChanges();
}
