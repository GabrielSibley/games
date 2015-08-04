using UnityEngine;
using System.Collections;

public class PackRuleDisplay : MonoBehaviour {

	public void Display(Machine machine, Vector2 machinePosition)
	{
		transform.position = new Vector3(machinePosition.x, machinePosition.y, -15);
	}
}
