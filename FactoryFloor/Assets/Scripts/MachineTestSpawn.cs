using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MachineTestSpawn {
	
	public static Machine GetRandomMachine(){
		return GetRandomMachineWithRule(GetNewRandomRule());
	}

	//Doesn't need to add up to 1
	private static float[] sizeWeights = new float[]{
		1, //size 1
		4, //size 2
		8, //size 3
		10, //size 4
		4, //size 5
		1  //size 6
	};

	public static Machine GetRandomMachineWithRule(IMachineRule rule)
	{
		Machine machine = new Machine();
		machine.Rule = rule;
		//1-6 parts
		int size = RandomIndexWeighted(sizeWeights) + 1;
		List<Vector2i> openLocations = new List<Vector2i>();
		openLocations.Add(new Vector2i(){x = 0, y = 0});
		int xMin = 0, xMax = 0, yMin = 0, yMax = 0;
		const int maxGridDimension = 3;
		for(int i = 0; i < size; i++)
		{
			if(openLocations.Count == 0)
			{
				Debug.LogError ("Machine tried to spawn too large!");
				break;
			}
			int locIndex = Random.Range (0, openLocations.Count);
			Vector2i loc = openLocations[locIndex];
			openLocations.RemoveAt(locIndex);
			int machWidth = xMax - xMin + 1;
			int machHeight = yMax - yMin + 1;
			if(machine.HasPartAt(loc)
				|| (machWidth >= maxGridDimension && (loc.x < xMin || loc.x > xMax))
				|| (machHeight >= maxGridDimension && (loc.y < yMin || loc.y > yMax))
			)
			{
				i--; //retry
			}
			else{
				MachinePart part = new MachinePart();
				part.Offset = loc;
				machine.AddPart(part);

				xMin = Mathf.Min(loc.x, xMin);
				xMax = Mathf.Max (loc.x, xMax);
				yMin = Mathf.Min (loc.y, yMin);
				yMax = Mathf.Max (loc.y, yMax);

				var dirs = new Vector2i[]{Vector2i.Up, Vector2i.Down, Vector2i.Left, Vector2i.Right};
				for(int j = 0; j < dirs.Length; j++){
					Vector2i newOpenLoc = loc + dirs[j];
					if(!machine.HasPartAt(newOpenLoc))
					{
						openLocations.Add (newOpenLoc);
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

	static int RandomIndexWeighted(float[] weights)
	{
		float roll = Random.value;
		float invWeightTotal = 1 / weights.Sum();
		float cumulativeWeight = 0;
		for(int i = 0; i < weights.Length; i++)
		{
			cumulativeWeight += weights[i] * invWeightTotal;
			if(roll <= cumulativeWeight)
			{
				return i;
			}
		}
		return weights.Length - 1;
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