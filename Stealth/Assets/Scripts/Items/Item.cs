using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	public Texture2D icon;
	public int slots = 1;
	public string displayName = "PLACEHOLDER";
	public virtual int Weight{
		get{return slots;}
	}
	//[HideInInspector]
	public ItemHolder holder;
	
	public override string ToString(){
		return name;
	}
	
	public Item Spawn(){
		Item i = Object.Instantiate(this, Vector3.zero, Quaternion.identity) as Item;
		return i;
	}
	
	public virtual PotentialActions Actions(){
		PotentialActions result = new PotentialActions();
		return result;
	}
	
	public virtual PotentialActions ItemActions(Item item){
		PotentialActions result = new PotentialActions();
		return result;
	}
}
