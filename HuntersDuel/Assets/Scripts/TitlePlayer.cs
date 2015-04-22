using UnityEngine;
using System.Collections;

public class TitlePlayer : MonoBehaviour {

	public bool IsReady;
	public int player;
	public SpriteRenderer readyIndicator;
	public SpriteRenderer howToReady;

	public Gun[] tutorialGuns;
	public TutorialInputSim tutorial;

	private int gunIndex;

	void Start(){
		tutorial.ActiveGun = tutorialGuns[0];
	}

	// Update is called once per frame
	void Update () {
		if(Winnput.A[player] && Winnput.B[player]){
			IsReady = true;
			readyIndicator.gameObject.SetActive(true);
			howToReady.gameObject.SetActive (false);
		}
		if(!IsReady){
			if(Winnput.LeftDown[player]){
				gunIndex = (gunIndex + tutorialGuns.Length + 1) % tutorialGuns.Length;
				tutorial.ActiveGun = tutorialGuns[gunIndex];
			}
			if(Winnput.RightDown[player]){
				gunIndex = (gunIndex + 1) % tutorialGuns.Length;
				tutorial.ActiveGun = tutorialGuns[gunIndex];
			}
		}
	}
}
