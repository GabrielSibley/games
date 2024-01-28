using UnityEngine;
using System.Collections;

public class Magazine : Item {
	public int capacity;
	public int count;
	public AmmoType ammoType;
	
	public override PotentialActions ItemActions(Item item){
		PotentialActions result = base.ItemActions(item);
		Firearm asGun = item as Firearm;
		if(asGun != null && asGun.magazine == null && asGun.ammoType == ammoType)
			result.Add("Load", typeof(LoadWeapon), 5);
		return result;
	}
}



public enum AmmoType{
	Pistol,
	Smg,
	Rifle,
	Shotgun
}
