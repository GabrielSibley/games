using UnityEngine;
using System.Collections;

[PrefabManager]
public class UnpackRuleDisplay : MonoBehaviour, IMachineRuleDisplay {

	public void Display(Machine machine, Vector2 machinePosition)
	{
		transform.position = new Vector3(machinePosition.x, machinePosition.y, -15);
	}
}
