using UnityEngine;
using System.Collections;

public class MachineDock : Dock {
	
	public Machine Machine { get; set; }

    public Vector2 Offset; //Tile-space offset from machine origin

    public MachineDock(object source, DockType type) : base(source, type){
		//just call superconstructor
	}

	public override Vector2 FloorPosition{
		get {
            if(Machine.OnFloor)
            {
                return Machine.Origin.Value + Offset;
            }
			return Vector2.zero;
		}
	}
}
