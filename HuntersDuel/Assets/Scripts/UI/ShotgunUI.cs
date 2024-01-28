using UnityEngine;
using System.Collections;
using SlideState = Shotgun.SlideState;

public class ShotgunUI : MonoBehaviour {

	public GameObject slideBack, slideForward;
	public GameObject[] shells;
	public GameObject chamberedBack, chamberedForward;
	public GameObject partialLoadShell;
	public GameObject frame;

	private Color liveBulletColor, deadBulletColor;

	public void SetColors(PlayerUI colorSource){
		frame.GetComponent<Renderer>().material.color = colorSource.DarkPlayerColor;
		slideBack.GetComponent<Renderer>().material.color = colorSource.BrightPlayerColor;
		slideForward.GetComponent<Renderer>().material.color = colorSource.BrightPlayerColor;
		liveBulletColor = colorSource.LiveBulletColor;
		deadBulletColor = colorSource.DeadBulletColor;
		for(int i = 0; i < shells.Length; i++){
			shells[i].GetComponent<Renderer>().material.color = colorSource.LiveBulletColor;
		}
		partialLoadShell.GetComponent<Renderer>().material.color = colorSource.LiveBulletColor;
	}

	public void Display(SlideState state, BulletState[] magazine, BulletState chamber, bool partialLoad){
		slideForward.SetActive(state == SlideState.Forward);
		slideBack.SetActive(state == SlideState.Back);
		
		for(int i = 0; i < shells.Length; i++){
			shells[i].SetActive(magazine[i] != BulletState.Empty);
		}
		if(chamber == BulletState.Empty || state == SlideState.Back){
			chamberedForward.SetActive (false);
		}
		else{
			chamberedForward.SetActive (true);
			chamberedForward.GetComponent<Renderer>().material.color = chamber == BulletState.Fired ? deadBulletColor : liveBulletColor;
		}
		if(chamber == BulletState.Empty || state == SlideState.Forward){
			chamberedBack.SetActive(false);
		}
		else{
			chamberedBack.SetActive (true);
			chamberedBack.GetComponent<Renderer>().material.color = chamber == BulletState.Fired ? deadBulletColor : liveBulletColor;
		}
		partialLoadShell.SetActive(partialLoad);
	}

}
