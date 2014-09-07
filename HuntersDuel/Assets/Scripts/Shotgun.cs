using UnityEngine;
using System.Collections;

public class Shotgun : Gun {
	
	public enum SlideState {
		Forward,
		Back
	}
	
	public GameObject[] m_boltGUI;
	public GameObject[] m_magazineGUI;
	public GameObject m_chamberGUI;
	public GameObject m_chamberBackGUI;
	public GameObject m_partialGUI;
	public GameObject m_bodyGUI;
	
	public Color m_bodyGUIColor, m_boltGUIColor, m_bulletGUIColor, m_bulletDeadGUIColor;
	
	public GameObject m_bulletPrefab;
	public float m_bulletSpeed;
	
	public int m_pelletCount;
	public float m_pelletSpread; //In degrees
	
	public AudioClip m_gunsfx;
	
	public int m_ammoReserve = 5;
	public int m_reserveMax = 10;
	
	protected SlideState m_slideState = SlideState.Forward;
	protected BulletState m_chamber;
	protected BulletState[] m_magazine = new BulletState[4];
	protected bool m_partialReload;
	
	public override int Damage { get { return 2; } }	
	
	bool MagazineFull{get{return m_magazine[m_magazine.Length-1] != BulletState.Empty;}}
	
	public override void ManipulateUp(){
		if(m_slideState == SlideState.Forward && !MagazineFull && m_ammoReserve > 0){
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
			m_ammoReserve--;
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
				GameObject bullet = Instantiate(m_bulletPrefab, player.transform.position, Quaternion.identity) as GameObject;
				bullet.rigidbody.velocity = Quaternion.Euler(0, 0, angle -m_pelletSpread/2 + m_pelletSpread * i / (m_pelletCount-1)) * new Vector3(m_bulletSpeed, 0, 0);
				Physics.IgnoreCollision(bullet.collider, player.collider);
			}
			return true;
		}
		return false;
	}
	
	void Start(){
		m_bodyGUI.renderer.material.color = m_bodyGUIColor;
		for(int i = 0; i < m_boltGUI.Length; i++){
			m_boltGUI[i].renderer.material.color = m_boltGUIColor;
		}
		for(int i = 0; i < m_magazineGUI.Length; i++){
			m_magazineGUI[i].renderer.material.color = m_bulletGUIColor;
		}

		for(int i = 0; i < m_magazine.Length; i++){
			m_magazine[i] = BulletState.Live;
		}
		m_chamber = BulletState.Live;
		m_partialGUI.renderer.material.color = m_bulletGUIColor;
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
		m_boltGUI[0].SetActive(m_slideState == SlideState.Forward);
		m_boltGUI[1].SetActive(m_slideState == SlideState.Back);
		
		for(int i = 0; i < m_magazineGUI.Length; i++){
			m_magazineGUI[i].SetActive(m_magazine[i] != BulletState.Empty);
		}
		if(m_chamber == BulletState.Empty || m_slideState == SlideState.Back){
			m_chamberGUI.SetActive (false);
		}
		else{
			m_chamberGUI.SetActive (true);
			m_chamberGUI.renderer.material.color = m_chamber == BulletState.Fired ? m_bulletDeadGUIColor : m_bulletGUIColor;
		}
		if(m_chamber == BulletState.Empty || m_slideState == SlideState.Forward){
			m_chamberBackGUI.SetActive(false);
		}
		else{
			m_chamberBackGUI.SetActive (true);
			m_chamberBackGUI.renderer.material.color = m_chamber == BulletState.Fired ? m_bulletDeadGUIColor : m_bulletGUIColor;
		}
		m_partialGUI.SetActive(m_partialReload);
	}
}
