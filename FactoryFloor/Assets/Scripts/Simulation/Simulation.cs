using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Simulation {

	public const int floorWidth = 8;
	public const int floorHeight = 8;

	public Floor Floor {get; private set;}

    private HashSet<IUpdatable> updatables = new HashSet<IUpdatable>();

	const int machineShopSizeLimit = 24;
	MachineFactory machineFactory;
	public List<Machine> MachineShop = new List<Machine>(machineShopSizeLimit);
    const int contractShopSizeLimit = 9;
    const int contractSlotLimit = 8;
    ContractFactory supplyContractFactory, deliveryContractFactory;
    public List<Contract> SupplyShop = new List<Contract>(contractShopSizeLimit);
    public List<Contract> DeliveryShop = new List<Contract>(contractShopSizeLimit);
    public List<ContractSlot> ContractSlots = new List<ContractSlot>(contractSlotLimit);

    public Simulation()
	{
		Floor = new Floor(this, floorWidth, floorHeight);
		machineFactory = new MachineFactory(this);
        supplyContractFactory = new ContractFactory(false);
        deliveryContractFactory = new ContractFactory(true);

		//Populate machine shop
		for(int i = 0; i < machineShopSizeLimit; i++)
		{
			MachineShop.Add (machineFactory.GetMachineForShop());
		}

        //Populate contracts
        for(int i = 0; i < contractShopSizeLimit; i++)
        {
            SupplyShop.Add(supplyContractFactory.GetContractForShop());
            DeliveryShop.Add(deliveryContractFactory.GetContractForShop());
        }

        //Create contract slots
        Vector2[] slotPositions = new Vector2[]
        {
            new Vector2(1, -0.666f),
            new Vector2(3, -0.666f),
            new Vector2(5, -0.666f),
            new Vector2(7, -0.666f),
            new Vector2(1, 8.666f),
            new Vector2(3, 8.666f),
            new Vector2(5, 8.666f),
            new Vector2(7, 8.666f)
        };
        for (int i = 0; i < contractSlotLimit; i++)
        {
            ContractSlot slot = new ContractSlot();
            slot.FloorPosition = slotPositions[i];
            ContractSlots.Add(slot);
        }
	}

    public void Tick(IUpdatable obj)
    {
        updatables.Add(obj);
    }

    public void Untick(IUpdatable obj)
    {
        updatables.Remove(obj);
    }

    public void Update(float deltaTime)
	{
		foreach(IUpdatable updatable in updatables)
		{
            updatable.Update(deltaTime);
		}
	}

    public void OnMachineMoved(Machine machine)
    {
        if(machine.OnFloor)
        {
            Floor.AddMachine(machine);
            
            //Check if machine was placed from machine shop
            int shopIndex = MachineShop.FindIndex(x => x == machine);
            if(shopIndex >= 0)
            {
                //Replace shop spot with new machine
                MachineShop[shopIndex] = machineFactory.GetMachineForShop();
            }
        }
        else
        {
            Floor.RemoveMachine(machine);
        }
    }
}
