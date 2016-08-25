using UnityEngine;
using System.Collections;

public class MachineShopButton : MonoBehaviour, IInputReceiver {

	public Transform RuleDisplayAnchor;
	public MachineMiniLayoutDisplay MiniLayout;
    public SpriteRenderer Background;
    public Sprite BackgroundUp, BackgroundDown;

	MachineRuleDisplay ruleDisplay;
    Machine currentMachine;

	public void Display(Machine machine)
	{
        currentMachine = machine;
		if(machine == null)
		{
			gameObject.SetActive(false);
		}
		else
		{
			gameObject.SetActive (true);
			//Get correct display for rule
			MachineRuleDisplay properDisplayPrefab = MachineRuleDisplayUtil.GetDisplayPrefabForRule(machine.Rule);
			//If rule has no display, nuke current
			if(properDisplayPrefab == null && ruleDisplay != null)
			{
				DestroyCurrentDisplay();
			}
			//Else nuke current and instantiate correct
			else if(properDisplayPrefab != null && (ruleDisplay == null || ruleDisplay.GetType() != properDisplayPrefab.GetType()))
			{
				MachineRuleDisplay properDisplay = Instantiate(properDisplayPrefab, Vector3.zero, Quaternion.identity) as MachineRuleDisplay;
				properDisplay.transform.SetParent(RuleDisplayAnchor, false);
				if(ruleDisplay != null)
				{
					DestroyCurrentDisplay();
				}
				ruleDisplay = properDisplay;
			}
			if(ruleDisplay != null)
			{
				ruleDisplay.Display(machine);
			}

			MiniLayout.Display (machine);

            Background.sprite = machine == SimulationView.Instance.CarriedMachine ? BackgroundDown : BackgroundUp;
		}
	}

    public void OnInputDown()
    {
        //Drop if already carrying
        if (SimulationView.Instance.CarriedMachine == currentMachine)
        {
            SimulationView.Instance.CarriedMachine = null;
        }
        //Pick up
        else
        {
            SimulationView.Instance.CarriedMachine = currentMachine;
        }
    }

	void DestroyCurrentDisplay()
	{
		Destroy((ruleDisplay as Component).gameObject);
	}
}
