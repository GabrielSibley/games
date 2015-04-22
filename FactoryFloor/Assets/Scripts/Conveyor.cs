using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum Direction {East, North, West, South};

public class Conveyor : TileFeature, ICrateGiver, ICrateTaker {

	public static List<Conveyor> allConveyors = new List<Conveyor>();
	static int conveyorCount = 0;

	public List<ICrateGiver> TakesFrom { get { return from; } }
	//TODO: Multiple give targets
	public ICrateTaker GivesTo { get { return to; } }
	public Crate NextTaken {
		get { return nextCrate; }
		set { nextCrate = value;}
	}

	public Crate NextGiven {
		get {
			return currentCrate;
		}
	}

	private ICrateTaker to;
	private List<ICrateGiver> from;
	private Crate currentCrate, nextCrate;

	public Direction direction;

	public override void AddToTile(Tile t){
		CurrentTile = t;
		t.features.Add (this);
	}

	public override void Remove(){
		CurrentTile.features.Remove(this);
		Destroy(gameObject);
	}

	public override void OnInputDown(){
		RotateCW();
	}

	public void TakeCrate(Crate newCrate){
		currentCrate = newCrate;
	}

	private void Awake(){
		name = "Conveyor " + ++conveyorCount;
		allConveyors.Add (this);
	}

	private void OnDestroy(){
		allConveyors.Remove (this);
	}

	public void RotateCW() {
		transform.Rotate (new Vector3(0, 0, -90));
		direction = (Direction)(((int)direction + 3) % 4);
	}

	public void ClearEdges(){
		to = null;
		from = new List<ICrateGiver>();
	}

	public void UpdateEdges(){
		to = null;
		int x = CurrentTile.x;
		int y = CurrentTile.y;
		OffsetByDirection(direction, ref x, ref y);
		Tile toTile = FloorLayout.GetTile (x, y);
		if(toTile != null){
			ICrateTaker toTaker = toTile.features.Where(feature => feature is ICrateTaker).FirstOrDefault() as ICrateTaker;
			if(toTaker != null){
				to = toTaker;
				to.TakesFrom.Add (this);
			}
		}
	}

	public void ApplyCrateChanges(){
		currentCrate = nextCrate;
		nextCrate = null;
	}

	public bool TryTakeCrateNext(Crate c){
		return false; //What have I done
	}

	private void OffsetByDirection(Direction direction, ref int x, ref int y){
		if(direction == Direction.North){
			y++;
		}
		if(direction == Direction.South){
			y--;
		}
		if(direction == Direction.East){
			x++;
		}
		if(direction == Direction.West){
			x--;
		}
	}
}
