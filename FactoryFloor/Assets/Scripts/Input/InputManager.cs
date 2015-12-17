using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InputManager : MonoBehaviour {

	public static List<IInputListener> InputListeners = new List<IInputListener>();

	public static IDraggable Dragged;

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

	public static Vector2 SimToWorld(Vector2 simPos)
	{
		return (Vector2)FloorView.GetTile(new Vector2i(0, 0)).transform.position + Vector2.Scale (simPos, FloorView.TileSize);
	}
}
