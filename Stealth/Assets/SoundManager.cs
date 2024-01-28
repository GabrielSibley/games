using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {

	public static SoundManager instance;
	public static int soundIndex = 3;
	
	public static void WeaponFired(Firearm weapon){
		instance.audio.PlayOneShot(weapon.sfx[soundIndex]);
	}
	
	protected void Awake(){
		instance = this;
	}
}
