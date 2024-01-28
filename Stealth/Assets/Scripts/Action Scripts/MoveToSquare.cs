//This is now an AI function
/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveToSquare : Action{

	public List<GridSquare> path;
	private float completionTime;
	public override float TotalTime{
		get{
			if(path.Count == 0)
				return 0;
			float d = 0;
			for(int i = 0; i < path.Count - 1; i++)
				d += (path[i+1].transform.position - path[i].transform.position).magnitude;
			return d / source.moveSpeed + TimeUntilCompletion();
		}
	}
	
	public MoveToSquare(Entity actor, GridSquare target):base(actor){
		name = "Move";
		path = AStar.GetPath(actor.location, target);
	}
	
	public MoveToSquare(Entity actor, GridSquare target, AStar.PathfindingWeightFunc pwf):base(actor){
		path = AStar.GetPath(actor.location, target, pwf);
		if(path.Count > 0)
			completionTime = (actor.location.transform.position - path[0].transform.position).magnitude / actor.moveSpeed;
		else{
			completed = true;
			completionTime = 0;
		}
	}
	
	public MoveToSquare(Entity actor, MoveToSquare oldMoveAction, GridSquare target, AStar.PathfindingWeightFunc pwf):base(actor){
		path = AStar.GetPath(actor.location, target, pwf);
		if(path.Count > 0){
			float bonusTime = 0;
			if(oldMoveAction != null && oldMoveAction.path.Count > 0 && oldMoveAction.path[0] == path[0])
				bonusTime = oldMoveAction.TimeUntilCompletion();
			completionTime = (actor.location.transform.position - path[0].transform.position).magnitude / actor.moveSpeed - bonusTime;
		}
		else{
			completed = true;
			completionTime = 0;
		}
	}
	
	public override void Update(){
		if(completed || cancelled)
			return;
		completionTime -= GameTime.deltaTime;
		if(completionTime > 0)
			return;
		if(path.Count > 0){
			if(path[0] != source.location){
				source.PlaceAtSquare(path[0]);
			}
			path.RemoveAt(0);
		}
		if(path.Count > 0){
			completionTime  = (source.location.transform.position - path[0].transform.position).magnitude / source.moveSpeed;
		}
		if(path.Count == 0){
			completed = true;
		}
	}
	
	public override float TimeUntilCompletion(){
		return completionTime;
	}
}
*/
