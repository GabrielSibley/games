using UnityEngine;
using System.Collections;

public class CrateFeatureDisplayManager : MonoBehaviour {

	static CrateFeatureDisplayManager instance;

	public static Sprite GetSprite(CrateFeature f)
	{
		return instance.shapes[f.Shape].colors[f.Color];
	}

	public CrateFeatureSpriteSet[] shapes;

	void Awake()
	{
		instance = this;
	}
}

[System.Serializable]
public class CrateFeatureSpriteSet
{
	public Sprite[] colors;
}
