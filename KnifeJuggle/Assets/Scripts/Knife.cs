using UnityEngine;
using System.Collections;

public class Knife : MonoBehaviour {
	
	public const int ActiveLayer = 8;
	public const int PassiveLayer = 9;
	
	public float idealHoldDistance;
	public float regrabTime = 0.05f;
	public Hand heldBy;
	public Hand lastHeldBy;
	public float pitchTweak, volumeTweak;
	public float maxPitch = 3;
	public float minPitch = 0.1f;
	
	public float timeSinceHeld;
	
	void Update()
	{
		float whirlVel = GetComponent<Rigidbody>().velocity.magnitude + GetComponent<Rigidbody>().angularVelocity.magnitude;
		float p = whirlVel * pitchTweak;
		float v;
		if(p < minPitch){
			v = 0;
		}
		else
		{
			p = Mathf.Clamp(p, minPitch, maxPitch);
			v = Mathf.Lerp(0, 1, Mathf.InverseLerp(minPitch, minPitch + 0.1f, p)) * whirlVel *  volumeTweak;
		}
		GetComponent<AudioSource>().volume = v;
		GetComponent<AudioSource>().pitch = p;
	}
	
	void FixedUpdate()
	{
		if(heldBy == null){
			timeSinceHeld += Time.deltaTime;
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		Hand hand = other.gameObject.GetComponent<Hand>();
		if(hand != null && hand != heldBy && (heldBy != null || timeSinceHeld >= regrabTime))
		{
			Vector3 myPos = transform.position;
			Vector3 otherPos = other.transform.position; 
			if(Vector3.Angle(otherPos - myPos, transform.right) < 90){
				Debug.Log("STAB");
				GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
				GetComponent<Rigidbody>().velocity = Vector3.zero;
				hand.Health -= 1;
				Jukebox.PlayStabSfx();
			}
			else if(heldBy == null && hand.GetComponent<HingeJoint>().connectedBody == null){			
				Debug.Log("GRAB");
				transform.position = transform.position + (
					(otherPos - myPos).normalized * (Vector3.Distance(otherPos, myPos) - idealHoldDistance)
				);
				transform.rotation = Quaternion.FromToRotation(Vector3.right, myPos - otherPos);
				hand.HoldKnife(this);				
				hand.GetComponent<HingeJoint>().connectedBody = this.GetComponent<Rigidbody>();
			}
		}
		
		Balloon balloon = other.gameObject.GetComponent<Balloon>();
		if(balloon != null && GetComponent<Rigidbody>().velocity.magnitude > 0.05f)
		{
			Debug.Log("POP");
			GameBro.GetHand(balloon.playerId).Health--;
			Destroy(balloon.gameObject);
			Jukebox.PlayPopSfx();
		}
	}
	
	void OnCollisionEnter(Collision info)
	{
		if(heldBy == null){
			lastHeldBy = null; //Stop ground pickups from giving +health
		}
	}
}
