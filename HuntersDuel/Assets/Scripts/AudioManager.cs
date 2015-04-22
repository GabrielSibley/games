using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public static void PlaySFX(AudioClip clip){
		if(clip != null && instance != null){
			instance.audio.PlayOneShot(clip);
		}
	}
	
	static AudioManager instance;
	
	void Awake(){
		instance = this;
	}
}
