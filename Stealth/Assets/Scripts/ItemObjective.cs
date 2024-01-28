using UnityEngine;
using System.Collections;

public class ItemObjective : MonoBehaviour {

	public bool m_completed;

	void Start(){
		Player.Objectives.Add(this);
	}

	void OnTriggerEnter(Collider other){
		Player p = other.GetComponent<Player>();
		if(p){
			m_completed = true;
			gameObject.SetActiveRecursively(false);
		}
	}
}
