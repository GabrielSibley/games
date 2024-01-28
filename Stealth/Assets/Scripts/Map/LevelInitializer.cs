using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelInitializer : MonoBehaviour {

	public GridSquare floorPrefab;
	public GameObject floorPlatePrefab;
	public GridSquare wallPrefab;
	public Transform pathMarker;
	public Guard guardPrefab;
	public Player playerPrefab;
	public ItemEntity secretPrefab;
	public Entity guardSpawnerPrefab;
	public Tracer tracerPrefab;
	public int levelWidth;
	public int levelHeight;
	public float nodeSize = 0.5f;
	
	// Use this for initialization
	public void BuildLevel() {
		LevelLogic.nodeSize = nodeSize;
		LevelLogic.grid = new GridSquare[levelWidth, levelHeight];
		
		//Spawn terrain objects
		for(int x = 0; x < levelWidth; x++){
			for(int y = 0; y < levelHeight; y++){
				//Determine whether tile is wall or floor
				GridSquare prefab;
				if(x == 0 || x == levelWidth - 1 || y == 0 || y == levelWidth - 1)
				{
					prefab = wallPrefab;
				}
				else{
					prefab = floorPrefab;
				}
				GridSquare obj = Object.Instantiate(prefab, new Vector3(x*nodeSize, 0, y*nodeSize), Quaternion.identity) as GridSquare;
				obj.transform.parent = this.transform;
				obj.x = x;
				obj.y = y;
				LevelLogic.grid[x,y] = obj;
			}
		}
		
		//Create floorplate
		GameObject floor = Object.Instantiate(floorPlatePrefab, 
			new Vector3((levelWidth - 1) * nodeSize / 2, 
				0, 
				(levelHeight - 1) * nodeSize / 2 ), 
				Quaternion.identity) as GameObject;
		floor.renderer.material.mainTextureScale = new Vector2(levelWidth, levelHeight);
		floor.transform.localScale = new Vector3(levelWidth * nodeSize, 1, levelHeight * nodeSize);
		
		//Setup pathfinding graph
		BuildNavGraph();
		BuildConnectivityMap();
		BuildLargestContinuousSet();

		//Calc sentry values
		CalcSentryValues();
		//Sort squares by sentry excellence
		System.Array.Sort(LevelLogic.largestContinuousSet, delegate (GridSquare a, GridSquare b){
			return a.SentryExcellence.CompareTo(b.SentryExcellence);
		});
		
		//Place guards
		for(int i = 0; i <= LevelLogic.largestContinuousSet.Length / 100; i++){
			bool isSentry = true; //Random.value > 0.5f;
			//Sentry type
			if(isSentry){
				GridSquare loc = LevelLogic.largestContinuousSet[LevelLogic.largestContinuousSet.Length - Random.Range(1, LevelLogic.largestContinuousSet.Length / 3)];
				int failCount = 0;
				while(failCount < 250 && LevelLogic.guards.Exists(delegate(Guard existingGuard){return !Utility.RaisedLineCast(existingGuard.transform.position, loc.transform.position, LayerSets.terrain);})){
					loc = LevelLogic.largestContinuousSet[LevelLogic.largestContinuousSet.Length - Random.Range(1, LevelLogic.largestContinuousSet.Length / 2)];
					failCount++;
				}
				Guard g = Object.Instantiate(guardPrefab, loc.transform.position, Quaternion.identity) as Guard;
				g.PushState(new GuardSentryState(loc));
				g.PlaceAtSquare(loc);
				LevelLogic.guards.Add(g);
			}
			//Patrol type
			else{
				GuardPatrolState gps = new GuardPatrolState();
				Guard g = Object.Instantiate(guardPrefab, gps.PlacementPoint, Quaternion.identity) as Guard;				
				g.PushState(gps);
				LevelLogic.guards.Add(g);
			}
		}
		//Place player somewhere in bottom 20% of sentry values
		GridSquare randomSquare = LevelLogic.largestContinuousSet[Random.Range(0, LevelLogic.largestContinuousSet.Length / 5)];
		Object.Instantiate(playerPrefab, randomSquare.transform.position, Quaternion.identity);

		//Place guard spawners
		for(int i = 0; i <= LevelLogic.largestContinuousSet.Length / 240 + 1; i++){
			GridSquare loc = LevelLogic.largestContinuousSet[Random.Range(0, LevelLogic.largestContinuousSet.Length)];
			Entity e = Object.Instantiate(guardSpawnerPrefab, loc.transform.position, Quaternion.identity) as Entity;
			LevelLogic.guardSpawns.Add(e);
		}

		//Place Secrets		
		for(int i = 0; i < 3; i++){
			GridSquare loc = LevelLogic.largestContinuousSet[Random.Range(0, LevelLogic.largestContinuousSet.Length)];
			ItemEntity ie = Object.Instantiate(secretPrefab, loc.transform.position, Quaternion.identity) as ItemEntity;
			ie.PlaceAtSquare(loc);
		}
	}
	
	public void BuildNavGraph(){		
		for(int x = 0; x < levelWidth; x++){
			for(int y = 0; y < levelHeight; y++){
				GridSquare t = LevelLogic.grid[x,y];
				if(t.Passable){
					foreach(GridSquare n in GetAllNeighbours(t)){
						if(n.Passable){
							t.neighbours.Add(n);
						}
						t.allNeighbours.Add(n);
					}
				}
			}
		}
	}

	public IEnumerable<GridSquare> GetAllNeighbours(GridSquare t){
		List<GridSquare> result = new List<GridSquare>();
		for(int x = 0; x < levelWidth; x++){
			for(int y = 0; y < levelHeight; y++){
				GridSquare sq = LevelLogic.grid[x,y];
				int dx = sq.x - t.x;
				int dy = sq.y - t.y;
				if(dx == -1 || dx == 1 || dy == -1 || dy == 1){
					result.Add (sq);
				}
			}
		}
		return result;
	}
	
	public void BuildConnectivityMap(){
		for(int x = 0; x < levelWidth; x++){
			for(int y = 0; y < levelHeight; y++){
				UnionFind.MakeSet(LevelLogic.grid[x,y]);
			}
		}
		for(int x = 0; x < levelWidth; x++){
			for(int y = 0; y < levelHeight; y++){
				if(!LevelLogic.grid[x,y].Passable)
					continue;
				if(x < levelWidth-1 && LevelLogic.grid[x+1, y].Passable)
					UnionFind.Union(LevelLogic.grid[x,y], LevelLogic.grid[x+1, y]);
				if(y < levelHeight-1 && LevelLogic.grid[x, y+1].Passable)
					UnionFind.Union(LevelLogic.grid[x,y], LevelLogic.grid[x, y+1]);
				if(x < levelWidth-1 && y < levelHeight-1 && LevelLogic.grid[x+1, y+1].Passable && LevelLogic.grid[x+1,y].Passable && LevelLogic.grid[x, y+1].Passable)
					UnionFind.Union(LevelLogic.grid[x,y], LevelLogic.grid[x+1, y+1]);
			}
		}
	}
	
	public void BuildLargestContinuousSet(){
		//Find root of biggest
		GridSquare bestYet = null;
		int bestSizeYet = 0;
		for(int x = 0; x < levelWidth; x++){
			for(int y = 0; y < levelHeight; y++){
				if(LevelLogic.grid[x,y].setSize > bestSizeYet){
					bestYet = LevelLogic.grid[x,y];
					bestSizeYet = LevelLogic.grid[x,y].setSize;
				}
			}
		}
		//Build set of biggest
		LevelLogic.largestContinuousSet = new GridSquare[bestSizeYet];
		int i = 0;
		for(int x = 0; x < levelWidth; x++){
			for(int y = 0; y < levelHeight; y++){
				if(UnionFind.Find(LevelLogic.grid[x,y]) == bestYet){
					LevelLogic.largestContinuousSet[i] = LevelLogic.grid[x,y];
					i++;
				}
			}
		}
	}
	
	public int CountPassableContinuousSets(){
		List<GridSquare> parents = new List<GridSquare>();
		for(int x = 0; x < levelWidth; x++){
			for(int y = 0; y < levelHeight; y++){
				GridSquare sq = LevelLogic.grid[x,y];
				if(sq.Passable && !parents.Contains(UnionFind.Find(sq))){
					parents.Add(UnionFind.Find(sq));
				}
			}
		}
		return parents.Count;
	}

	public void CalcSentryValues(){
		for(int i = 0; i < LevelLogic.largestContinuousSet.Length; i++){
			//Find out how many squares you can see from this square and in what direction they lie
			GridSquare a = LevelLogic.largestContinuousSet[i];
			for(int j = i+1; j < LevelLogic.largestContinuousSet.Length; j++){				
				GridSquare b = LevelLogic.largestContinuousSet[j];
				if(!Utility.RaisedLineCast(a.transform.position, b.transform.position, LayerSets.terrain)){
					//Can see B from A
					a.visibleSquares.Add(b);
					b.visibleSquares.Add(a);
					Vector3 aToB = b.transform.position - a.transform.position;
					int direction = Mathfx.Repeat(Mathf.FloorToInt(Mathf.Atan2(aToB.z, aToB.x) / (2*Mathf.PI) * GridSquare.kSentryDivisions + 0.5f), GridSquare.kSentryDivisions);
					a.SentryValues[direction] += 1;
					b.SentryValues[(direction + GridSquare.kSentryDivisions/2) % GridSquare.kSentryDivisions] += 1;					
				}
			}
			
			//For each direction, add up the squares for all the directions that are within a guard's vision arc when facing that direction.			
			int[] inArcSquareCount = new int[GridSquare.kSentryDivisions];
			int visionWidth = Mathf.FloorToInt(guardPrefab.visionArc * inArcSquareCount.Length / 360f) / 2;
			for(int j = 0; j < inArcSquareCount.Length; j++){
				for(int k = -visionWidth; k <= visionWidth; k++){
					inArcSquareCount[j] += a.SentryValues[Mathfx.Repeat(j+k, inArcSquareCount.Length)];
				}
			}
			
			//Total sentry value is then based on how good the directions the guard can see are.
			for(int j = 0; j < inArcSquareCount.Length; j++){
				a.SentryValues[j] = 0;
				for(int k = -visionWidth; k <= visionWidth; k++){
					a.SentryValues[j] += inArcSquareCount[Mathfx.Repeat(j+k, inArcSquareCount.Length)];
				}
				a.SentryValues[j] /= visionWidth * 2 + 1;
			}
			
			int[] adjValues = a.AdjustedSentryValues;
			for(int j = 0; j < adjValues.Length; j++){
				for(int k = -visionWidth; k <= visionWidth; k++){
					if(k == 0){
						adjValues[j] += a.SentryValues[Mathfx.Repeat(j+k, inArcSquareCount.Length)] * (visionWidth * 2 + 1);
					}
					else{
						adjValues[j] -= a.SentryValues[Mathfx.Repeat(j+k, inArcSquareCount.Length)];
					}
				}
			}
			
			a.SentryExcellence = GuardSentryState.CalcSquareSentryExcellence(a);			
		}
	}
}
