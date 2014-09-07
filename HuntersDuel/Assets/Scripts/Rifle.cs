using UnityEngine;
using System.Collections;

public class Rifle : Gun {
	
	public enum BoltState {
		Closed,
		Up,
		Back,
	}
	
	public GameObject[] m_boltGUI;
	public GameObject[] m_bulletGUI;
	public GameObject m_bodyGUI;
	
	public Color m_bodyGUIColor, m_boltGUIColor, m_bulletGUIColor, m_bulletDeadGUIColor;
	
	public GameObject m_bulletPrefab;
	public float m_bulletSpeed;
	
	public AudioClip m_gunsfx;
	
	public int m_ammoReserve = 5;
	public int m_reserveMax = 15;
	
	protected BoltState m_boltState = BoltState.Closed;
	protected bool m_chambered = true;
	protected bool m_deadShell;
	protected BulletState[] m_magazine = new BulletState[5];
	protected bool m_partialReload;
	
	public override int Damage { get { return 6; } }	
	
	bool MagazineFull{get{return m_magazine[m_magazine.Length-1] != BulletState.Empty;}}
	
	public override void ManipulateUp(){
		if(m_boltState == BoltState.Closed){
			m_boltState = BoltState.Up;
		}
		else if(m_boltState == BoltState.Back && !MagazineFull && m_ammoReserve > 0){
			m_partialReload = true;
		}
	}
	public override void ManipulateDown(){
		if(m_boltState == BoltState.Up){
			m_boltState = BoltState.Closed;
		}
		else if(m_boltState == BoltState.Back && m_partialReload)
		{
			m_ammoReserve--;
			m_partialReload = false;
			AddLiveRound();
		}
	}
	public override void ManipulateLeft(){
		if(m_boltState == BoltState.Up){
			m_boltState = BoltState.Back;
			if(m_magazine[0] == BulletState.Fired){
				RemoveTopRound();
			}
		}
	}
	public override void ManipulateRight(){
		if(m_boltState == BoltState.Back){
			m_boltState = BoltState.Up;
			m_partialReload = false;
		}
	}
	
	public override bool Fire(BasicMove player, float angle){
		if(m_magazine[0] == BulletState.Live && m_boltState == BoltState.Closed){
			AudioManager.PlaySFX(m_gunsfx);
			m_magazine[0] = BulletState.Fired;
			
			GameObject bullet = Instantiate(m_bulletPrefab, player.transform.position, Quaternion.identity) as GameObject;
			bullet.rigidbody.velocity = Quaternion.Euler(0, 0, angle) * new Vector3(m_bulletSpeed, 0, 0);
			Physics.IgnoreCollision(bullet.collider, player.collider);
			return true;
		}
		return false;
	}
	
	void Start(){
		m_bodyGUI.renderer.material.color = m_bodyGUIColor;
		for(int i = 0; i < m_boltGUI.Length; i++){
			m_boltGUI[i].renderer.material.color = m_boltGUIColor;
		}
		for(int i = 0; i < m_bulletGUI.Length; i++){
			m_bulletGUI[i].renderer.material.color = m_bulletGUIColor;
		}
		for(int i = 0; i < 5; i++){
			AddLiveRound();
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
		m_boltGUI[0].SetActive(m_boltState == BoltState.Closed);
		m_boltGUI[1].SetActive(m_boltState == BoltState.Up);
		m_boltGUI[2].SetActive(m_boltState == BoltState.Back);
		
		for(int i = 0; i < 5; i++){
			m_bulletGUI[i].SetActive(m_magazine[i] != BulletState.Empty);
		}
		if(m_boltState != BoltState.Back && m_magazine[0] != BulletState.Empty){
			m_bulletGUI[0].SetActive(false);
			m_bulletGUI[5].SetActive(true);
			m_bulletGUI[5].renderer.material.color = m_magazine[0] == BulletState.Fired ? m_bulletDeadGUIColor : m_bulletGUIColor;
		}
		else{
			m_bulletGUI[5].SetActive(false);
			m_bulletGUI[0].renderer.material.color = m_magazine[0] == BulletState.Fired ? m_bulletDeadGUIColor : m_bulletGUIColor;
		}
		m_bulletGUI[6].SetActive(m_partialReload);
	}
}
