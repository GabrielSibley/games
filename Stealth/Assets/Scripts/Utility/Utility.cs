using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utility{	
	public static string LimitedFloat(float f, int precision){
		string s = ""+f;
		int dPt = s.IndexOf('.');
		if(dPt >= 0){
			if(precision > 0){
				if(dPt + precision + 1 < s.Length)
					s = s.Substring(0, dPt+precision+1);
			}
			else
				s = s.Substring(0, dPt);
		}
		if(precision > 0){
			if(dPt < 0){
				dPt = s.Length;
				s = s + ".";
			}
			int padding = precision - (s.Length - dPt);
			for(int i = 0; i <= padding; i++)
				s = s + "0";
		}
		return s;
	}
	
	public static bool RaisedLineCast(Vector3 start, Vector3 end, int layerMask){
		return Physics.Linecast(start + Vector3.up, end + Vector3.up, layerMask);
	}
	
	public static bool RaisedSphereCast(Vector3 start, Vector3 end, float radius, int layerMask){
		RaycastHit rHit = new RaycastHit();
		Vector3 fromTo = end - start;
		return Physics.SphereCast(start + Vector3.up, radius, fromTo, out rHit, fromTo.magnitude, layerMask);
	}
	
	public static string FormatTime(float time){
		int minutes = (int)(time / 60);
		float seconds = time - minutes * 60;
		return string.Format("{0:00}:{1:00.00}", minutes, seconds);
	}
	
	public static string FormatTimeSeconds(float time){
		return string.Format("{0:0.00}", time);
	}
	
	public static void FindRicochet(GridSquare start, GridSquare end, int layerMask){
		float angle = 0;
		float increment = 4;
		for(; angle < 360; angle += increment){
			List<Vector3> result = Ricochet(start.transform.position + Vector3.up, Quaternion.Euler(0, angle, 0) * Vector3.right, end.transform.position + Vector3.up, 0.3f, 20, layerMask, 0);
			if(result != null){
				for(int i = 0; i < result.Count - 1; i++){
					Debug.DrawLine(result[i], result[i+1]);
				}
			}
		}
	}
	
	public static List<Vector3> Ricochet(Vector3 start, Vector3 direction, Vector3 targetPoint, float threshold, float maxDistance, int layerMask, int bounce){
		//Gone too far: Terminate
		if(maxDistance <= 0 || bounce >= 3)
			return null;
		//Attempt raycast
		RaycastHit rayHit;
		if(Physics.Raycast(start, direction, out rayHit, maxDistance, layerMask)){
			//Raycast hit a wall:
			Vector2 line1 = new Vector2(start.x, start.z);
			Vector2 line2 = new Vector2(rayHit.point.x, rayHit.point.z);
			if(MinDistanceFromSegmentToPoint(line1, line2, new Vector2(targetPoint.x, targetPoint.z)) <= threshold){
				//But we got close enough on the way
				List<Vector3> result = new List<Vector3>();
				result.Add(start);
				result.Add(targetPoint);
				return result;
			}
			else{
				//Did not get close enough on the way: continue ricocheting
				List<Vector3> result = Ricochet(rayHit.point, Vector3.Reflect(direction, rayHit.normal), targetPoint, threshold, (maxDistance - (start - rayHit.point).magnitude)*0.8f, layerMask, bounce + 1);
				if(result != null)
					result.Insert(0, start);
				return result;
			}
		}
		else{
			//Raycast did not hit wall
			Vector2 line1 = new Vector2(start.x, start.z);
			Vector3 endpoint = start + direction * maxDistance;
			Vector2 line2 = new Vector2(endpoint.x, endpoint.z);
			if(MinDistanceFromSegmentToPoint(line1, line2, new Vector2(targetPoint.x, targetPoint.z)) <= threshold){
				//But we got close enough to the target
				List<Vector3> result = new List<Vector3>();
				result.Add(start);
				result.Add(targetPoint);
				return result;
			}
			return null;
		}
	}
	
	public static float MinDistanceFromSegmentToPoint(Vector2 line1, Vector2 line2, Vector2 point){
		float vx = line1.x-point.x;
		float vy = line1.y-point.y;
		float ux = line2.x-line1.x;
		float uy = line2.y-line1.y;
		float length = ux*ux+uy*uy;
		float det = (-vx*ux)+(-vy*uy); //if this is < 0 or > length then its outside the line segment
		if(det < 0 || det > length){
			ux=line2.x-point.x;
        	uy=line2.y-point.y;
        	return Mathf.Min(vx*vx+vy*vy, ux*ux+uy*uy);
      	}
		det = ux*vy-uy*vx;
		return (det*det)/length;
	}
}
