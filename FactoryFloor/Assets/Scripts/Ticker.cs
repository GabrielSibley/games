using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Ticks things that cannot tick themselves
public class Ticker : MonoBehaviour {

	public static Ticker Instance {get; private set;}

	private List<IUpdatable> updatables = new List<IUpdatable>();
	private int updateIteratorIndex;

	private void Awake()
	{
		Instance = this;
	}

	public void Add(IUpdatable u)
	{
		updatables.Add (u);
	}

	public void Remove(IUpdatable u)
	{
		int index = updatables.IndexOf(u);
		//If we remove an updatable we've already passed this frame,
		//adjust our iterator to account for the removed item
		if(index <= updateIteratorIndex)
		{
			updateIteratorIndex--;
		}
		updatables.RemoveAt (index);
	}

	// Update is called once per frame
	void Update () {
		if(Machine.MachineOnCursor != null){
			Machine.MachineOnCursor.DisplayAt(InputManager.InputWorldPos);
		}

		for(updateIteratorIndex = 0; updateIteratorIndex < updatables.Count; updateIteratorIndex++)
		{
			updatables[updateIteratorIndex].Update(Time.deltaTime);
		}
	}
}
