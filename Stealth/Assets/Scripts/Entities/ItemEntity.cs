using UnityEngine;
using System.Collections;

public class ItemEntity : Entity {

	public Item item;
	
	void Update(){
		transform.rotation =  transform.rotation * Quaternion.Euler(0, Time.deltaTime * 80, 0);
	}
}
