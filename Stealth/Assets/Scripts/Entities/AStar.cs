using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar {

	public delegate float PathfindingWeightFunc(GridSquare a, GridSquare b);
	
	public static List<Vector3> GetVectorPath(Vector3 start, Vector3 goal, PathfindingWeightFunc func){
		GridSquare startNode = LevelLogic.GetClosestSquare(start);
		GridSquare endNode = LevelLogic.GetClosestSquare(goal);
		List<GridSquare> nodePath = GetPath (startNode, endNode, func);
		//Path is invalid
		if(nodePath.Count == 0){
			return new List<Vector3>();
		}
		List<Vector3> path = new List<Vector3>(nodePath.Count+2);
		path.Add (start);
		foreach(GridSquare n in nodePath){
			path.Add(n.transform.position);
		}
		path.Add (goal);
		return path;
	}
	
	public static List<GridSquare> GetPath(GridSquare start, GridSquare goal){
		return GetPath(start, goal, EuclideanPathfindingWeight);
	}
	
	public static List<GridSquare> GetPath(GridSquare start, GridSquare goal, PathfindingWeightFunc PathFunc){

		List<GridSquare> closedSet = new List<GridSquare>();
		PriorityQueue<GridSquare> openSet = new PriorityQueue<GridSquare>();
		openSet.Enqueue(start);
		start.G = 0;
		start.H = (start.Position - goal.Position).magnitude;
		start.F = start.H;
		start.Prev = null;
		
		while(openSet.Count > 0){
			GridSquare n = openSet.Dequeue();
			if(n == goal){
				List<GridSquare> path = ReconstructPath(goal);
				return path;
			}
			closedSet.Add(n);
			foreach(GridSquare possibleNext in n.neighbours){
				if(closedSet.Contains(possibleNext)){
					continue;
				}
				float tentativeGScore = n.G + EuclideanPathfindingWeight (n, possibleNext);
				
				bool tentativeIsBetter;
				bool enqueue = false;
				if(!openSet.Contains(possibleNext)){
					enqueue = true;
					tentativeIsBetter = true;
				}
				else if(tentativeGScore < possibleNext.G){
					tentativeIsBetter = true;
				}
				else{
					tentativeIsBetter = false;
				}
				if(tentativeIsBetter){
					possibleNext.Prev = n;
					possibleNext.G = tentativeGScore;
					possibleNext.H = EuclideanPathfindingWeight(n, possibleNext);						
					possibleNext.F = possibleNext.G + possibleNext.H;
				}
				if(enqueue){
					openSet.Enqueue(possibleNext);
				}
			}
		}
		//Failure
		return new List<GridSquare>();
	}
	
	private static List<GridSquare> ReconstructPath(GridSquare node){
		if(node.Prev != null){
			List<GridSquare> path = ReconstructPath(node.Prev);
			path.Add(node);
			return path;
		}
		else{
			List<GridSquare> path = new List<GridSquare>();
			path.Add(node);
			return path;
		}
	}
	
	//This is the default pathfinder weighting: Use basic edge weight
	public static float EuclideanPathfindingWeight(GridSquare a, GridSquare b){
		return Vector3.Distance (a.transform.position, b.transform.position);
	}
	
	public static void SmoothPath(List<Vector3> path){
		int i = 0, j = 2;
		while(j < path.Count-1){
			//If can move from i to j without difficulty
			if(!Utility.RaisedSphereCast(path[i], path[j], 0.2f, LayerSets.terrain)){
				path.RemoveAt(i+1); //We do not need the intermediary node
			}
			else{
				i++;
				j++;
			}
		}
	}
}
