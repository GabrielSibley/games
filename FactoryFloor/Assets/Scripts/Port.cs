using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PortType {In, Out};

public abstract class Port {

	public PortType Type;
	public Pipe Pipe; //modify this with Pipe.From and Pipe.To
	public abstract bool IsReal { get; }

	public Vector2 Offset; //Tile-space offset from machine origin
	public System.Action<Port, Grabber> OnGrabberDocked;
	public System.Action<Port, Grabber> OnGrabberUndocked;

	public List<Grabber> DockedGrabbers { get { return dockedGrabbers; } }
	private List<Grabber> dockedGrabbers = new List<Grabber>();

	public abstract Vector2 WorldPosition{
		get;
	}

	public Port(PortType t)
	{
		Type = t;
	}

	public void DockGrabber(Grabber grabber)
	{
		dockedGrabbers.Add(grabber);
		if(OnGrabberDocked != null){
			OnGrabberDocked(this, grabber);
		}
	}

	public void UndockGrabber(Grabber grabber)
	{
		dockedGrabbers.Remove(grabber);
		if(OnGrabberUndocked != null)
		{
			OnGrabberUndocked(this, grabber);
		}
	}
}
