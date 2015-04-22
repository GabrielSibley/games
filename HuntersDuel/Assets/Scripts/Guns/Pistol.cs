using UnityEngine;
using System.Collections;

public class Pistol : Gun {
	
	public enum CylinderState{
		Closed,
		Open
	}

	public PistolUI ui;

	//public int ammoReserve;
	//public int maxReserve;
	public GameObject bulletPrefab;
	public float bulletSpeed;
	public AudioClip gunsfx;
	public float walkSpeed;
	
	BulletState[] chambers = new BulletState[6];
	int currentChamber;
	int loadChamber;
	CylinderState state;
	
	bool CylinderClosed { get { return state == CylinderState.Closed; } }
	
	public override int Damage { get { return 3; } }
	public override float WalkSpeed { get { return walkSpeed; } }
	public override GunType GunType { get { return GunType.Pistol; } }
	public override bool Fire(BasicMove player, float angle){
		if(state != CylinderState.Closed){
			return false;
		}
		if(chambers[currentChamber] == BulletState.Live){
			AudioManager.PlaySFX(gunsfx);
			chambers[currentChamber] = BulletState.Fired;
			NextChamber();

			SpawnBullet(bulletPrefab, bulletSpeed, player, angle);
			
			return true;
		}
		else{
			NextChamber();
			return false;
		}
	}
	
	void NextChamber(){
		currentChamber = (currentChamber + 1) % chambers.Length;
	}

	public override void ManipulateUp(){
		//Load bullet
		if(state == CylinderState.Open /*&& ammoReserve > 0*/){
			for(int i = 0; i < chambers.Length; i++){
				int chamberIndex = (currentChamber + i)%chambers.Length;
				if(chambers[chamberIndex] == BulletState.Empty){
					chambers[chamberIndex] = BulletState.Live;
					//ammoReserve--;
					break;
				}
			}
		}
	}
	public override void ManipulateDown(){
		if(state == CylinderState.Open){
			for(int i = 0; i < chambers.Length; i++){
				chambers[i] = BulletState.Empty;
			}
			currentChamber = 0;
		}
	}
	public override void ManipulateLeft(){
		if(state == CylinderState.Closed){
			state = CylinderState.Open;
		}
	}
	public override void ManipulateRight(){
		if(state == CylinderState.Open){
			state = CylinderState.Closed;
		}
	}
	
	void OnEnable(){
		ui.gameObject.SetActive (true);
		state = CylinderState.Closed;
		for(int i = 0; i < chambers.Length; i++){
			chambers[i] = BulletState.Live;
		}
		Update();
	}

	void OnDisable(){
		if(ui){
			ui.gameObject.SetActive (false);
		}
	}

	void Update(){
		ui.Display(state, chambers, currentChamber);
	}
}
