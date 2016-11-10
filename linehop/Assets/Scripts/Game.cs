using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum GameMode{
	ReadyUp,
	Play,
	GameOver
}

public class Game : MonoBehaviour {
	
    const float PI2 = 2 * Mathf.PI;
    const int playerCount = 2;

	public PlayerTextureSet m_grahamTextures, m_tomTextures;
	public Texture2D m_backgroundTexture;
	public Rect[] m_basePlayerPositions = new Rect[2];
	public GameMode m_gameMode;
	public float m_lineSpeed; // in rad / sec
    public float m_jumpDuration; //in radians
    public float m_lineSpeedUp; //In rad / sec / sec
    public int m_jumpHeight;
    public Rect m_baseLineRect;
    public float m_lineSwingHeight;
    public AudioClip[] m_jumpSounds;
    public AudioClip m_tripSound;
    public Font m_font;
    public Rect[] m_tripRects;

    private float m_lineTheta; //in radians    
    private Rect m_lineRect;    
    private float[] m_jumpThetas = new float[] { -10, -10 };
    private int[] m_trips = new int[playerCount];
    private bool m_gameOverGrahamWins;
    private bool m_gameOverTomWins;
    private bool m_gameOverDraw;
    private Rect[] m_playerPositions = new Rect[2];

    private bool[] m_playerReady = new bool[2];
    

    private bool IsGrounded(int player)
    {
        return m_lineTheta - m_jumpThetas[player] >= m_jumpDuration; 
    }
	void Update(){
        //Inputs
        bool[] jumpButtonDown = Winnput.ADown;		
		
        if(m_gameMode == GameMode.Play)
        {
            m_lineSpeed += m_lineSpeedUp * Time.deltaTime;
        }

		if(m_gameMode == GameMode.Play || m_gameMode == GameMode.ReadyUp){
            //Advance line
            float oldLineTheta = m_lineTheta;
            m_lineTheta += m_lineSpeed * Time.deltaTime;

            //Check for jumping
            for(int i = 0; i < playerCount; i++)
            {
                if(IsGrounded(i) && jumpButtonDown[i])
                {
                    PlayerJumped(i);
                }
            }

            //Trip check
            if((int)(oldLineTheta / PI2) < (int)(m_lineTheta / PI2))
            {
                for(int i = 0; i < playerCount; i++)
                {
                    if(IsGrounded(i))
                    {
                        PlayerTripped(i);
                    }
                    else
                    {
                        m_playerReady[i] = true;                        
                    }
                } 
            }
            CheckForGameStart();
            CheckForGameOver();		
		}
        
        //Update player positions
        for(int i = 0; i < m_playerPositions.Length; i++)
        {
            m_playerPositions[i] = m_basePlayerPositions[i];
            if(!IsGrounded(i))
            {
                float jumpProgress = (m_lineTheta - m_jumpThetas[i]) / m_jumpDuration;
                if(jumpProgress > 0.5f)
                {
                    jumpProgress = 1 - jumpProgress;
                }
                jumpProgress *= 2;
                m_playerPositions[i].y -= jumpProgress * m_jumpHeight;                
            }
            m_playerPositions[i].y = Mathf.RoundToInt(m_playerPositions[i].y / 4) * 4;
        }

        //Update line position
        m_lineRect = m_baseLineRect;
        m_lineRect.y = Mathf.Lerp(m_baseLineRect.y, m_baseLineRect.y + m_lineSwingHeight, (Mathf.Cos(m_lineTheta) + 1) / 2);
        m_lineRect.y = Mathf.RoundToInt(m_lineRect.y / 4) * 4;
    }

    private void CheckForGameStart()
    {
        if(m_gameMode == GameMode.ReadyUp && m_playerReady[0] && m_playerReady[1])
        {
            m_gameMode = GameMode.Play;
        }
    }

    private void CheckForGameOver()
    {
        if(m_trips[0] >= 3)
        {
            if(m_trips[1] >= 3)
            {
                m_gameOverDraw = true;
                StartCoroutine(EndGameAndRestart());                
            }
            else
            {
                m_gameOverTomWins = true;
                StartCoroutine(EndGameAndRestart());
            }
        }
        else if(m_trips[1] >= 3)
        {
            m_gameOverGrahamWins = true;
            StartCoroutine(EndGameAndRestart());
        }
    }

    void OnGUI(){
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / 672f, Screen.height/480f, 1));
		Rect fullscreen = new Rect(0, 0, 672, 480);
		
		//Background
		GUI.DrawTexture(fullscreen, m_backgroundTexture);
        
        //Players + Line
        if(m_lineTheta % PI2 < Mathf.PI)
        {
            //line in back
            DrawLine();
            DrawPlayers();
        }
        else
        {
            //players in back
            DrawPlayers();
            DrawLine();
        }

        if(m_gameMode == GameMode.ReadyUp)
        {
            GUIStyle style = GUI.skin.GetStyle("label");
            style.alignment = TextAnchor.UpperCenter;
            style.font = m_font;
            GUI.Label(new Rect(0, 148, fullscreen.width, 64), "Linehop to start");
        }

        //Trips
        for(int i = 0; i < m_trips.Length; i++)
        {
            string tripString = "";
            for(int j = 0; j < m_trips[i]; j++)
            {
                tripString += "X";
            }
            GUI.color = Color.red;
            GUI.Label(m_tripRects[i], tripString);
            GUI.color = Color.white;
        }

        //Game over
        if(m_gameMode == GameMode.GameOver)
        {
            if(m_gameOverGrahamWins)
            {
                GUI.Label(new Rect(0, 148, fullscreen.width, 64), "Graham Wins");
            }
            if (m_gameOverTomWins)
            {
                GUI.Label(new Rect(0, 148, fullscreen.width, 64), "Tom Wins");
            }
            if (m_gameOverDraw)
            {
                GUI.Label(new Rect(0, 148, fullscreen.width, 64), "Draw");
            }
        }
	}

    void DrawLine()
    {
        GUI.DrawTexture(m_lineRect, Texture2D.whiteTexture);
    }

    void DrawPlayers()
    {
        for (int i = 0; i < playerCount; i++)
        {
            PlayerTextureSet texSet = i == 0 ? m_grahamTextures : m_tomTextures;
            Texture2D tex = IsGrounded(i) ? texSet.m_narrowDown : texSet.m_narrowUp;
            GUI.DrawTexture(m_playerPositions[i], tex);
        }
    }

    void PlayerJumped(int player)
    {
        m_jumpThetas[player] = m_lineTheta;
        GetComponent<AudioSource>().PlayOneShot(m_jumpSounds[player]);
    }

    void PlayerTripped(int player)
    {
        m_playerReady[player] = false;
        if (m_gameMode == GameMode.Play)
        {
            m_trips[player]++;
            GetComponent<AudioSource>().PlayOneShot(m_tripSound);
        }
    }

    IEnumerator EndGameAndRestart()
    {
        m_gameMode = GameMode.GameOver;
        yield return new WaitForSeconds(5);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}

[System.Serializable]
public class PlayerTextureSet{
	public Texture2D m_narrowDown, m_narrowUp, m_wideDown, m_wideUp;
}
