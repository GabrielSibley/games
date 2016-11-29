using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ContractShopButton : MonoBehaviour, IInputReceiver {

    ContractDisplay display;
    Contract currentContract;

	public void Display(Contract contract)
	{
        currentContract = contract;
		if(contract == null)
		{
			gameObject.SetActive(false);
		}
		else
		{
			gameObject.SetActive (true);
            if(display == null)
            {
                display = Instantiate(PrefabManager.ContractDisplay, Vector3.zero, Quaternion.identity) as ContractDisplay;
                display.transform.SetParent(this.transform, false);
            }
            display.Display(contract);            			
		}
	}

    public void OnInputDown()
    {
        if(currentContract == null)
        {
            return;
        }
        List<ContractSlot> slots = SimulationView.Instance.Simulation.ContractSlots;
        for (int i = 0; i < slots.Count; i++)
        {
            if(slots[i].Contract == null)
            {
                slots[i].Contract = currentContract;
                //TODO: Remove contract from shop
                Debug.Log("Assigned contract to slot " + i);
                break;
            }
        }
        
    }

	void DestroyCurrentDisplay()
	{
		Destroy(display.gameObject);
	}
}
