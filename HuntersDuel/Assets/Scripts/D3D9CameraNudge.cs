using UnityEngine;
using System.Collections;

public class D3D9CameraNudge : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(SystemInfo.graphicsDeviceVersion.StartsWith("Direct3D 9.0")){
			transform.position += new Vector3(0.5f, -0.5f, 0);
		}

	}
}
