using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Action{
	
	public static System.Type[] s_invokeTypes = new System.Type[]{typeof(Entity), typeof(ItemHolder), typeof(ItemHolder), typeof(Item)};
	
	public string name = "GENERIC";
	public Entity actor;
	public ItemHolder source;
	public ItemHolder destination;
	public Item item;
	public bool validAssignment;
	public float startTime;
	public float endTime;
	public bool completed;
	public bool cancelled;
	public ActionStatus status;
	public virtual float TotalTime{
		get{return TimeUntilCompletion();}
	}
	
	public Action(Entity actor, ItemHolder source, ItemHolder destination, Item item){
		this.actor = actor;
		this.source = source;
		this.destination = destination;
		this.item = item;
		validAssignment = source.SetAction(this);
	}
	
	public abstract void Update();
	
	public virtual float TimeUntilCompletion(){
		return Mathf.Infinity;
	}
	
	public virtual string StatusString(){
		return Utility.FormatTimeSeconds(TimeUntilCompletion());
	}
	
	public virtual void OnCompletion(){
		completed = true;
		source.SetAction(null);
	}
	
	public virtual void Cancel(){
		cancelled = true;
		source.SetAction(null);
	}
	
}

public enum ActionStatus{
	Okay,
	Warning,
	Error
}


