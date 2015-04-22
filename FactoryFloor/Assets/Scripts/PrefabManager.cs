using UnityEngine;
using System.Collections;

public class PrefabManager : MonoBehaviour {

	private static PrefabManager instance;

	public static Conveyor Conveyor{
		get{
			return instance.conveyorPrefab;
		}
	}

	[SerializeField] private Conveyor conveyorPrefab;

	void Awake(){
		instance = this;
	}
}
