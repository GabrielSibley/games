using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Crate : MonoBehaviour {

	private static int nextCrateId;
	public static List<Crate> allCrates = new List<Crate>();

	public Conveyor conveyor;
	public int timeStalled;

	private Vector3 lerpPositionFrom, lerpPositionTo;

	//A continuous string of moving crates with no gaps is a MoveChain
	//two crates with the same movechainhead are in the same movechain
	public Crate moveChainHead; //front-most member of movechain
	public Crate moveChainNext; //next member behind in movechain

	public static int CompareMovementPriority(Crate a, Crate b)	{
		int timePriority = b.timeStalled - a.timeStalled;
		if(timePriority != 0)
		{
			return timePriority;
		}
		int positionPriority = b.PositionPriority - a.PositionPriority;
		return positionPriority;
	}

	public void UpdatePosition(Conveyor newConveyor){
		if(conveyor != null){
			lerpPositionFrom = conveyor.transform.position;
		}
		else{
			transform.position = lerpPositionFrom = newConveyor.transform.position;
		}
		conveyor = newConveyor;
		lerpPositionTo = conveyor.transform.position;
	}

	//Topmost then rightmost
	public int PositionPriority{
		get{
			return conveyor.CurrentTile.y * FloorLayout.Width - conveyor.CurrentTile.x;
		}
	}

	private void Awake(){
		allCrates.Add(this);
		name = "Crate " + nextCrateId++;
	}

	private void OnDestroy(){
		allCrates.Remove(this);
	}

	private void Update(){
		transform.position = Vector3.Lerp (lerpPositionFrom, lerpPositionTo, TickManager.InterTick);
	}

	public void ClearMoveChain(){
		moveChainHead = this;
		moveChainNext = null;
	}
}
