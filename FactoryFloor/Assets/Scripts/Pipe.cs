using UnityEngine;
using System.Collections;

public class Pipe {

	public Port From;
	public Port To;

	private PipeDisplay display;

	public void SetPorts(Port from, Port to)
	{
		From = from;
		To = to;
	}

	public void UpdateDisplay()
	{
		if(display == null)
		{
			display = Object.Instantiate(PrefabManager.PipeDisplay) as PipeDisplay;
		}
		display.DisplayPointToPoint(From.WorldPosition, To.WorldPosition);
	}

	public void Destroy()
	{
		if(display != null)
		{
			GameObject.Destroy(display.gameObject);
			if(From != null)
			{
				From.Pipe = null;
			}
			if(To != null)
			{
				To.Pipe = null;
			}
		}
	}
}
