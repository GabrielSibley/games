using UnityEngine;
using System.Collections;

public class BounceText : MonoBehaviour {
	
	//Texas Overdraw Massacre
	public GameObject original;
	public int numDuplicates = 30;
	public int min;
	public float spacing = 0.01f;
	public float frequency = 1;
	public Color finalColor;
	public Color otherColor;
	
	private GameObject[] duplicates;
	
	// Use this for initialization
	void Start () {
		duplicates = new GameObject[numDuplicates+1];
		duplicates[0] = original;
		for(int i = 1; i < duplicates.Length; i++)
		{
			duplicates[i] = Instantiate(original,
				original.transform.position - original.transform.forward * spacing * i,
				original.transform.rotation) as GameObject;
		}
		for(int i = 0; i < duplicates.Length; i++){
			duplicates[i].transform.parent = transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		int threshold = (int)(Mathf.PingPong(Time.time, frequency) / frequency * (duplicates.Length - min)) + min;
		for(int i = 0; i < duplicates.Length; i++){
			if(threshold >= i){
				duplicates[i].SetActive(true);
				if(i == threshold){
					duplicates[i].GetComponent<Renderer>().material.color = finalColor;
				}
				else{
					duplicates[i].GetComponent<Renderer>().material.color = otherColor;
				}
			}
			else{
				duplicates[i].SetActive(false);
			}
		}
	}
}
