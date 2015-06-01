using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InputManager : MonoBehaviour {

	public static List<IInputListener> InputListeners = new List<IInputListener>();

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown (0)){
			foreach(var listener in InputListeners)
			{
				if(listener.OnInputDown())
				{
					return;
				}
			}
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

	public static Vector3 InputWorldPos{
		get{
			return Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
	}
}
