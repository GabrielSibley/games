using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardAgroState : GuardState {
	
	protected float surpriseTime;
	protected List<Vector3> m_pathToPlayer;
	
	public GuardAgroState(){
		surpriseTime = 0.3f;
	}
	
	public override void OnStateEntered(Guard guard){
		guard.DisplayMode = guard.alertMaterial;
	}
	
	public override void Update (Guard guard) {
		if(LevelLogic.alert == AlertMode.Evasion){
			guard.ChangeState(new GuardSearchState());
			return;
		}
		else if(LevelLogic.alert <= AlertMode.Warning){
			guard.PopState();
			return;
		}
		if(surpriseTime > 0){
			surpriseTime -= Time.deltaTime;
		}
		//Can see player: look at them
		if(guard.CanSeePlayer && Player.m_instance != null){
			Vector3 fromTo = Player.m_instance.transform.position - guard.transform.position;
			fromTo.y = 0;
			Look(guard, fromTo);
			Quaternion lookAtPlayer = Quaternion.LookRotation(fromTo);			
			guard.isOnTarget = surpriseTime <= 0 && Quaternion.Angle(guard.transform.rotation, lookAtPlayer) == 0;
		}
		else if (Player.m_instance != null){
			//Move to player
			if(m_pathToPlayer == null || m_pathToPlayer.Count == 0){
				//Get new path
				m_pathToPlayer = GetPathTo(guard, Player.m_instance.transform.position);
			}
			MoveAlongPath(guard, m_pathToPlayer);
			//Look along direction of travel
			if(guard.rigidbody.velocity.magnitude > 0){
				Look(guard, guard.rigidbody.velocity);
			}
		}
	}
}
