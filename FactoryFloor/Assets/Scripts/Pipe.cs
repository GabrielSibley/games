using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pipe {

	public Port From
	{
		get
		{
			return fromPort;
		}
		set
		{
			SetPort(ref fromPort, value);
		}
	}
	private Port fromPort;

	public Port To
	{
		get
		{
			return toPort;
		}
		set
		{
			SetPort(ref toPort, value);
		}
	}
	private Port toPort;
	
	//Pipes don't move things unless both ports are real
	public bool Paused
	{
		get
		{
			return From == null || !From.IsReal || To == null || !To.IsReal;
		}
	}

	public float Length
	{
		get
		{
			if(From == null || To == null)
			{
				return 0;
			}
			return Vector2.Distance (From.WorldPosition, To.WorldPosition);
		}
	}

	public int GrabberCount { get { return grabbers.Count; } } 

	private List<Grabber> grabbers = new List<Grabber>();
	private PipeDisplay display;

	public void UpdateDisplay()
	{
		if(display == null)
		{
			display = Object.Instantiate(PrefabManager.PipeDisplay) as PipeDisplay;
		}
		display.Display(this);
		foreach(Grabber g in grabbers)
		{
			g.UpdateDisplay();
		}
	}

	public void Destroy()
	{
		if(display != null)
		{
			GameObject.Destroy(display.gameObject);
		}
		From = null;
		To = null;
		foreach(Grabber g in grabbers)
		{
			g.Destroy();
		}
	}

	public void AddGrabber()
	{
		Grabber newGrabber = new Grabber();
		newGrabber.Pipe = this;
		grabbers.Add (newGrabber);
		UpdateDisplay();
	}

	public Vector3 GetNormalizedPoint(float u)
	{
		if(From == null || To == null)
		{
			Debug.LogError("Tried to get normalized point on detached pipe");
			return Vector3.zero;
		}
		return Vector3.Lerp (From.WorldPosition, To.WorldPosition, u);
	}

	private void SetPort(ref Port myPortField, Port targetPort)
	{
		List<Grabber> dockedGrabbersToTransfer = null;
		if(myPortField != null)
		{
			myPortField.Pipe = null;
			dockedGrabbersToTransfer = new List<Grabber>(myPortField.DockedGrabbers);
			foreach(Grabber g in dockedGrabbersToTransfer)
			{
				g.Undock(myPortField);
			}
		}
		myPortField = targetPort;
		if(targetPort != null)
		{
			if(targetPort.Pipe != null)
			{
				Debug.LogError("Target port already occupied");
			}
			targetPort.Pipe = this;
			if(dockedGrabbersToTransfer != null)
			{
				foreach(Grabber g in dockedGrabbersToTransfer)
				{
					g.Dock (targetPort);
				}
			}
		}
	}
}
