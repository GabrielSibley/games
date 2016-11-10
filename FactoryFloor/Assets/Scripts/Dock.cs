using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum DockType {In, Out};
public enum DockEffect { Default, First, Last };

public abstract class Dock {

	public DockType Type { get; private set; }
	public DockEffect Effect;
    public object Source { get; private set; }
	public Pipe Pipe
    {
        get; private set;
    }

	public System.Action<Dock, Grabber> OnGrabberDocked;
	public System.Action<Dock, Grabber> OnGrabberUndocked;

	public List<Grabber> DockedGrabbers { get { return dockedGrabbers; } }
	private List<Grabber> dockedGrabbers = new List<Grabber>();

    //Position in floorspace
	public abstract Vector2 FloorPosition{
		get;
	}

    public bool PausePipe
    {
        get { return false; }
    }

	public Dock(object source, DockType t)
	{
        Source = source;
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

    public void Connect(Pipe pipe)
    {
        if(Pipe != null)
        {
            Debug.LogWarning("Tried to connect pipe to already connected port");
            return;
        }
        Pipe = pipe;
    }

    public void Disconnect(Pipe pipe)
    {
        if(Pipe != pipe)
        {
            Debug.LogWarning("Tried to disconnect unconnected pipe");
            return;
        }
        Pipe = null;
    }
}
