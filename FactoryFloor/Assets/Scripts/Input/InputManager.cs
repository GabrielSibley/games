using UnityEngine;
using System.Collections;
using System.Linq;

public class InputManager : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown (0)){
			RaycastHit hit;
			if(Physics.Raycast (Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
			{
				IInputReceiver receiver = hit.collider.gameObject.GetComponents<Component>().Where(x => x is IInputReceiver).FirstOrDefault() as IInputReceiver;
				if(receiver != null)
				{
					receiver.OnInputDown();
				}
			}
		}
	}
}
