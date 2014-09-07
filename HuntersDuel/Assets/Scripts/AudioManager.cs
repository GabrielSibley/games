using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public static void PlaySFX(AudioClip clip){
		instance.audio.PlayOneShot(clip);
	}
	
	static AudioManager instance;
	
	void Awake(){
		instance = this;
	}
}
