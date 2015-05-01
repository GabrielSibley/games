using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MachineTestSpawn : MonoBehaviour {

	public Tile targetTile;

	public static Machine GetRandomMachine(){
		Machine machine = new Machine();
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
		//1 input, 1 output
		var inPart = machine.Parts[Random.Range (0, machine.Parts.Count)];
		var outPart = machine.Parts[Random.Range (0, machine.Parts.Count)];
		inPart.AddInPort();
		outPart.AddOutPort();
		return machine;
	}
}