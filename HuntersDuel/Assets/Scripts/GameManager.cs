using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static void PlayerDied(){
		m_instance.StartCoroutine(m_instance._PlayerDied());
	}
	
	private static GameManager m_instance;
	
	public BasicMove[] m_players;
	
	public GameObject m_victoryGraham, m_victoryTom, m_draw;
	
	void Awake(){
		m_instance = this;
	}
	
	IEnumerator _PlayerDied(){
		yield return new WaitForSeconds(4);
		Application.LoadLevel("game");
	}
	
	void Update(){
		m_victoryGraham.SetActive(m_players[0].m_isAlive && !m_players[1].m_isAlive);
		m_victoryTom.SetActive(!m_players[0].m_isAlive && m_players[1].m_isAlive);
		m_draw.SetActive(!m_players[0].m_isAlive && !m_players[1].m_isAlive);
	}
}
