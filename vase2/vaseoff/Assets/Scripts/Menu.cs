using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {


	
	// Update is called once per frame
	void Update () {
	    if(Winnput.AnyButtonDown)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
	}
}
