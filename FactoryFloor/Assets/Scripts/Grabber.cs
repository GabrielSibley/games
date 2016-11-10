using UnityEngine;
using System.Collections;

public class Grabber: IUpdatable {

    private Simulation Simulation;

	public Pipe Pipe;
	public float MinSpeed = 0.01f;
	public float Acceleration = 16;
	public float NormalizedDistance;
	public Crate HeldCrate;

	public bool Docked { get { return DockedAt != null; } }
	public Dock DockedAt { get; set; }
	private bool reverse = true;
	private float normalizedMovementAllowance;

    public Grabber(Pipe pipe, Simulation sim)
    {
        Simulation = sim;
        Simulation.Tick(this);
        Pipe = pipe;
    }

	public void Update(float deltaTime)
	{
		if(Pipe != null && !Pipe.Paused)
		{
			float endpointDistance = 0.5f - Mathf.Abs (0.5f - NormalizedDistance); //better way?
			float speed = Mathf.Sqrt(2 * endpointDistance * Acceleration * Pipe.Length) / Pipe.Length;
			speed = Mathf.Max (speed, MinSpeed);
			normalizedMovementAllowance = speed * deltaTime;
			if(!Docked)
			{
				Move();
			}
		}
	}

	public void Move()
	{
		if(Docked)
		{
			Debug.LogError("Grabber tried to move while docked");
			return;
		}
		if(normalizedMovementAllowance <= 0)
		{
			return;
		}
		float speed = normalizedMovementAllowance;
		if(reverse)
		{
			speed = -speed;
		}
		float distMoved;
		if(NormalizedDistance + speed >= 1)
		{
			distMoved = 1 - NormalizedDistance;
			NormalizedDistance = 1;
			normalizedMovementAllowance -= distMoved;
			Dock(Pipe.To);
		}
		else if(NormalizedDistance + speed <= 0)
		{
			distMoved = NormalizedDistance;
			normalizedMovementAllowance -= distMoved;
			NormalizedDistance = 0;
			Dock(Pipe.From);
		}
		else
		{
			NormalizedDistance = NormalizedDistance + speed;
			distMoved = Mathf.Abs (speed);
			normalizedMovementAllowance -= distMoved;
		}
	}

	public void Dock(Dock dock)
	{
		DockedAt = dock;
		if(dock == Pipe.To)
		{
			NormalizedDistance = 1;
		}
		else if(dock == Pipe.From)
		{
			NormalizedDistance = 0;
		}
		else
		{
			Debug.LogError ("Grabber docked to port not on its pipe");
		}
        dock.DockGrabber(this);
	}

	public void Undock(Dock dock)
	{
		if(DockedAt != dock)
		{
			Debug.LogError ("Undock from mismatched port");
		}
        DockedAt = null;
        dock.UndockGrabber(this);
	}
	
	//Undocks, sets crate, and triggers movement update
	public void Dispatch(Crate crate, Dock port)
	{
		HeldCrate = crate;
		Undock(port);
		reverse = port == Pipe.To;
		Move();
	}

    public void Destroy()
    {
        Simulation.Untick(this);
    }
}
