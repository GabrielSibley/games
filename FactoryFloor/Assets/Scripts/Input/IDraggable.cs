using UnityEngine;
using System.Collections;

//Things carryable by the mouse/touch
public interface IDraggable  {

	void OnDragStart(Vector2 inputPosition);
	void OnDragEnd(Vector2 inputPosition);
	void OnDragged(Vector2 inputPosition);
}
