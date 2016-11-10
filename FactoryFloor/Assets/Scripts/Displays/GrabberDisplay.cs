using UnityEngine;
using System.Collections;

[PrefabManager]
public class GrabberDisplay : MonoBehaviour {

	public SpriteRenderer[] featureRenderers;

    public Grabber Grabber;
    public PipeDisplay PipeDisplay;

	public void Display()
	{
		transform.position = PipeDisplay.GetNormalizedPoint(Grabber.NormalizedDistance) - Vector3.forward * 20;
		for(int i = 0; i < featureRenderers.Length; i++)
		{
			if(Grabber.HeldCrate == null || Grabber.HeldCrate.Features.Count <= i)
			{
				featureRenderers[i].enabled = false;
			}
			else
			{
				featureRenderers[i].enabled = true;
				featureRenderers[i].sprite = Grabber.HeldCrate.Features[i].GetSprite();
			}
		}
	}
}
