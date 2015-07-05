using UnityEngine;
using System.Collections;

public class GrabberDisplay : MonoBehaviour {

	public void Display(Grabber grabber)
	{
		transform.position = grabber.Pipe.GetNormalizedPoint(grabber.NormalizedDistance) - Vector3.forward * 20;
	}
}
