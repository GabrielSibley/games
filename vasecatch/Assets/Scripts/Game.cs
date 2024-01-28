using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameMode{
	Play,
	AlmostGameOver,
	GameOver,
	Menu
}

public class Game : MonoBehaviour {
	
	public PlayerTextureSet m_grahamTextures, m_tomTextures;
	public Texture2D m_vaseTexture;
	public Texture2D m_backgroundTexture;
	public Texture2D m_titleCardTexture, m_grahamVsTomTexture, m_menuTexture, m_grahamSelectTexture, m_gameOverTexture;
	public Rect m_catcherPosition;
	public Rect m_throwerPosition;
	public GameMode m_gameMode;
	public AudioClip m_sfxWalk1, m_sfxWalk2, m_sfxCatch, m_sfxBreak, m_sfxGameOver, m_sfxSelectDown, m_sfxSelectUp, m_sfxSelect;
	public float m_catcherMin = 48, m_catcherMax = 576;
	public float m_throwTime;
	public float m_fastThrowTime;
	public AudioSource m_throwSource;
	public bool m_cheatEnabled;
	public float m_cheatSeekAlpha = 0.5f;
	
	private PlayerTextureSet m_catcherTextures, m_throwerTextures;
	private bool m_catcherWideStance = false;
	private List<Vase> m_vases = new List<Vase>();
	private float m_moveCooldown;
	private float m_throwProgression;
	private int m_throwStage;
	private Vase m_brokenVase;
	private float m_rubItInTime;
	private bool m_playAsGraham = true;
	private int m_vasesThrown;
	
	void Start () {
		m_catcherTextures = m_grahamTextures;
		m_throwerTextures = m_tomTextures;
	}
	
	void Update(){
		//Inputs
		bool moveLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
		bool moveRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
		bool moveUp = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
		bool moveDown = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
		bool fire = Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return);
		
		if(m_gameMode == GameMode.Play){
			//Catcher movement
			m_moveCooldown -= Time.deltaTime;
			if(moveLeft && !moveRight && m_catcherPosition.x > m_catcherMin){
				if(m_moveCooldown <= 0){
					m_moveCooldown += m_catcherTextures.m_moveTime;
					m_catcherPosition.x = m_catcherPosition.x - 8;
					m_catcherWideStance = !m_catcherWideStance;
					audio.PlayOneShot(m_catcherWideStance ? m_sfxWalk1 : m_sfxWalk2);
				}
			}			
			else if(moveRight && !moveLeft && m_catcherPosition.x < m_catcherMax){				
				if(m_moveCooldown <= 0){
					m_moveCooldown += m_catcherTextures.m_moveTime;
					m_catcherPosition.x = m_catcherPosition.x + 8;
					m_catcherWideStance = !m_catcherWideStance;
					audio.PlayOneShot(m_catcherWideStance ? m_sfxWalk1 : m_sfxWalk2);
				}
			}
			else if(m_moveCooldown < 0){
				m_moveCooldown= 0;
			}
			
			//Thrower prep / throw
			if(CanThrowVase){
				m_throwProgression += Time.deltaTime;
				
				float throwTime;
				if(m_vasesThrown >= 3){
					throwTime = m_fastThrowTime;
				}
				else if(m_vasesThrown == 2 && m_throwStage == 1){
					throwTime = m_throwTime * 3;
				}
				else{
					throwTime = m_throwTime;
				}
				
				if(m_throwProgression >= throwTime){
					m_throwStage++;
					m_throwProgression = 0;
				}

				if(m_throwStage == 2){
					m_vasesThrown++;
					Vase v = new Vase();
					v.m_position = new Rect(630, 60, 36, 36);
					v.m_speed = new Vector2(Random.Range(-8f, -60f), Random.Range(-10f, -40f));
					m_vases.Add(v);
					m_throwProgression = 0;
					m_throwSource.Play();
					m_throwStage = 0;
				}
			}
			
			for(int i = 0; i < m_vases.Count; i++){
				m_vases[i].Update();
				if(m_cheatEnabled && m_vases[i].m_position.y >= 120){
					float alpha = Mathf.Pow(m_cheatSeekAlpha, Time.deltaTime);
					float distFromGround = 400 - m_vases[i].m_position.y;
					float v =  m_vases[i].m_speed.y;
					float timeToCrash = (-v + Mathf.Sqrt(v*v + 32 * distFromGround)) / 16;
					m_vases[i].m_speed.x = m_vases[i].m_speed.x * alpha + (m_catcherPosition.x - m_vases[i].m_position.x) / timeToCrash * (1-alpha);
				}
				//Catch vase
				if(Mathf.Abs(
						m_vases[i].m_position.x
						- m_catcherPosition.x
						+ m_vases[i].m_position.width / 2
						- m_catcherPosition.width / 2)
					<= 24
					&& m_vases[i].m_position.y >= 352){
					m_vases.RemoveAt(i);
					i--;
					audio.PlayOneShot(m_sfxCatch);
					continue;
				}
				//Break vase
				if(m_vases[i].m_position.y >= 400){
					m_brokenVase = m_vases[i];
					m_vases.Clear();
					audio.PlayOneShot(m_sfxBreak);
					m_gameMode = GameMode.AlmostGameOver;
					m_rubItInTime = 0;
					continue;
				}
			}
			
			//Fake slowdown
			Time.timeScale = m_vases.Count <= 32 ? 1 : 32f / m_vases.Count;
		}
		
		if(m_gameMode == GameMode.AlmostGameOver){
			Time.timeScale = 1;
			m_rubItInTime += Time.deltaTime;
			if(m_rubItInTime > 3){
				m_rubItInTime = 0;
				m_gameMode = GameMode.GameOver;
				audio.PlayOneShot(m_sfxGameOver);
			}
		}
		
		if(m_gameMode == GameMode.GameOver){
			m_brokenVase = null;
			m_vasesThrown = 0;
			m_rubItInTime += Time.deltaTime;
			if(m_rubItInTime > m_sfxGameOver.length + 1){
				m_gameMode = GameMode.Menu;
			}
		}
		
		if(m_gameMode == GameMode.Menu){
			if(moveUp){
				m_playAsGraham = !m_playAsGraham;
				audio.PlayOneShot(m_sfxSelectUp);
			}
			if(moveDown){
				m_playAsGraham = !m_playAsGraham;
				audio.PlayOneShot(m_sfxSelectDown);
			}
			if(fire){
				audio.PlayOneShot(m_sfxSelect);
				if(m_playAsGraham){
					m_catcherTextures = m_grahamTextures;
					m_throwerTextures = m_tomTextures;
				}
				else{
					m_catcherTextures = m_tomTextures;
					m_throwerTextures = m_grahamTextures;
				}
				m_catcherPosition.x = 384;
				m_gameMode = GameMode.Play;
			}
		}
	}
	
	bool CanThrowVase{
		get{
			if(m_vasesThrown >= 3){
				return true;
			}
			return m_vases.Count == 0;
		}
	}
	
	void OnGUI(){
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / 672f, Screen.height/480f, 1));
		Rect fullscreen = new Rect(0, 0, 672, 480);
		
		if(m_gameMode == GameMode.Play || m_gameMode == GameMode.AlmostGameOver){
			//Background
			GUI.DrawTexture(fullscreen, m_backgroundTexture);
			//Catcher
			GUI.DrawTexture(m_catcherPosition, m_catcherWideStance ? m_catcherTextures.m_wideUp : m_catcherTextures.m_narrowUp);
			//Thrower			
			if(m_throwStage == 0){
				GUI.DrawTexture(m_throwerPosition, m_throwerTextures.m_narrowDown);
			}
			if(m_throwStage == 1){
				GUI.DrawTexture(m_throwerPosition, m_throwerTextures.m_narrowUp);
				GUI.DrawTexture(new Rect(630, 60, 36, 36), m_vaseTexture);
			}
			//Vase(s)
			for(int i = 0; i < m_vases.Count; i++){
				GUI.DrawTexture(m_vases[i].m_position, m_vaseTexture);
			}
			if(m_brokenVase != null){
				if(Mathf.FloorToInt(Time.time * 5) % 2 == 0){
					GUI.DrawTexture(m_brokenVase.m_position, m_vaseTexture);
				}
			}
		}
		
		if(m_gameMode == GameMode.GameOver){
			GUI.DrawTexture(new Rect(152, 120, 368, 264), m_gameOverTexture);
		}
		
		if(m_gameMode == GameMode.Menu){
			
			GUI.DrawTexture(new Rect(0, 12, 642, 90), m_grahamVsTomTexture); 
			
			GUI.DrawTexture(new Rect(120, 120, 48, 48), m_playAsGraham ? m_grahamTextures.m_narrowUp : m_grahamTextures.m_narrowDown);
			GUI.DrawTexture(new Rect(530, 120, 48, 48), m_playAsGraham ? m_tomTextures.m_narrowDown : m_tomTextures.m_narrowUp);
			
			GUI.DrawTexture(new Rect(169, 200, 340, 44), m_titleCardTexture); 
			
			GUI.DrawTexture(new Rect(150, 300, 416, 170), m_menuTexture);
			
			if(m_playAsGraham){
				GUI.DrawTexture(new Rect(200, 298, 32, 32), m_grahamSelectTexture);
			}
			else{
				GUI.DrawTexture(new Rect(200, 322, 32, 32), m_grahamSelectTexture);
			}
		}
	}
}

[System.Serializable]
public class PlayerTextureSet{
	public Texture2D m_narrowDown, m_narrowUp, m_wideDown, m_wideUp;
	public float m_moveTime;
}

public class Vase{
	public Rect m_position;
	public Vector2 m_speed;
	public Vector2 m_positionCarryOver;
	
	public void Update(){
		m_speed.y += 16 * Time.deltaTime;
		m_positionCarryOver.x += m_speed.x * Time.deltaTime;
		m_positionCarryOver.y += m_speed.y * Time.deltaTime;
		if(m_positionCarryOver.x <= -4){
			m_position.x += -4;
			m_positionCarryOver.x += 4;
		}
		if(m_positionCarryOver.x >= 4){
			m_position.x += 4;
			m_positionCarryOver.x += -4;
		}
		if(m_positionCarryOver.y <= -4){
			m_position.y += -4;
			m_positionCarryOver.y += 4;
		}
		if(m_positionCarryOver.y >= 4){
			m_position.y += 4;
			m_positionCarryOver.y += -4;
		}
	}
}
