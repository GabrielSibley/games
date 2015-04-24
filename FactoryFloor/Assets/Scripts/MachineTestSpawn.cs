using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MachineTestSpawn : MonoBehaviour {

	public Tile targetTile;

	public static Machine GetRandomMachine(){
		Machine machine = new Machine();
		//1-6 parts
		int size = Random.Range (1, 4) + Random.Range (0, 4);
		List<Offset> openLocations = new List<Offset>();
		openLocations.Add(new Offset(){x = 0, y = 0});
		for(int i = 0; i < size; i++)
		{
			int locIndex = Random.Range (0, openLocations.Count);
			Offset loc = openLocations[locIndex];
			openLocations.RemoveAt(locIndex);
			if(machine.HasPartAt(loc))
			{
				i--;
			}
			else{
				MachinePart part = new MachinePart();
				part.XOffset = loc.x;
				part.YOffset = loc.y;
				machine.AddPart(part);
				var dirs = new Offset[]{Offset.Up, Offset.Down, Offset.Left, Offset.Right};
				for(int j = 0; j < dirs.Length; j++){
					if(!machine.HasPartAt(loc + dirs[j]))
					{
						openLocations.Add (loc + dirs[j]);
					}
				}
			}
		}
		return machine;
	}
}

public struct Offset{
	public static Offset Up {
		get{ return new Offset(0, 1); }
	}
	
	public static Offset Down {
		get{ return new Offset(0, -1); }
	}
	
	public static Offset Left {
		get{ return new Offset(-1, 0); }
	}
	
	public static Offset Right {
		get{ return new Offset(1, 0); }
	}
	
	public static Offset operator + (Offset a, Offset b){
		return new Offset(a.x + b.x, a.y + b.y);
	}
	
	public int x;
	public int y;
	
	public Offset(int x, int y){
		this.x = x;
		this.y = y;
	}
	
	public override bool Equals(object other)
	{
		if(other is Offset)
		{
			var otherOffset = (Offset)other;
			return otherOffset.x == x && otherOffset.y == y;
		}
		return false;
	}
	
	public override int GetHashCode(){
		return x * 101 + y;
	}
}
