//This is now an AI function rather than an action.

/*
using UnityEngine;
using System.Collections;

public class FaceEntity : Action {

	private Entity target;
	
	public FaceEntity(Entity actor, Entity target) : base(actor){
		name = "Face";
		this.target = target;
	}
	
	public override void Update(){
		if(cancelled || completed)
			return;
		if(target == null)
			completed = true;
		else if(target.location != source.location){
			Quaternion desiredRotation = Quaternion.LookRotation(target.location.transform.position - source.location.transform.position);
			float hereToThere = Quaternion.Angle(desiredRotation, source.transform.rotation);
			if(hereToThere > source.turnRate * Time.deltaTime)
				source.transform.rotation = Quaternion.Slerp(source.transform.rotation, desiredRotation, source.turnRate * Time.deltaTime / hereToThere);
			else
				source.transform.rotation = desiredRotation;
		}
	}
	
	public override void Cancel ()
	{
		base.Cancel ();
	}
}
 */