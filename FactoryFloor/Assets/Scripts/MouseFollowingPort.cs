using UnityEngine;
using System.Collections;

public class MouseFollowingPort : Port {

	public override bool IsReal{get{return false;}}
	public override Vector2 WorldPosition{
		get {
			return InputManager.InputWorldPos;
		}
	}

	public MouseFollowingPort(PortType type): base(type)
	{
		//super
	}

	public override bool TryPutCrate(Crate crate)
	{
		Debug.LogWarning ("Tried to put crate in mouse port");
		return false;
	}
	public override bool TryGetCrate(out Crate crate)
	{
		Debug.LogWarning ("Tried to get crate from mouse port");
		crate = null;
		return false;
	}
}
