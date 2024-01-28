using UnityEngine;
using System.Collections;

public class Jukebox : MonoBehaviour {
	
	private static Jukebox instance;
	
	public AudioSource music;
	public AudioSource sfx;
	public AudioClip[] stabSfx;
	public AudioClip healSfx;
	public AudioClip gameOverSfx;
	public AudioClip popSfx;
	
	void Start()
	{
		if(!instance)
		{
			instance = this;
			music.Play();
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
	
	public static void PlayStabSfx()
	{
		instance.sfx.PlayOneShot(instance.stabSfx[Random.Range(0, instance.stabSfx.Length)]);
	}
	
	public static void PlayHealSfx()
	{
		instance.sfx.PlayOneShot(instance.healSfx);
	}
	
	public static void PlayGameOverSfx()
	{
		instance.sfx.PlayOneShot(instance.gameOverSfx);
	}
	
	public static void PlayPopSfx()
	{
		instance.sfx.PlayOneShot(instance.popSfx);
	}
}
