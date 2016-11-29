using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Updates UI to match game state
public class SimulationView : MonoBehaviour {

    public static SimulationView Instance;

    public enum UIPanel { Machines, Contracts}
    public UIPanel ActivePanel;

    public Machine CarriedMachine {
        get { return carriedMachine; }
        set
        {
            carriedMachine = value;            

            if(carriedMachine != null && carriedMachine.OnFloor)
            {
                //TODO: remember crane links here
                carriedMachine.RemoveFromFloor();                
            }

            carriedMachineDisplay.Display(value);
        }
    }
    private Machine carriedMachine;

    //Currently displayed simulation
    public Simulation Simulation { get; private set;}

    public InputManager inputManager;
	public MachineShopView shopView;
    public CarriedMachineDisplay carriedMachineDisplay;
    public MachineDisplay machineFloorDisplayPrefab;
    public PipeEditManager pipeEditor;

    public GameObject contractsPanel;
    public ContractShopView supplyView;
    public ContractShopView deliverView;

    public DockDisplay[] contractSlotDocks;

    private List<MachineDisplay> machineFloorDisplayPool = new List<MachineDisplay>();
    private List<ContractDisplay> contractDisplays = new List<ContractDisplay>();

    private void Awake()
    {
        Instance = this;
    }

	public void Display(Simulation sim)
	{
        Simulation = sim;
        pipeEditor.Simulation = sim; //TODO: axe
        inputManager.ProcessInput();
        if (ActivePanel == UIPanel.Machines)
        {
            shopView.gameObject.SetActive(true);
            shopView.Display(sim.MachineShop);
        }
        else
        {
            shopView.gameObject.SetActive(false);
        }

        if (ActivePanel == UIPanel.Contracts)
        {
            contractsPanel.SetActive(true);
            supplyView.Display(sim.SupplyShop);
            deliverView.Display(sim.DeliveryShop);
        }
        else
        {
            contractsPanel.SetActive(false);
        }

        carriedMachineDisplay.Display(CarriedMachine);

        DisplayFloorMachines(sim);
        DisplayContractSlots();

        Simulation = null;        
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
            displayPosition.z = ZLayer.Machine;
            display.transform.position = displayPosition;

            poolIndex++;
        }
        for(; poolIndex < machineFloorDisplayPool.Count; poolIndex++)
        {
            machineFloorDisplayPool[poolIndex].Display(null);
        }
    }

    private void DisplayContractSlots()
    {
        foreach(ContractDisplay display in contractDisplays)
        {
            Destroy(display.gameObject); //TODO: Pool these
        }
        contractDisplays.Clear();
        for(int i = 0; i < Simulation.ContractSlots.Count; i++)
        {
            var slot = Simulation.ContractSlots[i];
            if (slot.Contract != null)
            {
                ContractDisplay display = Instantiate(PrefabManager.ContractDisplay) as ContractDisplay;
                display.Display(slot.Contract);
                Vector3 displayPosition = FloorView.FloorToWorldPoint(slot.FloorPosition, FloorViewSpace.TileCorner);
                displayPosition.z = ZLayer.Contract;
                display.transform.position = displayPosition;
                contractDisplays.Add(display);

                var dockDisplay = contractSlotDocks[i];
                dockDisplay.gameObject.SetActive(true);
                dockDisplay.Dock = slot.Dock;
                dockDisplay.Display();
            }
            else
            {
                var dockDisplay = contractSlotDocks[i];
                dockDisplay.gameObject.SetActive(false);
                dockDisplay.Dock = null;                
            }
        }
    }
}
