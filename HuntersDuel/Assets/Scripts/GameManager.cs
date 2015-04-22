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
		Application.LoadLevel("title");
	}
	
	void Update(){
		m_victoryGraham.SetActive(HasWon(0));
		m_victoryTom.SetActive(HasWon(1));
		m_draw.SetActive(!m_players[0].m_isAlive && !m_players[1].m_isAlive);
	}

	bool HasWon(int player){
		int otherPlayer = 1 - player;
		return m_players[player].m_isAlive //you are alive
			&& !m_players[otherPlayer].m_isAlive //opponent is dead
			&& !Bullet.bullets.Exists ( x => x.owner == otherPlayer); //opponent has no bullets in air
	}
}
