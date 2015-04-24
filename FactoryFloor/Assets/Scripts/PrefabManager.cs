using UnityEngine;
using System.Collections;

public class PrefabManager : MonoBehaviour {

	private static PrefabManager instance;

	public static Crate Crate{
		get{
			return instance.cratePrefab;
		}
	}

	public static MachinePartDisplay MachinePartDisplay{
		get{
			return instance.machinePartDisplayPrefab;
		}
	}

	[SerializeField] private Crate cratePrefab;
	[SerializeField] private MachinePartDisplay machinePartDisplayPrefab;

	void Awake(){
		instance = this;
	}
}
