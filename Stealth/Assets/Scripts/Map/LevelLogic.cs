using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelLogic : MonoBehaviour {
	
	public static GridSquare[,] grid;
	public static float nodeSize; //Size of each grid square
	public static List<Guard> guards;
	public static float timeSincePlayerSighted;
	public static AlertMode alert;
	public static int guardsSeeingPlayer;
	public static GridSquare[] largestContinuousSet;
	public static List<GridSquare> hidingSpots;
	public static List<Entity> guardSpawns;
	
	public static Dictionary<Guard, HashSet<GridSquare>> dibs;
	private static bool searchSquareAssignedThisFrame;
	
	public delegate bool FloodProcess(GridSquare current, GridSquare next);
	
	public float guardSpawnTime;
	public Guard guardPrefab;
	
	
	private float bakeSightTimer;
	private float guardSpawnTimer;

	public static GridSquare GetClosestSquare(Vector3 position){
		float minDist = float.MaxValue;
		GridSquare minTile = null;
		foreach(GridSquare t in grid){
			float d = (t.transform.position - position).sqrMagnitude;
			if(d < minDist){
				minTile = t;
				minDist = d;
			}
		}
		return minTile;
	}

	public static FloodProcess IncreaseWithDistance(FloodKey key){
		return delegate(GridSquare current, GridSquare next){
			float cost = Vector3.Distance (current.transform.position, next.transform.position);
			if(current.floodValues[key] + cost < next.floodValues[key]){
				next.floodValues[key] = current.floodValues[key] + cost;
				return true;
			}
			return false;
		};
	}
	
	public static FloodProcess DecreaseWithDistance(FloodKey key){
		return delegate(GridSquare current, GridSquare next){
			float cost = Vector3.Distance (current.transform.position, next.transform.position);
			if(current.floodValues[key] - cost > next.floodValues[key]){
				next.floodValues[key] = current.floodValues[key] - cost;
				return true;
			}
			return false;
		};
	}

	void Awake(){
		dibs = new Dictionary<Guard, HashSet<GridSquare>>();
		guards = new List<Guard>();
		guardSpawns = new List<Entity>();
		timeSincePlayerSighted = 0;
		alert = AlertMode.None;
		guardsSeeingPlayer = 0;
		GetComponent<LevelInitializer>().BuildLevel();
		hidingSpots = new List<GridSquare>(largestContinuousSet);
		ClearFillAll(0, FloodKey.Suspicion);
	}	
	
	public void BakeGuardLoS(){
		float[] blurred = new float[largestContinuousSet.Length];
		int i = 0;
		foreach(GridSquare sq in largestContinuousSet){
			float lowestNeighbourValue = 1;
			foreach(GridSquare n in sq.neighbours){
				lowestNeighbourValue = Mathf.Min(lowestNeighbourValue, n.bakedSight);
			}
			blurred[i] = lowestNeighbourValue * 0.048f + sq.bakedSight * 0.95f;
			i++;
		}
		i = 0;
		foreach(GridSquare sq in largestContinuousSet){
			if(sq.bakedSight > blurred[i]){
				sq.bakedSight = blurred[i];
			}
			i++;
		}
		foreach(Guard g in guards){
			GridSquare guardLocation = g.Location;
			foreach(GridSquare sq in guardLocation.visibleSquares){
				if(sq.bakedSight < 1 && g.InVisionArc(sq)){
					sq.bakedSight = 1;
				}
			}
		}

		foreach(GridSquare sq in largestContinuousSet){
			sq.floodValues[FloodKey.Suspicion] *= (0.99f - 0.7f * Mathf.Clamp01(sq.bakedSight - 0.5f));
		}
	}
	
	public static GridSquare ClosestGridSquareTo(Vector3 position){
		try{
			return grid[Mathf.RoundToInt(position.x / nodeSize), Mathf.RoundToInt(position.z / nodeSize)];
		}
		catch{
			return null;
		}
	}
	
	public static GridSquare ClosestGridSquareTo(Entity e){
		return ClosestGridSquareTo(e.transform.position);
	}
	
	public static void EmitSound(GridSquare source, float loudness){
		FloodKey key = FloodKey.Noise;
		ClearFillAll(0, key);
		FloodFillAll(source, loudness, delegate(GridSquare current, GridSquare next){
			float cost = Vector3.Distance (current.transform.position, next.transform.position) * ((!current.Passable && next.Passable) || current.neighbours.Contains(next) ? 1 : 4);
			if(current.floodValues[key] > next.floodValues[key] + cost){
				next.floodValues[key] = current.floodValues[key] - cost;
				return true;
			}
			return false;
		}, key);
		bool heard = false;
		foreach(Guard g in guards){
			if(g.Location.floodValues[key] > 0){
				g.suspicion += 0.5f;
				heard = true;
			}
		}
		if(heard){
			FloodFill(source, loudness, DecreaseWithDistance(FloodKey.Suspicion), FloodKey.Suspicion);
		}
	}
	
	public static void ClearDibs(){
		foreach(GridSquare sq in largestContinuousSet){
			sq.dibsGuard = null;
			sq.dibsValue = 0;
		}
		dibs.Clear();
	}
	
	public static void PlayerBrokeLoS(){
		if(Player.m_instance != null){
			ClearFill(0, FloodKey.Suspicion);
			FloodFill(Player.m_instance.Location, 100, DecreaseWithDistance(FloodKey.Suspicion), FloodKey.Suspicion);
		}
	}
	
	public static void ClearFill(float baseValue, FloodKey key){
		foreach(GridSquare sq in largestContinuousSet){
			sq.floodValues[key] = baseValue;
		}
	}
	
	public static void FloodFill(GridSquare source, float sourceValue, FloodProcess processor, FloodKey key){
		List<GridSquare> openSet = new List<GridSquare>();
		openSet.Add(source);
		source.floodValues[key] = sourceValue;
		while(openSet.Count > 0){
			GridSquare curr = openSet[0];
			openSet.RemoveAt(0);
			foreach(GridSquare sq in curr.neighbours){
				if(processor(curr, sq))
					openSet.Add(sq);
			}
		}
	}
	
	public static void ClearFillAll(float baseValue, FloodKey key){
		foreach(GridSquare sq in grid){
			sq.floodValues[key] = baseValue;
		}
	}
	
	public static void FloodFillAll(GridSquare source, float sourceValue, FloodProcess processor, FloodKey key){
		List<GridSquare> openSet = new List<GridSquare>();
		openSet.Add(source);
		source.floodValues[key] = sourceValue;
		while(openSet.Count > 0){
			GridSquare curr = openSet[0];
			openSet.RemoveAt(0);
			foreach(GridSquare sq in curr.allNeighbours){
				if(processor(curr, sq))
					openSet.Add(sq);
			}
		}
	}
	
	public static GridSquare AssignSearchSquare(Guard g){
		if(hidingSpots == null || hidingSpots.Count == 0 || searchSquareAssignedThisFrame){
			if(!searchSquareAssignedThisFrame){
				Debug.Log("Search square assignment failed");
			}
			return null;
		}
		searchSquareAssignedThisFrame = true;
		ClearFill(0, FloodKey.GuardDistance);
		FloodFill(g.Location, 0, IncreaseWithDistance(FloodKey.GuardDistance), FloodKey.GuardDistance);
		float fastWeight = Mathf.Clamp01(timeSincePlayerSighted/10);
		float mediumWeight = Mathf.Clamp01(timeSincePlayerSighted/30);
		float slowWeight = Mathf.Clamp01(timeSincePlayerSighted/60);
		foreach(GridSquare sq in hidingSpots){
			float suspicionScore = sq.floodValues[FloodKey.Suspicion];
			float distanceScore = sq.floodValues[FloodKey.GuardDistance] + (sq.dibsGuard == g ? -sq.dibsValue : sq.dibsValue);
			float recentSightScore = sq.bakedSight * 35;
			sq.searchScore = suspicionScore - distanceScore - recentSightScore; //High search score --> search first
		}
		hidingSpots.Sort(delegate(GridSquare x, GridSquare y){
			if(x.searchScore > y.searchScore)
				return -1;
			if(x.searchScore < y.searchScore)
				return 1;
			return 0;
		});
		DibsSquare(hidingSpots[0], g);
		return hidingSpots[0];
	}
	
	public static void DibsSquare(GridSquare sq, Guard g){
		//Release existing dibs
		if(dibs.ContainsKey(g)){
			foreach(GridSquare dsq in dibs[g]){
				dsq.dibsGuard = null;
				dsq.dibsValue = 0;
			}
		}
		dibs[g] = new HashSet<GridSquare>();
		List<GridSquare> openSet = new List<GridSquare>();
		sq.dibsValue = 6;
		if(sq.dibsGuard != null)
			dibs[sq.dibsGuard].Remove(sq);
		dibs[g].Add(sq);
		sq.dibsGuard = g;
		openSet.Add(sq);
		while(openSet.Count > 0){
			GridSquare curr = openSet[0];
			openSet.RemoveAt(0);
			foreach(GridSquare n in curr.neighbours){
				float cost = 1;
				if(curr.dibsValue - cost > n.dibsValue){
					if(n.dibsGuard != null){
						n.dibsGuard = g;
						dibs[n.dibsGuard].Remove(n);
					}
					dibs[g].Add(n);
					n.dibsValue = curr.dibsValue - cost;
					openSet.Add(n);
				}
			}
		}
	}
	
	public static void SetAlertMode(AlertMode newMode){
		if(newMode == AlertMode.Alert && alert != AlertMode.Alert){
			ClearDibs();
			timeSincePlayerSighted = 0;
		}
		alert = newMode;
	}
	
	// Update is called once per frame
	void Update () {
		searchSquareAssignedThisFrame = false;
		bakeSightTimer -= Time.deltaTime;
		timeSincePlayerSighted += Time.deltaTime;
		
		if(guardsSeeingPlayer > 0){
			SetAlertMode(AlertMode.Alert);
		}
		else if(alert == AlertMode.Alert){
			SetAlertMode(AlertMode.Evasion);
		}
		else if(alert == AlertMode.Evasion){
			if(timeSincePlayerSighted > 60){
				/*
				if(guardsSuspicious > 0)
					SetAlertMode(AlertMode.Warning);
				else
				*/
					SetAlertMode(AlertMode.Caution);
			}
		}
		
		if(alert == AlertMode.Alert || alert == AlertMode.Evasion){
			guardSpawnTimer -= Time.deltaTime;
			if(guardSpawnTimer <= 0){
				guardSpawnTimer = guardSpawnTime;
				GridSquare loc = Randomx.Element(guardSpawns).Location;
				Guard g = Object.Instantiate(guardPrefab, loc.transform.position, Quaternion.identity) as Guard;
				g.PlaceAtSquare(loc);
				guards.Add(g);
			}
		}
		
		if(bakeSightTimer <= 0){
			bakeSightTimer = 0.1f;
			BakeGuardLoS();
		}
		
		/*
		foreach(Guard g in guards){
			GridSquare sq = g.Location;
			for(int i = 0; i < GridSquare.kSentryDivisions - 1; i+= 1){
				float theta = Mathf.PI * 2 * i / GridSquare.kSentryDivisions;
				float thetaNext = Mathf.PI * 2 * (i+1) / GridSquare.kSentryDivisions;
				Vector3 pt1 = new Vector3(Mathf.Cos(theta), 0, Mathf.Sin(theta)) * (Mathf.Sqrt(Mathf.Max(sq.AdjustedSentryValues[i], 0)) / 4);
				Vector3 pt2 = new Vector3(Mathf.Cos(thetaNext), 0, Mathf.Sin(thetaNext)) * (Mathf.Sqrt(Mathf.Max(sq.AdjustedSentryValues[i+1], 0)) / 4);
				Debug.DrawLine(sq.transform.position + pt1, sq.transform.position + pt2, Color.yellow);
			}		 	
		}
		*/
	}
}

public enum AlertMode{
	None = 0,
	Caution = 1,
	Warning = 2,
	Evasion = 3,
	Alert = 4
}