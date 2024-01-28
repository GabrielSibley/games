using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardState : AIState<Guard> {
	
	protected void Look(Guard guard, Vector3 direction){
		guard.transform.rotation = Quaternion.RotateTowards(guard.transform.rotation, Quaternion.LookRotation(direction), guard.turnRate * Time.deltaTime);
	}
	
	protected List<Vector3> GetPathTo(Guard guard, Vector3 destination){
		List<Vector3> path = AStar.GetVectorPath(guard.transform.position, destination, Guard.GuardPathfindingWeight);
		AStar.SmoothPath(path);
		return path;
	}
	
	protected void MoveAlongPath(Guard guard, List<Vector3> path){
		//Follow path
		if(path != null){
			Vector3 toNext = path[0] - guard.transform.position;
			Debug.DrawLine(guard.transform.position, path[0]);
			guard.rigidbody.AddForce(toNext.normalized * Time.deltaTime * 10, ForceMode.Impulse);
			if(toNext.magnitude < 0.1f){
				path.RemoveAt(0);
			}
		}
		//Find other guards repulsive in proportion to their direction of travel
		Vector3 repulsionForce = Vector3.zero;
		foreach(Guard otherGuard in LevelLogic.guards){
			if(guard != otherGuard){
				Vector3 toOtherGuard = otherGuard.transform.position - guard.transform.position;
				float dot = Vector3.Dot(guard.rigidbody.velocity, otherGuard.rigidbody.velocity);
				if(toOtherGuard.magnitude < 1){
					Vector3 a = -toOtherGuard.normalized * Mathf.InverseLerp(0, 1, dot);
					float d = (1 - toOtherGuard.magnitude);
					repulsionForce += a * d * 10 * Time.deltaTime;
				}
			}
		}
		//Repulsion force in sum cannot excede 80% of pathing force
		repulsionForce = Vector3.ClampMagnitude(repulsionForce, 8);
		guard.rigidbody.AddForce(repulsionForce, ForceMode.Impulse);
	}
}
