using UnityEngine;
using System.Collections;

public class Tracer : MonoBehaviour {
	
	private float alpha;
	
	public void Fire(Vector3 startPosition, Vector3 endPosition){
		//Graphics
		alpha = 0.3f;
		GetComponent<LineRenderer>().SetPosition(0, startPosition);
		GetComponent<LineRenderer>().SetPosition(1, endPosition);
	}
	
	void Update(){
		alpha -= 2*Time.deltaTime;
		Color c = new Color(1,1,1,alpha);
		GetComponent<LineRenderer>().SetColors(c, c);
		if(alpha <= 0)
			Destroy(this.gameObject);
	}
}
