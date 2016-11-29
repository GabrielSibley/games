using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pipe {

    public Simulation Simulation;

	public Dock From
	{
		get
		{
			return fromEnd;
		}
		set
		{
			SetPort(ref fromEnd, value);
		}
	}
	private Dock fromEnd;

	public Dock To
	{
		get
		{
			return toEnd;
		}
		set
		{
			SetPort(ref toEnd, value);
		}
	}
	private Dock toEnd;
	
	//Pipes don't move things unless both ends are attached
	public bool Paused
	{
		get
		{
            return From == null || To == null;
		}
	}

    //Returns length in floor units
	public float Length
	{
		get
		{
			if(From == null || To == null)
			{
				return 0;
			}
			return Vector2.Distance (From.FloorPosition, To.FloorPosition);
		}
	}

	public int GrabberCount { get { return grabbers.Count; } } 

	public List<Grabber> grabbers = new List<Grabber>();

    public Pipe(Simulation sim)
    {
        Simulation = sim;
    }

	public void Destroy()
	{
		From = null;
		To = null;
		foreach(Grabber g in grabbers)
		{
			g.Destroy();
		}
	}

	public void AddGrabber()
	{
		Grabber newGrabber = new Grabber(this, Simulation);
		newGrabber.Pipe = this;
		grabbers.Add (newGrabber);;
	}

	private void SetPort(ref Dock currentEnd, Dock newEnd)
	{
		List<Grabber> dockedGrabbersToTransfer = null;
		if(currentEnd != null)
		{
            currentEnd.Disconnect(this);
			dockedGrabbersToTransfer = new List<Grabber>(currentEnd.DockedGrabbers);
			foreach(Grabber g in dockedGrabbersToTransfer)
			{
				g.Undock(currentEnd);
			}
		}
        currentEnd = newEnd;
		if(newEnd != null)
		{
			
            newEnd.Connect(this);
			if(dockedGrabbersToTransfer != null)
			{
				foreach(Grabber g in dockedGrabbersToTransfer)
				{
					g.Dock (newEnd);
				}
			}
		}
	}
}
