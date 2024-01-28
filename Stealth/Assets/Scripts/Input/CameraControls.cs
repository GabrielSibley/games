using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {
	
	public float panSpeed = 1;
	public float zoomSpeed = 1;
	public float minZoom = 4;
	public float maxZoom = 24;
	public Rect cameraLimits = new Rect(-5, -5, 30, 30);
	public Rect corners;
	public Vector3 camToCorners;
	
	void Update () {
		//TODO: limit this input to only working outside the gui
		if(Input.GetMouseButton(2)){
			Camera.main.transform.position = Camera.main.transform.position + new Vector3(Input.GetAxis("Mouse X") * panSpeed, 0, Input.GetAxis("Mouse Y") * panSpeed);
		}
		Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
		
		//Clamp zoom
		if(Camera.main.orthographicSize > maxZoom)
			Camera.main.orthographicSize = maxZoom;
		if(Camera.main.orthographicSize < minZoom)
			Camera.main.orthographicSize = minZoom;
		
		//Figure extent of non-UI space - this should probably be linked to GUIDriver
		Vector3 lowerLeftBound = Camera.main.ScreenToWorldPoint(new Vector3(0, 80, 1));
		Vector3 upperRightBound = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth - 336, Camera.main.pixelHeight, 1));
		corners = new Rect(lowerLeftBound.x, lowerLeftBound.z, upperRightBound.x - lowerLeftBound.x, upperRightBound.z - lowerLeftBound.z);
		
		
		Transform mct = Camera.main.transform;
		camToCorners = new Vector3((corners.x + corners.xMax)/2, 0, (corners.y + corners.yMax)/2) - mct.position;
		
		if(corners.width > cameraLimits.width)
			mct.position = new Vector3((cameraLimits.x + cameraLimits.xMax)/2 - camToCorners.x, mct.position.y, mct.position.z);
		else if(corners.x < cameraLimits.x)
			mct.position = new Vector3(mct.position.x + cameraLimits.x - corners.x, mct.position.y, mct.position.z);
		else if(corners.xMax > cameraLimits.xMax)
			mct.position = new Vector3(mct.position.x + cameraLimits.xMax - corners.xMax, mct.position.y, mct.position.z);
		
		if(corners.height > cameraLimits.height)
			mct.position = new Vector3(mct.position.x, mct.position.y, (cameraLimits.y + cameraLimits.yMax)/2 - camToCorners.z);
		else if(corners.y < cameraLimits.y)
			mct.position = new Vector3(mct.position.x, mct.position.y, mct.position.z + cameraLimits.y - corners.y);
		else if(corners.yMax > cameraLimits.yMax)
			mct.position = new Vector3(mct.position.x, mct.position.y, mct.position.z + cameraLimits.yMax - corners.yMax);
	}
}
