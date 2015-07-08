using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MachineTestSpawn {
	
	public static Machine GetRandomMachine(){
		return GetRandomMachineWithRule(GetNewRandomRule());
	}

	public static Machine GetRandomMachineWithRule(IMachineRule rule)
	{
		Machine machine = new Machine();
		machine.Rule = rule;
		//1-6 parts
		int size = Random.Range (1, 4) + Random.Range (0, 4);
		List<Vector2i> openLocations = new List<Vector2i>();
		openLocations.Add(new Vector2i(){x = 0, y = 0});
		for(int i = 0; i < size; i++)
		{
			int locIndex = Random.Range (0, openLocations.Count);
			Vector2i loc = openLocations[locIndex];
			openLocations.RemoveAt(locIndex);
			if(machine.HasPartAt(loc))
			{
				i--;
			}
			else{
				MachinePart part = new MachinePart();
				part.Offset = loc;
				machine.AddPart(part);
				var dirs = new Vector2i[]{Vector2i.Up, Vector2i.Down, Vector2i.Left, Vector2i.Right};
				for(int j = 0; j < dirs.Length; j++){
					if(!machine.HasPartAt(loc + dirs[j]))
					{
						openLocations.Add (loc + dirs[j]);
					}
				}
			}
		}
		//TODO: Add support for > 1 port of each type
		if(machine.Rule.NumInPorts == 1)
		{
			var inPart = machine.Parts[Random.Range (0, machine.Parts.Count)];
			inPart.AddInPort();
		}
		if(machine.Rule.NumOutPorts == 1)
		{
			var outPart = machine.Parts[Random.Range (0, machine.Parts.Count)];
			outPart.AddOutPort();
		}
		if(machine.Rule.NumInPorts > 1 || machine.Rule.NumOutPorts > 1)
		{
			Debug.LogError ("No support for machines with more than 1 port of each type yet");
		}
		return machine;
	}

	private static bool produceNext = true;
	private static bool destroyNext;
	private static IMachineRule GetNewRandomRule()
	{
		if(destroyNext)
		{
			destroyNext = false;
			return new DestroyRule();
		}
		float i = Random.value;
		if(i < 0.2f || produceNext)
		{
			produceNext = false;
			destroyNext = true;
			return new ProduceRandomRule(); //Spawn random crates
		}
		return MatchReplaceRule.GetRandom();
	}
}