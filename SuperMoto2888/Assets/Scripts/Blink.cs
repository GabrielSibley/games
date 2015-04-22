using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour {

	public float blinkTime;

	IEnumerator Start(){
		while(true){
			renderer.enabled = !renderer.enabled;
			yield return new WaitForSeconds(blinkTime);
		}
	}
}
