using UnityEngine;
using System.Collections;
using CylinderState = Pistol.CylinderState;

public class PistolUI : MonoBehaviour {

	public GameObject cylinder;
	public Vector3 closedOffset, openOffset;
	public GameObject frame;
	public GameObject[] bullets;

	private Color liveBulletColor, deadBulletColor;

	public void SetColors(PlayerUI colorSource){
		cylinder.GetComponent<Renderer>().material.color = colorSource.BrightPlayerColor;
		frame.GetComponent<Renderer>().material.color = colorSource.DarkPlayerColor;
		liveBulletColor = colorSource.LiveBulletColor;
		deadBulletColor = colorSource.DeadBulletColor;
	}

	public void Display(CylinderState state, BulletState[] chambers, int currentChamber){
		cylinder.transform.localPosition = state == CylinderState.Closed ? closedOffset : openOffset;
		
		for(int i = 0; i < bullets.Length; i++){
			var chamberState = chambers[(currentChamber + i) % chambers.Length];
			if(chamberState == BulletState.Empty){
				bullets[i].SetActive(false);
			}
			else{
				bullets[i].SetActive(true);
				bullets[i].GetComponent<Renderer>().material.color = chamberState == BulletState.Fired ? deadBulletColor : liveBulletColor;
			}
		}
	}
}
