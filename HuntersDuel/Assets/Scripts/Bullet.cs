using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour {
	public static List<Bullet> bullets = new List<Bullet>();

	public const float bulletSpawnDistance = 16;

	public Transform rendererTransform;
	public int damage;
	public int owner;

	IEnumerator Start(){
		rendererTransform.localScale = new Vector3(16, 16, 16);
		yield return 0;
		rendererTransform.localScale = new Vector3(8,8,8);
	}

	void Awake(){
		bullets.Add(this);
	}

	void OnDestroy(){
		bullets.Remove(this);
	}
	
	public void OnCollisionEnter(){
		Destroy(gameObject);
	}
}
