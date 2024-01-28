using UnityEngine;
using System.Collections;
using System.Reflection;

public class ItemHolder {

	public Item held;
	
	public bool IsFree{
		get{return !InUse && held == null;}
	}
	
	public bool InUse{
		get{return m_currentAction != null;}
	}
	
	protected Action m_currentAction;
	
	public void Hold(Item item){
		if(item == null && held != null && held.holder == this){
			held.holder = null;
		}
		else if(item != null){
			item.holder = this;
		}
		held = item;
	}
	
	public bool SetAction(Action a){
		if(m_currentAction != null && a != null){
			return false;
		}
		else{
			m_currentAction = a;
			return true;
		}
	}
	
	public float TimeOnAction(){
		if(m_currentAction == null)
			return 0;
		else
			return m_currentAction.TimeUntilCompletion();
	}
	
	public PotentialActions Actions(){
		if(held == null){
			//Empty hands can't do anything yet...
			return new PotentialActions();
		}
		else{
			return held.Actions();
		}
	}
	
	public PotentialActions ItemActions(ItemHolder itemHolder){
		PotentialActions actionList;
		if(held != null){
			actionList = held.ItemActions(itemHolder.held);
		}
		else{
			actionList = new PotentialActions();
		}
		if(held != null || itemHolder.held != null){
			actionList.Add("Swap", typeof(SwapItems));
		}
		return actionList;
		
	}
	
	public void Update(){
		if(m_currentAction != null){
			m_currentAction.Update();
		}
	}
}
