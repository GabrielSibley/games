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

	public Simulation()
	{
		Floor = new Floor(this, floorWidth, floorHeight);
		machineFactory = new MachineFactory(this);

		//Populate machine shop
		for(int i = 0; i < machineShopSizeLimit; i++)
		{
			MachineShop.Add (machineFactory.GetMachineForShop());
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
