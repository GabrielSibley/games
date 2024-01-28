using UnityEngine;
using System.Collections;

public class BalloonSpawner : MonoBehaviour {
	
	public Balloon balloonPrefab;
	public float spawnInterval;
	
	// Use this for initialization
	IEnumerator Start () {
		while(true){
			yield return new WaitForSeconds(spawnInterval);
			Instantiate(balloonPrefab, transform.position, Quaternion.identity);
		}
	}
}
