using UnityEngine;
using System.Collections;

public interface ICrateGiver {

	string name { get; }
	ICrateTaker GivesTo { get; }
	Crate NextGiven { get; }
}
