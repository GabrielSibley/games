using UnityEngine;
using System.Collections;

public class UnloadWeapon : Action {

	private Firearm gun;
	private float completionTime;
	
	public UnloadWeapon(Entity actor, ItemHolder source, ItemHolder destination, Item item):base(actor, source, destination, item){
		name = "Unload";
		this.gun = item as Firearm;
		completionTime = gun.reloadTime;
		if(destination != null){
			destination.SetAction(this);
		}
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
	
	public override void OnCompletion () {
		base.OnCompletion ();
		if(destination != null)
			destination.Hold(gun.magazine);
		else{
			//TODO: Drop item to ground instead of losing it to oblivion
		}
		gun.magazine = null;
		if(destination != null){
			destination.SetAction(null);
		}
	}
	
	public override void Cancel (){
		base.Cancel ();
		if(destination != null){
			destination.SetAction(null);
		}
	}
	
}
