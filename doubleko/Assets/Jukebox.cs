using UnityEngine;
using System.Collections;

public class Jukebox : MonoBehaviour {
	
	public AudioClip[] tracks;
	
	protected int currentTrack;
	
	// Update is called once per frame
	void Update () {
		if(!audio.isPlaying){
			currentTrack = (currentTrack + 1) % tracks.Length;
			audio.clip = tracks[currentTrack];
			audio.Play();
		}
	}
}
