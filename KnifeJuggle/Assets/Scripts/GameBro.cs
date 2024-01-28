using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameBro : MonoBehaviour {
	
	private static GameBro instance;
	
	public  bool gameAlreadyOver;
	public GameObject[] endGameText;
	public GameObject[] knives;
	public Hand[] hands;
	
	void Awake(){
		instance = this;
		if(Menu.knives < 2){
			if(Random.value < 0.5f){
				knives[0].SetActive(false);
			}
			else{
				knives[1].SetActive(false);
			}
		}
	}
	
	void Update(){
		if(Winnput.HomeDown){
			SceneManager.LoadScene("title");
		}
	}
	
	public static Hand GetHand(int i){
		return instance.hands[i];
	}
	
	public static void EndGamePlayerDied(int player){
		instance.StartCoroutine(instance.endgame(player));
	}
	
	private IEnumerator endgame(int deadPlayer){
		if(!gameAlreadyOver){
			endGameText[deadPlayer].SetActive(true);
			gameAlreadyOver = true;
			Jukebox.PlayGameOverSfx();
			yield return new WaitForSeconds(5);
            SceneManager.LoadScene("title");
		}
	}
}
