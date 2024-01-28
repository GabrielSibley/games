using UnityEngine;
using System.Collections;

public class Rifle : Gun {
	
	public enum BoltState {
		Closed,
		Up,
		Open,
	}

	public RifleUI ui;
	
	public GameObject m_bulletPrefab;
	public float m_bulletSpeed;
	
	public AudioClip m_gunsfx;
    public AudioClip loadBulletSfx;
	public float walkSpeed;
	
	//public int m_ammoReserve = 5;
	//public int m_reserveMax = 15;
	
	protected BoltState m_boltState = BoltState.Closed;
	protected BulletState[] m_magazine = new BulletState[5];
	protected bool m_partialReload;
	
	public override int Damage { get { return 6; } }
	public override float WalkSpeed { get { return walkSpeed; } }
	public override GunType GunType { get { return GunType.Rifle; } }
	bool MagazineFull{get{return m_magazine[m_magazine.Length-1] != BulletState.Empty;}}
	
	public override void ManipulateUp(){
		if(m_boltState == BoltState.Closed){
			m_boltState = BoltState.Up;
		}
		else if(m_boltState == BoltState.Open && !MagazineFull /*&& m_ammoReserve > 0*/){
			m_partialReload = true;
		}
	}
	public override void ManipulateDown(){
		if(m_boltState == BoltState.Up){
			m_boltState = BoltState.Closed;
		}
		else if(m_boltState == BoltState.Open && m_partialReload)
		{
			//m_ammoReserve--;
			m_partialReload = false;
			AddLiveRound();
		}
	}
	public override void ManipulateLeft(){
		if(m_boltState == BoltState.Up){
			m_boltState = BoltState.Open;
			RemoveTopRound();
		}
	}
	public override void ManipulateRight(){
		if(m_boltState == BoltState.Open){
			m_boltState = BoltState.Up;
			m_partialReload = false;
		}
	}
	
	public override bool Fire(BasicMove player, float angle){
		if(m_magazine[0] == BulletState.Live && m_boltState == BoltState.Closed){
			AudioManager.PlaySFX(m_gunsfx);
			m_magazine[0] = BulletState.Fired;

			SpawnBullet(m_bulletPrefab, m_bulletSpeed, player, angle);

			return true;
		}
		return false;
	}
	
	void OnEnable(){
		ui.gameObject.SetActive (true);
		m_boltState = BoltState.Closed;
		m_magazine = new BulletState[5];
		m_partialReload = false;
		for(int i = 0; i < m_magazine.Length; i++){
            m_magazine[i] = BulletState.Live;
		}
		Update();
	}

	void OnDisable(){
		if(ui){
			ui.gameObject.SetActive (false);
		}
	}
	
	void AddLiveRound(){
        AudioManager.PlaySFX(loadBulletSfx);
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
		ui.Display(m_boltState, m_magazine, m_partialReload);
	}
}
