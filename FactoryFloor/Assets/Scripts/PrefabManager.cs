using UnityEngine;
using System.Collections;

public class PrefabManager : MonoBehaviour {

	private static PrefabManager instance;

	public static MachinePartDisplay MachinePartDisplay{
		get{
			return instance.machinePartDisplayPrefab;
		}
	}

	public static MatchReplaceRuleDisplay MatchReplaceRuleDisplay
	{
		get
		{
			return instance.matchReplaceRuleDisplay;
		}
	}

	public static PackRuleDisplay PackRuleDisplay
	{
		get
		{
			return instance.packRuleDisplay;
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

	public static FourFeatureDisplay FourFeatureDisplay
	{
		get
		{
			return instance.fourFeatureDisplay;
		}
	}
	
	[SerializeField] private MachinePartDisplay machinePartDisplayPrefab;
	[SerializeField] private PipeDisplay pipeDisplay;
	[SerializeField] private GrabberDisplay grabberDisplay;
	[SerializeField] private MatchReplaceRuleDisplay matchReplaceRuleDisplay;
	[SerializeField] private PackRuleDisplay packRuleDisplay;
	[SerializeField] private FourFeatureDisplay fourFeatureDisplay;

	void Awake(){
		instance = this;
	}
}
