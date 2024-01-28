using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PotentialActions {
	/* 	ACTION PRIORITY TABLE
		10	chamber
		9	fire
		5	load
		0	swap
		-10	unload
	*/
	
	private List<PotentialAction> actions;
	private bool m_sorted;
	
	public int Count{
		get{ return actions.Count;}
	}
	
	public PotentialActions(){
		actions = new List<PotentialAction>();
	}
	
	public void Add(string s, System.Type actionType){
		Add(s, actionType, ActionStatus.Okay, 0);
		m_sorted = false;
	}
	
	public void Add(string s, System.Type actionType, ActionStatus status){
		actions.Add(new PotentialAction(s, actionType, status, 0));
		m_sorted = false;
	}
	
	public void Add(string s, System.Type actionType, int priority){
		actions.Add(new PotentialAction(s, actionType, ActionStatus.Okay, priority));
		m_sorted = false;
	}
	
	public void Add(string s, System.Type actionType, ActionStatus status, int priority){
		actions.Add(new PotentialAction(s, actionType, status, priority));
		m_sorted = false;
	}
	
	public PotentialAction GetAction(int i){
		if(!m_sorted){
			m_sorted = true;
			//Sort by priority descending
			actions.Sort(delegate(PotentialAction a, PotentialAction b){return b.priority.CompareTo(a.priority);});
		}
		return actions[i];
	}
}

public struct PotentialAction{
	public string name;
	public System.Type actionType;
	public ActionStatus status;
	public int priority;
	
	public PotentialAction(string name, System.Type actionType, ActionStatus status, int priority){
		this.name = name;
		this.actionType = actionType;
		this.status = status;
		this.priority = priority;
	}
}


