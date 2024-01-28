using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardSearchState : GuardState {
	
	List<Vector3> m_path;
	GridSquare targetSquare;
	
	public override void OnStateEntered(Guard guard){
		guard.DisplayMode = guard.searchingMaterial;
	}
	
	public override void Update (Guard guard) {
		if(LevelLogic.alert == AlertMode.Alert){
			guard.ChangeState(new GuardAgroState());
			return;
		}
		else if(LevelLogic.alert < AlertMode.Warning){
			guard.PopState();
			return;
		}
		
		if(targetSquare != null && targetSquare.bakedSight >= 0.95f){
			targetSquare = null;
		}
		if(targetSquare == null){
			targetSquare = LevelLogic.AssignSearchSquare(guard);
			m_path = null;
		}
		if(targetSquare != null && (m_path == null || m_path.Count == 0)){
			//Get new path
			m_path = GetPathTo(guard, targetSquare.transform.position);
		}
		if(targetSquare != null){
			Color c = Color.yellow;
			c.a = 0.5f;
			Debug.DrawLine(guard.transform.position, targetSquare.transform.position, c);
		}
		
		MoveAlongPath(guard, m_path);
		
		//Look along direction of travel
		if(guard.rigidbody.velocity.magnitude > 0.01f){
			Look(guard, guard.rigidbody.velocity);
		}
		
	}
}
