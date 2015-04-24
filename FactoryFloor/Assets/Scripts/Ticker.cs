using UnityEngine;
using System.Collections;
//Ticks things that cannot tick themselves
public class Ticker : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if(Machine.MachineOnCursor != null){
			Machine.MachineOnCursor.DisplayAt(InputManager.InputWorldPos);
		}
	}
}
