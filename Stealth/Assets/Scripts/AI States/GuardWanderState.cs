using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardWanderState : GuardState {
	
	List<Vector3> m_path;
	
	public override void OnStateEntered(Guard guard){
		guard.DisplayMode = guard.normalMaterial;
	}
	
	public override void Update (Guard guard) {
		if(LevelLogic.alert == AlertMode.Alert){
			guard.PushState(new GuardAgroState());
			return;
		}
		if(LevelLogic.alert == AlertMode.Evasion){
			guard.PushState(new GuardSearchState());
			return;
		}
		
		if(guard.suspicion > 0){
			guard.PushState(new GuardSearchState());
			LevelLogic.SetAlertMode(AlertMode.Warning);
			return;
		}
		
		if(m_path == null || m_path.Count == 0){			
			//Get new path
			GridSquare dest = LevelLogic.largestContinuousSet[Random.Range(0, LevelLogic.largestContinuousSet.Length)];
			m_path = GetPathTo(guard, dest.transform.position);
		}
		//Follow path
		MoveAlongPath(guard, m_path);
		
		//Look along direction of travel
		if(guard.rigidbody.velocity.magnitude > 0){
			Look(guard, guard.rigidbody.velocity);
		}
		
	}
}
