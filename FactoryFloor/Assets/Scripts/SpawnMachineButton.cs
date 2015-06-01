using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpawnMachineButton : MonoBehaviour, IInputReceiver {
	
	public void OnInputDown(){
		if(!GameMode.ModeLocked)
		{
			Machine mach = MachineTestSpawn.GetRandomMachine();
			mach.PickUp();
		}
	}
}
