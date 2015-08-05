using UnityEngine;
using System.Collections;

[PrefabManager]
public class GrabberDisplay : MonoBehaviour {

	public SpriteRenderer[] featureRenderers;

	public void Display(Grabber grabber)
	{
		transform.position = grabber.Pipe.GetNormalizedPoint(grabber.NormalizedDistance) - Vector3.forward * 20;
		for(int i = 0; i < featureRenderers.Length; i++)
		{
			if(grabber.HeldCrate == null || grabber.HeldCrate.Features.Count <= i)
			{
				featureRenderers[i].enabled = false;
			}
			else
			{
				featureRenderers[i].enabled = true;
				featureRenderers[i].sprite = grabber.HeldCrate.Features[i].GetSprite();
			}
		}
	}
}
