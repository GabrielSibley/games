using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MachineMiniLayoutDisplay : MonoBehaviour {

	private Queue<GameObject> squaresPool = new Queue<GameObject>();

	private List<GameObject> squares = new List<GameObject>();

	public GameObject SquarePrefab;
	public Vector2 LayoutSpacing;

	public void Display(Machine machine)
	{
		AllocSquares(machine);
		for(int i = 0; i < machine.Parts.Count; i++)
		{
			squares[i].transform.localPosition = new Vector2(LayoutSpacing.x * machine.Parts[i].Offset.x, LayoutSpacing.y * machine.Parts[i].Offset.y);
		}
	}

	private void AllocSquares(Machine machine)
	{
		int squaresNeeded = machine.Parts.Count - squares.Count;
		//Fetch needed squares
		for(int i = 0; i < squaresNeeded; i++)
		{
			GameObject square;
			if(squaresPool.Count == 0)
			{
				square = Object.Instantiate(SquarePrefab) as GameObject;
			}
			else
			{
				square = squaresPool.Dequeue();
				square.SetActive(true);
				square.hideFlags = HideFlags.None;
			}
			square.transform.SetParent (this.transform, false);
			squares.Add (square);
		}
		//Release unneeded squares
		for(int i = 0; i < -squaresNeeded; i++)
		{
			GameObject square = squares[squares.Count - 1 - i];
			squaresPool.Enqueue(square);
			square.SetActive(false);
			square.hideFlags = HideFlags.HideInHierarchy;
			square.transform.SetParent(null);
		}
		if(squaresNeeded < 0)
		{
			squares.RemoveRange (squares.Count + squaresNeeded, -squaresNeeded);
		}
	}
}
