using UnityEngine;
using System.Collections;

public class Boxing : MonoBehaviour {

	public AudioClip punch, whiff;
	public Texture2D ring;
	public Texture2D[] standing;
	public Texture2D[] punching;
	public float[] startingPosition;
	public float[] position;
	public float floorOffset;
	public float moveSpeed = 200;
	public int showWinner = -1;
	public Texture2D[] winnerBanners;
	public float hitDistance;
	
	private bool lockout;
	private int[] hits = new int[2];
	
	void Update(){
		if(lockout)
			return;
		for(int player = 0; player < 2; player++){
			if(Winnput.ADown[player]){				
				//If players are close enough, score a hit
				if(Mathf.Abs(position[0] - position[1]) < hitDistance){
					hits[player]++;
					audio.PlayOneShot(punch);
				}
				else{
					audio.PlayOneShot(whiff);
				}
			}
			position[player] += Winnput.Horizontal[player] * Time.deltaTime * moveSpeed;
		}
		if(hits[0] >= 10 && hits[1] >= 10){
			//Double KO!
			StartCoroutine(DeclareWinner(3));
		}
		else if(hits[0] >= 10){
			//Graham wins
			StartCoroutine(DeclareWinner(0));
		}
		else if(hits[1] >= 10){
			//Tom wins
			StartCoroutine(DeclareWinner(1));
		}
	}
	
	IEnumerator DeclareWinner(int winner){
		lockout = true;
		showWinner = winner;
		yield return new WaitForSeconds(2);
		showWinner = -1;
		lockout = false;
		StartOver();
	}
	
	void StartOver(){
		for(int player = 0; player < 2; player++){
			position[player] = startingPosition[player];
			hits[player] = 0;
		}
	}
	
	void OnGUI(){
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / 640f, Screen.height/480f, 1));
		GUI.DrawTexture(new Rect(0, 0, 640, 480), ring);
		for(int player = 0; player < 2; player++){
			GUI.DrawTexture(new Rect(position[player], floorOffset, 256, 256), Winnput.A[player] ? punching[player] : standing[player]);
		}
		if(showWinner >= 0){
			GUI.DrawTexture(new Rect(0, 0, 640, 480), winnerBanners[showWinner]);
		}
	}
}
