using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimContext {

	public List<SimEvent> EventsPending;
	public List<SimEvent> EventsExecutable;

	public void ProcessTick()
	{
		//Gather event triggers
		//for each event trigger
			//if trigger condition satisfied
		//????

		//flag events list dirty
		//while events list dirty or pending events > 0
			//clear events list dirty flag
			//for each event
				//if event is executable
					//move event from pending list to executable list
					//flag events list dirty

			//if events list clean or pending events == 0
				//copy pending events list to failed events
				//fail all failed events (may generate additional events)
			//???
	}
}
