using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/* This class contains pathfinding information for a grid square*/
public class GridSquare : MonoBehaviour, System.IComparable {
	
	public const int kSentryDivisions = 32;

	//Pathfinding vars
	public float F {get; set;}
	public float G {get; set;}
	public float H {get; set;}
	public GridSquare Prev{get; set;}

	//level construction var
	public int setSize;

	//union find vars
	public int rank;
	public GridSquare parent;

	public int CompareTo(object obj){
		GridSquare that = obj as GridSquare;
		if(this.F < that.F){
			return -1;
		}
		if(this.F > that.F){
			return 1;
		}
		return 0;
	}

	public int x{
		get{
			return m_position.x;
		}
		set{
			m_position.x = value;
		}
	}
	public int y{
		get{
			return m_position.y;
		}
		set{
			m_position.y = value;
		}
	}
	public Vector2i Position{
		get{ return m_position;	}
		set{ m_position = value; }
	}
	private Vector2i m_position;

	public bool Passable{
		get{
			return m_passable;
		}
		set{m_passable = value;}
	}
	[SerializeField] protected bool m_passable;

	public List<GridSquare> neighbours;
	public List<GridSquare> allNeighbours;
	public List<GridSquare> visibleSquares; //Squares visible from this one
	public Color baseColor;
	public Color highlightColor;
	
	//Hide and seek vars
	//Number of squares visible given a vision arc centered on one of 32 directions
	public int[] SentryValues{
		get{return sentryValues;}
		set{sentryValues = value;}
	}
	//Adjusted for how good nearby directions are
	public int[] AdjustedSentryValues{
		get{return adjSentryValues;}
		set{adjSentryValues = value;}
	}
	//Goodness as a sentry post from a gameplay standpoint
	public float SentryExcellence{
		get; set;
	}
	protected int[] sentryValues = new int[kSentryDivisions]; 
	protected int[] adjSentryValues = new int[kSentryDivisions];
	public float bakedSight;
	public Dictionary<FloodKey, float> floodValues = new Dictionary<FloodKey, float>();
	
	public Guard dibsGuard;
	public float dibsValue;
	public float searchScore;
	
}

public enum FloodKey{
	General,
	GuardDistance,
	Suspicion,
	Noise
}
