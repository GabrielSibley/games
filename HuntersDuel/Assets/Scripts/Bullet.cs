using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	
	public void OnCollisionEnter(){
		Destroy(gameObject);
	}
}
