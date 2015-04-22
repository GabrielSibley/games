using UnityEngine;
using System.Collections;

public class TestInputReceiver : MonoBehaviour, IInputReceiver {

	public void OnInputDown(){
		Debug.Log ("click!");
	}
}
