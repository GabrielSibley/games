using UnityEngine;
using System.Collections;

public class MouseFollowingPort : Port {

	public override Vector2 WorldPosition{
		get {
			return InputManager.InputWorldPos;
		}
	}

	public MouseFollowingPort(PortType type): base(type)
	{
		//super
	}
}
