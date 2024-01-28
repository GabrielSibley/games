using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[PrefabManager]
public class PipeDisplay : MonoBehaviour {

	public const float PipeSize = 16;
    public const float TextureAR = 5;
    public Pipe Pipe;

    public GrabberDisplay GrabberDisplay;

    public IPipeDisplayAnchor From { get { return FromOverride != null ? FromOverride : SimulationView.Instance.pipeEditor.GetAnchorForDock(Pipe.From); } }
    public IPipeDisplayAnchor To { get { return ToOverride != null ? ToOverride : SimulationView.Instance.pipeEditor.GetAnchorForDock(Pipe.To); } }

    public IPipeDisplayAnchor FromOverride;
    public IPipeDisplayAnchor ToOverride;

    private List<Vector3> pathPoints = new List<Vector3>();

	public void Display()
	{
        Vector2 fromWorld = From.WorldPos;
        Vector2 toWorld = To.WorldPos;
		Vector2 midpoint = (fromWorld + toWorld) / 2;
        bool appearHigh = (Pipe.From != null && SimulationView.Instance.CarriedMachine == Pipe.From.Source) 
                        || (Pipe.To != null && SimulationView.Instance.CarriedMachine == Pipe.To.Source);
        transform.position = new Vector3(midpoint.x, midpoint.y, appearHigh ? ZLayer.Pipe + ZLayer.CarriedOffset : ZLayer.Pipe);
		transform.rotation = Quaternion.Euler (0, 0, Mathf.Rad2Deg * Mathf.Atan2 ((toWorld - midpoint).y, (toWorld - midpoint).x));
		float repeats = Vector2.Distance(fromWorld, toWorld) / PipeSize;                
		transform.localScale = new Vector3(repeats * PipeSize, PipeSize, 1);
		GetComponent<Renderer>().material.mainTextureScale = new Vector2(repeats / TextureAR, 1);

		if(Pipe.Paused)
		{
			GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.3f);
		}
		else
		{
			GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.3f);
		}

        if (Pipe.GrabberCount > 0)
        {
            if (GrabberDisplay == null)
            {
                GrabberDisplay = Instantiate(PrefabManager.GrabberDisplay);
            }        
            GrabberDisplay.Grabber = Pipe.grabbers[0];
            GrabberDisplay.PipeDisplay = this;
            GrabberDisplay.Display();
        }
	}

    public Vector3 GetNormalizedPoint(float u)
    {
        return Vector3.Lerp(From.WorldPos, To.WorldPos, u);
    }

    private void Update()
	{
		Vector2 offset = GetComponent<Renderer>().material.mainTextureOffset;
		offset.x -= Time.deltaTime;
		GetComponent<Renderer>().material.mainTextureOffset = offset;
        Display();
	}

    void OnDestroy()
    {
        if(GrabberDisplay != null)
        {
            Destroy(GrabberDisplay.gameObject);
        }
    }
}
