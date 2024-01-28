using UnityEngine;
using System.Collections;

public class EffectsPrefabs : MonoBehaviour {

	public static EffectsPrefabs instance;
	
	public Tracer tracerPrefab;
	public Entity bogusEntity;
	public GameObject shellCasing;
	
	void Awake(){
		instance = this;
	}
}
