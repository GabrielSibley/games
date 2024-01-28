using UnityEngine;
using System.Collections;

public class SwapItems : Action {
	
	public const float baseSwapTime = 0.3f;
	public const float perSizeSwapTime = 0.1f;
	
	private float completionTime;
	
	public SwapItems(Entity actor, ItemHolder source, ItemHolder destination):base(actor, source, destination, null){
		//Swap items also lays claim to the destination IH's action space
		destination.SetAction(this);
		name = "Move Item";
		completionTime = baseSwapTime 
			+ ((source.held != null ? source.held.slots : 0) + (destination.held != null? destination.held.slots : 0)) * perSizeSwapTime;	
	}
	
	public SwapItems(Entity actor, ItemHolder source, ItemHolder destination, Item item):base(actor, source, destination, item){
		//Swap items also lays claim to the destination IH's action space
		destination.SetAction(this);
		name = "Move Item";
		completionTime = baseSwapTime 
			+ ((source.held != null ? source.held.slots : 0) + (destination.held != null? destination.held.slots : 0)) * perSizeSwapTime;	
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
		Item sourceItem = source.held;
		Item destItem = destination.held;
		source.Hold(destItem);
		destination.Hold(sourceItem);
	}
	
	public override void Cancel ()
	{
		base.Cancel ();
		destination.SetAction(null);
	}
	
}
