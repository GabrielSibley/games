using UnityEngine;
using System.Collections;

public class Grabber: IUpdatable {

	public Pipe Pipe;
	public float MinSpeed = 0.01f;
	public float Acceleration = 1200;
	public float NormalizedDistance;
	public Crate HeldCrate;

	public bool Docked { get { return dockedToPort != null; } }
	public Port DockedToPort{ get { return dockedToPort; } }

	private Port dockedToPort;
	private bool reverse = true;
	private GrabberDisplay display;
	private float normalizedMovementAllowance;

	public Grabber(){
		Ticker.Instance.Add (this);
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
		UpdateDisplay();
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
		UpdateDisplay();
	}

	public void UpdateDisplay()
	{
		if(!display)
		{
			display = Object.Instantiate(PrefabManager.GrabberDisplay) as GrabberDisplay;
		}
		display.Display(this);
	}

	public void Dock(Port port)
	{
		dockedToPort = port;
		if(port == Pipe.To)
		{
			NormalizedDistance = 1;
		}
		else if(port == Pipe.From)
		{
			NormalizedDistance = 0;
		}
		else
		{
			Debug.LogError ("Grabber docked to port not on its pipe");
		}
		port.DockGrabber(this);
	}

	public void Undock(Port port)
	{
		if(dockedToPort != port)
		{
			Debug.LogError ("Undock from mismatched port");
		}
		dockedToPort = null;
		port.UndockGrabber(this);
	}
	
	//Undocks, sets crate, and triggers movement update
	public void Dispatch(Crate crate, Port port)
	{
		HeldCrate = crate;
		Undock(port);
		reverse = port == Pipe.To;
		Move();
	}

	public void Destroy()
	{
		Ticker.Instance.Remove (this);
		if(display)
		{
			GameObject.Destroy (display.gameObject);
		}
	}
}
