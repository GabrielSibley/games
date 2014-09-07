using UnityEngine;
using System.Collections;

public class Pistol : Gun {
	
	public enum CylinderState{
		Closed,
		Open,
		Dumped
	}
	
	public int ammoReserve;
	public int maxReserve;
	public GameObject bulletPrefab;
	public float bulletSpeed;
	public AudioClip gunsfx;
	
	BulletState[] chambers = new BulletState[6];
	int currentChamber;
	int loadChamber;
	CylinderState state;
	
	bool CylinderClosed { get { return state == CylinderState.Closed; } }
	
	public override int Damage { get { return 3; } }
	public override bool Fire(BasicMove player, float angle){
		if(chambers[currentChamber] == BulletState.Live && CylinderClosed){
			AudioManager.PlaySFX(gunsfx);
			chambers[currentChamber] = BulletState.Fired;
			NextChamber();
			
			GameObject bullet = Instantiate(bulletPrefab, player.transform.position, Quaternion.identity) as GameObject;
			bullet.rigidbody.velocity = Quaternion.Euler(0, 0, angle) * new Vector3(bulletSpeed, 0, 0);
			Physics.IgnoreCollision(bullet.collider, player.collider);
			
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
		if(state == CylinderState.Open && ammoReserve > 0){
			for(int i = currentChamber; ; NextChamber()){
				if(chambers[i] == BulletState.Empty){
					chambers[i] = BulletState.Live;
					ammoReserve--;
					break;
				}
				if(i == (currentChamber + chambers.Length - 1) % chambers.Length){
					break;
				}
			}
		}
		else if(state == CylinderState.Dumped){
			state = CylinderState.Open;
		}
	}
	public override void ManipulateDown(){
		if(state == CylinderState.Open && System.Array.Exists(chambers, x => x == BulletState.Fired)){
			state = CylinderState.Dumped;
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
	
	void Start(){
		for(int i = 0; i < chambers.Length; i++){
			chambers[i] = BulletState.Live;
		}
	}
}
