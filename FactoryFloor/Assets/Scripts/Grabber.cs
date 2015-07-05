using UnityEngine;
using System.Collections;

public class Grabber: IUpdatable {

	public Pipe Pipe;
	public float MaxSpeed = 10000;
	public float MinSpeed = 30;
	public float Acceleration = 1200;
	public float NormalizedDistance;

	private bool reverse;

	private GrabberDisplay display;

	public Grabber(){
		Ticker.Instance.Add (this);
	}

	public void Update(float deltaTime)
	{
		if(Pipe != null && !Pipe.Paused)
		{
			float endpointDistance = 0.5f - Mathf.Abs (0.5f - NormalizedDistance); //better way?
			endpointDistance *= Pipe.Length;
			float speed = Mathf.Sqrt(2 * endpointDistance * Acceleration);
			speed = Mathf.Clamp (speed, MinSpeed, MaxSpeed);
			if(reverse)
			{
				speed = -speed;
			}
			NormalizedDistance = NormalizedDistance + speed * deltaTime / Pipe.Length;
			if(NormalizedDistance >= 1)
			{
				reverse = true;
				//make delivery here
				NormalizedDistance = 2 - NormalizedDistance;
			}
			if(NormalizedDistance <= 0)
			{
				reverse = false;
				//make pickup here
				NormalizedDistance = -NormalizedDistance;
			}
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

	public void Destroy()
	{
		Ticker.Instance.Remove (this);
		if(display)
		{
			GameObject.Destroy (display.gameObject);
		}
	}
}
