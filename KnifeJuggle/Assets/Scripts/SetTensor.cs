using UnityEngine;
using System.Collections;

public class SetTensor : MonoBehaviour {
	
	public Vector3 tensorScale;
	public Vector3 comOffset;
	
	void Start () {
		var rigidbody = GetComponent<Rigidbody>();
        //set tensor explodes if rigidbody has constraints applied
        RigidbodyConstraints c = rigidbody.constraints;
        rigidbody.constraints = RigidbodyConstraints.None;
		rigidbody.centerOfMass = GetComponent<Rigidbody>().centerOfMass + Vector3.Scale(transform.localScale, comOffset);
		rigidbody.inertiaTensor = Vector3.Scale(rigidbody.inertiaTensor, tensorScale);
        rigidbody.constraints = c;
	}
}
