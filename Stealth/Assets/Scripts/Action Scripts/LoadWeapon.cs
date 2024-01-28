using UnityEngine;
using System.Collections;

public class LoadWeapon : Action {

	private Firearm gun;
	private Magazine ammo;
	private float completionTime;
	
	public LoadWeapon(Entity actor, ItemHolder source, ItemHolder destination, Item item) : base(actor, source, destination, item){
		name = "Load";
		gun = source.held as Firearm ?? destination.held as Firearm;
		ammo = destination.held as Magazine ?? source.held as Magazine;
		completionTime = gun.reloadTime;
		destination.SetAction(this);
	}
	
	public override void Update(){
		completionTime -= Time.deltaTime;
		if(completionTime <= 0){
			OnCompletion();
		}
	}
	
	public override float TimeUntilCompletion(){
		return completionTime;
	}
	
	public override void OnCompletion(){
		base.OnCompletion();
		destination.SetAction(null);
		if(ammo.holder != null){
			ammo.holder.Hold(null);
		}
		gun.magazine = ammo;
	}
	
	public override void Cancel ()
	{
		base.Cancel ();
		destination.SetAction(null);
	}
}
