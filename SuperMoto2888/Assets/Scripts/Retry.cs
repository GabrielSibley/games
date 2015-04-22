using UnityEngine;
using System.Collections;

public class Retry : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.Z)){
			Application.LoadLevel("scene");
		}
	}
}
