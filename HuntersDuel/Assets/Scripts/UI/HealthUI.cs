using UnityEngine;
using System.Collections;

public class HealthUI : MonoBehaviour {

	public Sprite pipSprite;
	public GameObject[] pips;

	void Awake(){
		SetPipSprite(pipSprite);
	}

	public void Display(int hp){
		for(int i = 0; i < pips.Length; i++){
			pips[i].SetActive (i < hp);
		}
	}

	public void SetPipSprite(Sprite sprite){
		for(int i = 0; i < pips.Length; i++){
			pips[i].GetComponent<SpriteRenderer>().sprite = sprite;
		}
	}
}
