using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Entity : MonoBehaviour {
	public GridSquare Location{get{return LevelLogic.ClosestGridSquareTo(this);}}
	public float moveSpeed = 1.5f;
	public float turnRate = 220; //Degrees / sec
	public List<Action> actions;
	protected List<Action> autoActions;
	public float health = 10;
	public float maxHealth = 10;
	
	public void PlaceAtSquare(GridSquare sq){
		if(sq != null){
			transform.position = sq.transform.position;
		}
		else{
			//No place for us here!
			Destroy(gameObject);
		}
	}
	
	public void ModifyHealth(float amount){
		health = Mathf.Min(amount + health, maxHealth);
		if(health <= 0)
			OnDie();
	}
	
	public virtual void OnDie(){
		TextLog.AddEntry(this.name+" died a generic death!");
		Destroy(this.gameObject);
	}
	
	public virtual float[] GetShotDeviationModifiers(Firearm weapon){
		float[] result = new float[]{1, 0};
		return result;
	}
	
	public virtual Vector3 WeaponOrigin(){
		return transform.position + Vector3.up;
	}
	
	public virtual Vector3 FiringDirection(){
		return transform.forward;
	}
}
