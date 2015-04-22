using UnityEngine;
using System.Collections;

public class BasicMove : MonoBehaviour {
		
	public int m_player;
	public float m_aimSpeed; //In deg/sec
	public int health = 6;
	public GameObject m_reticle;
	public float m_reticleDistance;
	
	public Renderer m_renderer;
	public Texture2D[] m_walkTextures;
	public Texture2D m_deadTexture;

	private Gun gun;
	public Pistol pistol;
	public Rifle rifle;
	public Shotgun shotgun;
	public PlayerUI ui;
	
	public bool IsAiming{get; set;}
	public bool m_isAlive = true;
	
	protected float m_aimAngle; //In degrees

	void Start() {
		Application.targetFrameRate = 60;
		switch(TitleManager.PlayerGuns[m_player]){
			case GunType.Pistol:
				gun = pistol;
				break;
			case GunType.Rifle:
				gun = rifle;
				break;
			case GunType.Shotgun:
				gun = shotgun;
				break;
		}
		gun.gameObject.SetActive(true);
	}

	void Update () {
		rigidbody.velocity = Vector3.zero;
		IsAiming = false;		
		
		if(m_isAlive && Winnput.A[m_player] && !(Winnput.B[m_player] && Winnput.ADown[m_player])){
			//Work dat bolt
			if(Winnput.UpDown[m_player]){
				gun.ManipulateUp();
			}
			if(Winnput.DownDown[m_player]){
				gun.ManipulateDown();
			}
			if(Winnput.LeftDown[m_player]){
				gun.ManipulateLeft();
			}
			if(Winnput.RightDown[m_player]){
				gun.ManipulateRight();
			}
		}
		
		if(m_isAlive && Winnput.B[m_player]){
			//Aiming
			IsAiming = true;
			
			if(!Winnput.A[m_player]){
				if(Winnput.Up[m_player]){
					AimTowards(90);
				}
				else if(Winnput.Down[m_player]){
					AimTowards(270);
				}
				else if(Winnput.Right[m_player]){
					AimTowards(0);
				}
				else if(Winnput.Left[m_player]){
					AimTowards(180);
				}
			}
			
			//Firing
			if(Winnput.ADown[m_player]){
				gun.Fire(this, m_aimAngle);
			}
		}
		if(m_isAlive && (!(Winnput.A[m_player] || Winnput.B[m_player]))){
			//Movement
			if(Winnput.Up[m_player]){
				rigidbody.velocity += new Vector3(0, gun.WalkSpeed, 0);
				m_aimAngle = 90;
			}
			else if(Winnput.Down[m_player]){
				rigidbody.velocity += new Vector3(0, -gun.WalkSpeed, 0);
				m_aimAngle = 270;
			}
			else if(Winnput.Right[m_player]){
				rigidbody.velocity += new Vector3(gun.WalkSpeed, 0 , 0);
				m_aimAngle = 0;
			}
			else if(Winnput.Left[m_player]){
				rigidbody.velocity += new Vector3(-gun.WalkSpeed, 0 , 0);
				m_aimAngle = 180;
			}
		}
		
		m_reticle.SetActive(IsAiming);
		m_reticle.transform.localPosition = Quaternion.Euler(0, 0, m_aimAngle) * new Vector3(m_reticleDistance, 0, -100);
		
		//Update appearance
		if(m_isAlive){
			if((m_aimAngle >= 0 && m_aimAngle < 45) || m_aimAngle >= 315){
				m_renderer.material.mainTexture = m_walkTextures[0];
			}
			else if(m_aimAngle >= 45 && m_aimAngle < 135){
				m_renderer.material.mainTexture = m_walkTextures[1];
			}
			else if(m_aimAngle >= 135 && m_aimAngle < 225){
				m_renderer.material.mainTexture = m_walkTextures[2];
			}
			else{
				m_renderer.material.mainTexture = m_walkTextures[3];
			}
		}
		else{
			m_renderer.material.mainTexture = m_deadTexture;
		}
		ui.healthUI.Display (health);
	}
	
	void AimTowards(float degAngle){
		Quaternion adjustedAim = Quaternion.RotateTowards(
			Quaternion.Euler(0, 0, m_aimAngle),
			Quaternion.Euler(0, 0, degAngle),
			m_aimSpeed * Time.deltaTime
		);
		m_aimAngle = adjustedAim.eulerAngles.z;
	}
	
	void OnCollisionEnter(Collision collisionInfo){
		if(m_isAlive && collisionInfo.gameObject.GetComponent<Bullet>()){
			//Touched a bullet, get damaged
			health = Mathf.Max(0, health - collisionInfo.gameObject.GetComponent<Bullet>().damage);
			if(health == 0){
				GameManager.PlayerDied();
				m_isAlive = false;
			}
		}
	}
}
