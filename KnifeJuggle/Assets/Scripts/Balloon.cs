using UnityEngine;
using System.Collections;

public class Balloon : MonoBehaviour {
	
	public float upSpeed;
	public int playerId;
	
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody>().velocity = Vector3.up * upSpeed;
	}
	
	void Update (){
		if(transform.position.y > 10)
		{
			Destroy(gameObject);
		}
	}
}
