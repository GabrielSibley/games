using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu : MonoBehaviour {

	public static int knives = 1;
	
	public GameObject select1active, select1inert,select2active, select2inert;
	
	void Awake(){
		SetSelection(knives, false);
	}
	
	void SetSelection(int k, bool playSfx){
		knives = k;
		if(k == 1){
			select1active.SetActive(true);
			select1inert.SetActive(false);
			select2active.SetActive(false);
			select2inert.SetActive(true);
		}
		else{
			select1active.SetActive(false);
			select1inert.SetActive(true);
			select2active.SetActive(true);
			select2inert.SetActive(false);
		}
		if(playSfx)
		{
			GetComponent<AudioSource>().Play();
		}
	}
	
	void Update(){
		bool moveLeft = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
		bool moveRight = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
		bool moveUp = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
		bool moveDown = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
		
		if(Winnput.HomeDown){
			Application.Quit();
		}
		else if(
				moveLeft || moveRight || moveUp || moveDown
		){
			if(knives == 1){
				SetSelection(2, true);
			}
			else{
				SetSelection(1, true);
			}
		}
		else if(Winnput.AnyButtonDown){
			Jukebox.PlayStabSfx();
            SceneManager.LoadScene("game");
		}
	}
}
