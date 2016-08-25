using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Updates UI to match game state
public class SimulationView : MonoBehaviour {

    public static SimulationView Instance;

    public Machine CarriedMachine {
        get { return carriedMachine; }
        set
        {
            carriedMachine = value;
            carriedMachineDisplay.Display(value);
        }
    }
    private Machine carriedMachine;

    public InputManager inputManager;
	public MachineShopView shopView;
    public CarriedMachineDisplay carriedMachineDisplay;
    public MachineDisplay machineFloorDisplayPrefab;

    private List<MachineDisplay> machineFloorDisplayPool = new List<MachineDisplay>();

	public void Display(Simulation sim)
	{
        Instance = this;
        inputManager.ProcessInput();
		shopView.Display(sim.MachineShop);
        carriedMachineDisplay.Display(CarriedMachine);

        DisplayFloorMachines(sim);        

        Instance = null;
	}

    //Updates display of machines on the sim floor
    private void DisplayFloorMachines(Simulation sim)
    {
        int poolIndex = 0;
        foreach (Machine machine in sim.Floor.Machines)
        {
            if(poolIndex >= machineFloorDisplayPool.Count)
            {
                MachineDisplay newDisplay = Instantiate(machineFloorDisplayPrefab);
                machineFloorDisplayPool.Add(newDisplay);
            }
            MachineDisplay display = machineFloorDisplayPool[poolIndex];
            display.Display(machine);
            Vector3 displayPosition = FloorView.FloorToWorldPoint(machine.Origin.Value, FloorViewSpace.TileCenter);
            displayPosition.z = ZLayer.FloorMachine;
            display.transform.position = displayPosition;

            poolIndex++;
        }
        for(; poolIndex < machineFloorDisplayPool.Count; poolIndex++)
        {
            machineFloorDisplayPool[poolIndex].Display(null);
        }
    }
}
