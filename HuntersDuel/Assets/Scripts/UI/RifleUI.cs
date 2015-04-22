using UnityEngine;
using System.Collections;
using BoltState = Rifle.BoltState;

public class RifleUI : MonoBehaviour {

	public GameObject boltClosed, boltUp, boltOpen;
	public GameObject[] bullets;
	public GameObject chamberedBullet;
	public GameObject partialLoadBullet;
	public GameObject frame;

	private Color liveBulletColor, deadBulletColor;

	public void SetColors(PlayerUI colorSource){
		boltClosed.renderer.material.color = colorSource.BrightPlayerColor;
		boltUp.renderer.material.color = colorSource.BrightPlayerColor;
		boltOpen.renderer.material.color = colorSource.BrightPlayerColor;
		frame.renderer.material.color = colorSource.DarkPlayerColor;
		partialLoadBullet.renderer.material.color = colorSource.LiveBulletColor;
		liveBulletColor = colorSource.LiveBulletColor;
		deadBulletColor = colorSource.DeadBulletColor;
		for(int i = 0; i < bullets.Length; i++){
			bullets[i].renderer.material.color = colorSource.LiveBulletColor;
		}
	}

	public void Display(BoltState state, BulletState[] magazine, bool partialLoad){
		boltClosed.SetActive(state == BoltState.Closed);
		boltUp.SetActive(state == BoltState.Up);
		boltOpen.SetActive(state == BoltState.Open);
		
		for(int i = 0; i < magazine.Length; i++){
			bullets[i].SetActive(magazine[i] != BulletState.Empty);
		}
		if(state != BoltState.Open && magazine[0] != BulletState.Empty){
			bullets[0].SetActive(false);
			chamberedBullet.SetActive(true);
			chamberedBullet.renderer.material.color = magazine[0] == BulletState.Fired ? deadBulletColor : liveBulletColor;
		}
		else{
			chamberedBullet.SetActive(false);
			bullets[0].renderer.material.color = magazine[0] == BulletState.Fired ? deadBulletColor : liveBulletColor;
		}
		partialLoadBullet.SetActive(partialLoad);
	}
}
