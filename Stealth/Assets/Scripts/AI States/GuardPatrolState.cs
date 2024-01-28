using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardPatrolState : GuardState {

	protected List<Vector3> m_waypoints;
	protected List<Vector3> m_path;
	protected bool m_pingpong;
	protected bool m_travelReverse;
	protected int m_currentWaypoint;

	public GuardPatrolState(){
		/* lololololol
		List<Room> realRooms = LevelLogic.rooms;
		//Pick random starting room
	 	Room currentRoom = Randomx.Element(realRooms);
		List<Room> visitedRooms = new List<Room>();
		int numWaypoints = Random.Range(2, 7);
		for(int i = 0; i < numWaypoints; i++){
			//Add current room to patrol route
			visitedRooms.Add(currentRoom);
			//Find an adjacent unvisited room to visit next
			List<Room> nextRooms = currentRoom.adjacent.FindAll(delegate(Room r){return !visitedRooms.Contains(r);});
			//If none found, look for ones adjacent to a previous room
			if(nextRooms.Count == 0){
				for(int j = visitedRooms.Count - 2; j >= 0; j--){
					nextRooms.AddRange(visitedRooms[j].adjacent.FindAll(delegate(Room r){return !visitedRooms.Contains(r);}));
				}
				if(nextRooms.Count > 0){
					break;
				}
			}
			if(nextRooms.Count > 0){
				currentRoom = Randomx.Element(nextRooms);
			}
			else{
				break;				
			}
		}		
		m_waypoints = new List<Vector3>();
		for(int i = 0; i < visitedRooms.Count; i++){
			List<Tile> floors = visitedRooms[i].floors;
			Tile target = floors[Random.Range(0, floors.Count)];
			m_waypoints.Add(LevelLogic.grid[target.x, target.y].transform.position);
		}
		if(visitedRooms.Count >= 2){
			Room startRoom = visitedRooms[0];
			Room endRoom = visitedRooms[visitedRooms.Count - 1];
			m_pingpong = !(startRoom == endRoom || startRoom.adjacent.Contains(endRoom));
		}
		m_currentWaypoint = Random.Range(0, m_waypoints.Count);
		*/
	}

	public override void OnStateEntered(Guard guard){
		guard.DisplayMode = guard.normalMaterial;
	}
	
	public override void Update(Guard guard){
		if(LevelLogic.alert == AlertMode.Alert){
			guard.PushState(new GuardAgroState());
			return;
		}
		if(LevelLogic.alert == AlertMode.Evasion){
			guard.PushState(new GuardSearchState());
			return;
		}
		
		if(guard.suspicion > 0){
			guard.PushState(new GuardSearchState());
			LevelLogic.SetAlertMode(AlertMode.Warning);
			return;
		}
		
		if(m_path == null || m_path.Count == 0){
			m_path = GetPathTo(guard, AdvanceWaypoint());
		}
		MoveAlongPath(guard, m_path);
		//Look along direction of travel
		if(guard.rigidbody.velocity.magnitude > 0){
			Look(guard, guard.rigidbody.velocity);
		}
		
		for(int i = 0; i < m_waypoints.Count-1; i++){
			Debug.DrawLine(m_waypoints[i], m_waypoints[i+1], new Color(0.8f, 0.4f, 1));
		}
		if(!m_pingpong){
			Debug.DrawLine(m_waypoints[m_waypoints.Count-1], m_waypoints[0], new Color(0.8f, 0.4f, 1));
		}
	}
	
	public Vector3 PlacementPoint{
		get{return m_waypoints[m_currentWaypoint];}
	}
	
	protected Vector3 AdvanceWaypoint(){
		if(m_pingpong && ((m_currentWaypoint == 0 && m_travelReverse) || (m_currentWaypoint == m_waypoints.Count-1 && !m_travelReverse))){
			m_travelReverse = !m_travelReverse;
		}
		if(m_travelReverse){
			m_currentWaypoint = Mathfx.Repeat(m_currentWaypoint - 1, m_waypoints.Count);
		}
		else{
			m_currentWaypoint = Mathfx.Repeat(m_currentWaypoint + 1, m_waypoints.Count);
		}
		return m_waypoints[m_currentWaypoint];
	}
}
