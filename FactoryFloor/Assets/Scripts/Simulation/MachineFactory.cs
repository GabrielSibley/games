using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MachineFactory {

	private Simulation Simulation;

	public MachineFactory(Simulation sim)
	{
		Simulation = sim;
	}

	public Machine GetMachineForShop()
	{
		Machine machine = new Machine();
		machine.Simulation = Simulation;
		machine.Rule = GetRandomMachineRuleForShop();
		machine.GeneratePartLayout();
		return machine;
	}

	private IMachineRule GetRandomMachineRuleForShop()
	{
		float rand = Random.value;
		if(rand < 0.7f)
		{
			//Match/replace
			return MatchReplaceRule.GetRandom();
		}
		else if(rand < 0.8f)
		{
			//Pack
			return new PackRule();
		}
		else if(rand < 0.9f)
		{
			//Unpack
			return new UnpackRule();
		}
		else
		{
			//DestroyAny
			return new DestroyRule();
		}
	}
}
