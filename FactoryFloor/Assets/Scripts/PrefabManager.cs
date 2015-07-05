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

	public static PipeDisplay PipeDisplay
	{
		get{
			return instance.pipeDisplay;
		}
	}

	public static GrabberDisplay GrabberDisplay
	{
		get
		{
			return instance.grabberDisplay;
		}
	}

	[SerializeField] private Crate cratePrefab;
	[SerializeField] private MachinePartDisplay machinePartDisplayPrefab;
	[SerializeField] private PipeDisplay pipeDisplay;
	[SerializeField] private GrabberDisplay grabberDisplay;

	void Awake(){
		instance = this;
	}
}
