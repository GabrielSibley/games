using UnityEngine;
using System.Collections;

public class SpawnMachineButton : MonoBehaviour, IInputReceiver {

	public void OnInputDown(){
		Machine mach = MachineTestSpawn.GetRandomMachine();
		mach.PickUp();
	}
}
