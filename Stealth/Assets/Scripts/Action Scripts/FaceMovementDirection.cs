//This is now an AI function rather than an action.
/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FaceMovementDirection : Action {
	
	private MoveToSquare moveAction;
	private GridSquare lookingAtSquare;
	
	public FaceMovementDirection(Entity actor, MoveToSquare moveAction):base(actor){
		name = "Face";
		this.moveAction = moveAction;
	}
	
	public override void Update(){
		if(completed || cancelled)
			return;
		if(moveAction.completed || moveAction.cancelled)
			completed = true;
		lookingAtSquare = LastVisiblePathSquare();
		if(lookingAtSquare != null && lookingAtSquare != source.location){
			Quaternion desiredRotation = Quaternion.LookRotation(lookingAtSquare.transform.position - source.location.transform.position);
			float hereToThere = Quaternion.Angle(desiredRotation, source.transform.rotation);
			if(hereToThere > source.turnRate * GameTime.deltaTime)
				source.transform.rotation = Quaternion.Slerp(source.transform.rotation, desiredRotation, source.turnRate * GameTime.deltaTime / hereToThere);
			else
				source.transform.rotation = desiredRotation;
		}
		
	}
	
	private GridSquare LastVisiblePathSquare(){
		if(moveAction.path == null || moveAction.path.Count == 0)
			return null;
		int i = 0;
		for(; i < moveAction.path.Count; i++){
			GridSquare sq = moveAction.path[i];
			if(Utility.PermissiveSquareToSquareCast(source.location, sq, LayerSets.terrain))
				break;
		}
		if(i < moveAction.path.Count && i <= 4)
			return moveAction.path[i]; //Look at the square just after the last one you can see
		return moveAction.path[i-1];
	}
}
 */