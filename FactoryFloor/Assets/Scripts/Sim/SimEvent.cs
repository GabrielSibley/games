using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SimEvent {
	//Events which must have happened this tick for this event to occur
	private List<SimEvent> parents;
	//Events which depend on this event in order to occur
	private List<SimEvent> children;

	//Perform the event
	public abstract void Execute();
}
