using UnityEngine;
using System.Collections;

public class ChamberWeapon : Action {

	private Firearm weapon;
	private ItemHolder hand;
	private float completionTime;
	
	public ChamberWeapon (Entity actor, ItemHolder source, Firearm item) : base(actor, source, null, item){
		name = "Chamber";
		this.weapon = item;
		completionTime = weapon.chamberTime;
	}
	
	public ChamberWeapon (Entity actor, ItemHolder source, ItemHolder destination, Item item) : this(actor, source, (Firearm)item){
		
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
		weapon.isChambered = true;
		weapon.magazine.count -= 1;
	}
	
	public override void Cancel ()
	{
		base.Cancel ();
	}
}
