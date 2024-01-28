//This is now an AI function rather than an action.
/*
using UnityEngine;
using System.Collections;

public class FaceSquare : Action {

	private GridSquare target;
	
	public FaceSquare(Entity actor, GridSquare target) : base(actor){
		name = "Face";
		this.target = target;
	}
	
	public override void Update(){
		if(completed || cancelled)
			return;
		if(target != source.location){
			Quaternion desiredRotation = Quaternion.LookRotation(target.transform.position - source.location.transform.position);
			float hereToThere = Quaternion.Angle(desiredRotation, source.transform.rotation);
			if(hereToThere > source.turnRate * GameTime.deltaTime)
				source.transform.rotation = Quaternion.Slerp(source.transform.rotation, desiredRotation, source.turnRate * GameTime.deltaTime / hereToThere);
			else
				source.transform.rotation = desiredRotation;
		}
	}
}
 */