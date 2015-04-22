using UnityEngine;
using System.Collections;

public class Shotgun : Gun {
	
	public enum SlideState {
		Forward,
		Back
	}

	public ShotgunUI ui;
	
	public GameObject m_bulletPrefab;
	public float m_bulletSpeed;
	
	public int m_pelletCount;
	public float m_pelletSpread; //In degrees
	
	public AudioClip m_gunsfx;
	public float walkSpeed;
	
	//public int m_ammoReserve = 5;
	//public int m_reserveMax = 10;
	
	protected SlideState m_slideState = SlideState.Forward;
	protected BulletState m_chamber;
	protected BulletState[] m_magazine = new BulletState[4];
	protected bool m_partialReload;
	
	public override int Damage { get { return 1; } }
	public override float WalkSpeed { get { return walkSpeed; } }
	public override GunType GunType { get { return GunType.Shotgun; } }
	
	bool MagazineFull{get{return m_magazine[m_magazine.Length-1] != BulletState.Empty;}}
	
	public override void ManipulateUp(){
		if(m_slideState == SlideState.Forward && !MagazineFull /*&& m_ammoReserve > 0*/){
			m_partialReload = true;
		}
	}
	public override void ManipulateDown(){
		//Doesnt do anything
	}
	public override void ManipulateLeft(){
		if(m_slideState == SlideState.Forward){
			m_partialReload = false;
			m_slideState = SlideState.Back;
			m_chamber = m_magazine[0];
			RemoveTopRound();
		}
	}
	public override void ManipulateRight(){
		if(m_partialReload){
			m_partialReload = false;
			//m_ammoReserve--;
			AddLiveRound();
		}
		if(m_slideState == SlideState.Back){
			m_slideState = SlideState.Forward;
		}
	}
	
	public override bool Fire(BasicMove player, float angle){
		if(m_chamber == BulletState.Live && m_slideState == SlideState.Forward){
			AudioManager.PlaySFX(m_gunsfx);
			m_chamber = BulletState.Fired;
			
			for(int i = 0; i < m_pelletCount; i++){
				SpawnBullet(m_bulletPrefab, m_bulletSpeed, player, angle -m_pelletSpread/2 + m_pelletSpread * i / (m_pelletCount-1));
			}
			return true;
		}
		return false;
	}
	
	void OnEnable(){
		ui.gameObject.SetActive (true);
		m_slideState = SlideState.Forward;
		m_magazine = new BulletState[4];
		for(int i = 0; i < m_magazine.Length; i++){
			m_magazine[i] = BulletState.Live;
		}
		m_chamber = BulletState.Live;
		m_partialReload = false;
		Update();
	}

	void OnDisable(){
		if(ui){
			ui.gameObject.SetActive (false);
		}
	}
	
	void AddLiveRound(){
		for(int i = m_magazine.Length-1; i > 0; i--){
			m_magazine[i] = m_magazine[i-1];
		}
		m_magazine[0] = BulletState.Live;
	}
	
	void RemoveTopRound(){
		for(int i = 0; i < m_magazine.Length-1; i++){
			m_magazine[i] = m_magazine[i+1];
		}
		m_magazine[m_magazine.Length-1] = BulletState.Empty;
	}
	
	void Update(){
		//Update UI
		ui.Display (m_slideState, m_magazine, m_chamber, m_partialReload);
	}
}
