using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TickManager : MonoBehaviour {

	public static System.Text.StringBuilder Log = new System.Text.StringBuilder();

	public static float tickInterval = 0.4f;
	private static TickManager instance;
	
	private float nextTick;

	public static float InterTick{
		get{
			return Mathf.InverseLerp (instance.nextTick - tickInterval, instance.nextTick, Time.time);
		}
	}

	private void Awake(){
		instance = this;
	}
	
	private void Update(){
		if(Time.time > nextTick){
			MoveCrates();
			nextTick = Time.time + tickInterval;
		}
	}

	/**
	 * Target conveyor can accept crate iff
	 * 1.1 It does not current contain a crate.
	 * and
	 * 1.2 There is no other crate targeting the same conveyor with higher priority
	 * or
	 * 2. The target is part of a 'proper' loop back to this conveyor (no 180 deg turns)
	 */	
	private static void MoveCrates(){
		var pendingEvents = new List<SimEvent>();
		var validEvents = new List<SimEvent>();






















		var stalledCrates = new List<Crate>(Crate.allCrates);
		var movingCrates = new List<Crate>(Crate.allCrates.Count);
		//Clear cached data
		foreach(var crate in Crate.allCrates){
			crate.ClearMoveChain();
		}
		foreach(var conveyor in Conveyor.allConveyors){
			conveyor.NextTaken = conveyor.NextGiven;
		}
		var moveLog = new System.Text.StringBuilder();
		moveLog.AppendLine("Frame " + Time.frameCount);
		
		bool checkForMovement = true;
		while(checkForMovement){
			moveLog.AppendLine("Check for movement");
			checkForMovement = false;
			for(int i = 0; i < stalledCrates.Count; ){
				var crate = (Crate)stalledCrates[i];
				Conveyor to = (Conveyor)crate.conveyor.GivesTo;
				if(to != null){
					bool canMoveTo = false;
					//target conveyor is predicted empty
					if(to.NextTaken == null){
						canMoveTo = true;
						moveLog.AppendLine (crate.name + " on " + crate.conveyor.name + " will move to empty " + to.name);
					}
					//target conveyor is predicted w/ moving crate, but we supercede it
					else if(to.NextGiven != to.NextTaken
					        && Crate.CompareMovementPriority(crate, to.NextTaken) < 0){
						canMoveTo = true;
						//undo movement of superceded movechain
						moveLog.Append(
							crate.name + " on " + crate.conveyor.name + " will move to " + to.name
							);
						Crate blockedCrate = to.NextTaken;
						while(blockedCrate != null){
							stalledCrates.Add(blockedCrate);
							movingCrates.Remove (blockedCrate);
							blockedCrate.conveyor.NextTaken = blockedCrate;
							moveLog.Append (", blocking " + blockedCrate.name);
							Crate toClear = blockedCrate;
							blockedCrate = blockedCrate.moveChainNext;
							toClear.ClearMoveChain();
						}
						moveLog.AppendLine ();
					}
					if(canMoveTo){
						//moving into a currently occupied conveyor: become part of movechain
						if(to.NextGiven != null){
							to.NextGiven.moveChainNext = crate;
							crate.moveChainHead = to.NextGiven.moveChainHead;
						}
						crate.conveyor.NextTaken = null;
						to.NextTaken = crate;
						movingCrates.Add (crate);
						stalledCrates.Remove (crate);
						checkForMovement = true;
					}
					else{
						moveLog.AppendLine (
							crate.name + " on " + crate.conveyor.name + " cannot move"
							);
						i++;
					}
				}
				else{
					moveLog.AppendLine (crate.name + " on " + crate.conveyor.name + " has no dest");
					i++;
				}
			}
		}
		foreach(var crate in movingCrates){
			crate.timeStalled = 0;
		}
		foreach(var crate in stalledCrates){
			crate.timeStalled++;
		}
		foreach(var conveyor in Conveyor.allConveyors){
			conveyor.ApplyCrateChanges();
			if(conveyor.NextGiven != null){
				conveyor.NextGiven.UpdatePosition(conveyor);
			}
		}
		//SANITY CHECK
		HashSet<Crate> cratesOnConveyors = new HashSet<Crate>();
		foreach(var conveyor in Conveyor.allConveyors){
			if(conveyor.NextGiven != null){
				if(cratesOnConveyors.Contains(conveyor.NextGiven)){
					Debug.LogError ("Crate is on two conveyors!\n"+moveLog);
					Debug.Break ();
				}
				else{
					cratesOnConveyors.Add (conveyor.NextGiven);
				}
			}
		}
		foreach(var crate in Crate.allCrates){
			if(crate.conveyor.NextGiven != crate){
				Debug.LogError ("Crate on wrong conveyor!\n"+moveLog, crate);
				Debug.Break ();
			}
		}
	}
}
