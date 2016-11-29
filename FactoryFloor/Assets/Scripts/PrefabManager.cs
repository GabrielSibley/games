// ----- AUTO GENERATED CODE ----- //
using UnityEngine;
public class PrefabManager : MonoBehaviour {
	private static PrefabManager instance;
	private void Awake(){ instance = this; }
	public static ContractDisplay ContractDisplay { get { return instance._ContractDisplay; } }
	[SerializeField] private ContractDisplay _ContractDisplay;
	public static DockDisplay DockDisplay { get { return instance._DockDisplay; } }
	[SerializeField] private DockDisplay _DockDisplay;
	public static FourFeatureDisplay FourFeatureDisplay { get { return instance._FourFeatureDisplay; } }
	[SerializeField] private FourFeatureDisplay _FourFeatureDisplay;
	public static GrabberDisplay GrabberDisplay { get { return instance._GrabberDisplay; } }
	[SerializeField] private GrabberDisplay _GrabberDisplay;
	public static MachineDisplay MachineDisplay { get { return instance._MachineDisplay; } }
	[SerializeField] private MachineDisplay _MachineDisplay;
	public static MachinePartDisplay MachinePartDisplay { get { return instance._MachinePartDisplay; } }
	[SerializeField] private MachinePartDisplay _MachinePartDisplay;
	public static PipeDisplay PipeDisplay { get { return instance._PipeDisplay; } }
	[SerializeField] private PipeDisplay _PipeDisplay;
	public static DestroyExactlyRuleDisplay DestroyExactlyRuleDisplay { get { return instance._DestroyExactlyRuleDisplay; } }
	[SerializeField] private DestroyExactlyRuleDisplay _DestroyExactlyRuleDisplay;
	public static DestroyRuleDisplay DestroyRuleDisplay { get { return instance._DestroyRuleDisplay; } }
	[SerializeField] private DestroyRuleDisplay _DestroyRuleDisplay;
	public static MatchReplaceRuleDisplay MatchReplaceRuleDisplay { get { return instance._MatchReplaceRuleDisplay; } }
	[SerializeField] private MatchReplaceRuleDisplay _MatchReplaceRuleDisplay;
	public static PackRuleDisplay PackRuleDisplay { get { return instance._PackRuleDisplay; } }
	[SerializeField] private PackRuleDisplay _PackRuleDisplay;
	public static ProduceRuleDisplay ProduceRuleDisplay { get { return instance._ProduceRuleDisplay; } }
	[SerializeField] private ProduceRuleDisplay _ProduceRuleDisplay;
	public static UnpackRuleDisplay UnpackRuleDisplay { get { return instance._UnpackRuleDisplay; } }
	[SerializeField] private UnpackRuleDisplay _UnpackRuleDisplay;
}
