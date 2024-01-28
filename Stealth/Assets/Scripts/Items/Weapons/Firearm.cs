using UnityEngine;
using System.Collections;

public class Firearm : Item {
	public int damage;
	public float unaimedDeviation;
	public float oneHandedDeviationModifier;
	public float maxAimPower;
	public float powerGain;
	public float maxPowerHalfRange;
	public float powerGainHalfRange;
	public float fireTime;
	public bool isSemiAuto;
	
	public float chamberTime;
	public float reloadTime;
	
	public float noise;
	
	public AmmoType ammoType;
	public Magazine magazine;
	
	public bool isChambered;
	
	public int numAttacks = 1; //>1 For shotguns
	
	public AudioClip[] sfx;
	
	public override int Weight{
		get{return slots + (magazine != null ? magazine.Weight : 0);}
	}
	
	public override string ToString(){
		if(magazine != null)
			return name + " ("+magazine.count+")";
		else
			return name;
	}
	
	public override PotentialActions Actions()
	{
		PotentialActions result = base.Actions();
		if(!isChambered && magazine != null && magazine.count > 0){
			result.Add("Chamber", typeof(ChamberWeapon), 10);
		}
		result.Add("Fire", typeof(FireWeapon), (isChambered ? ActionStatus.Okay : ActionStatus.Error), 9);
		if(magazine != null)
			result.Add("Unload", typeof(UnloadWeapon), -10);
		return result;
	}
	
	public override PotentialActions ItemActions(Item item){
		PotentialActions result = base.ItemActions(item);
		Magazine asMag = item as Magazine;
		if(magazine == null && asMag != null && asMag.ammoType == ammoType)
			result.Add("Load", typeof(LoadWeapon), 5);
		if(magazine != null && item == null){
			result.Add("Unload To", typeof(UnloadWeapon), -10);
		}
		return result;
	}
}
