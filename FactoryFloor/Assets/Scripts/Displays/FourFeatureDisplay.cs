using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FourFeatureDisplay : MonoBehaviour {

	public SpriteRenderer[] featureRenderers;
	
	public void Display(IList<CrateFeature> features, Vector3 position)
	{
		transform.position = position;
		for(int i = 0; i < featureRenderers.Length; i++)
		{
			if(features.Count <= i)
			{
				featureRenderers[i].enabled = false;
			}
			else
			{
				featureRenderers[i].enabled = true;
				featureRenderers[i].sprite = features[i].GetSprite();
			}
		}
	}
}
