using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {
	
	public int playerID;
	public int health = 5;
	public float fastSpeed;
	public float slowSpeed;
	public Knife heldKnife;
	public HingeJoint grab;
	public Renderer sprite;
	public Texture[] textures;
	public ParticleEmitter[] particles;
	public GameObject deadHand;
	
	public Bounds bounds;
	
	private Vector3 knifeStartingRotation;
	private Quaternion startingRotation;
	
	public int Health
	{
		get{
			return health;
		}
		set{
			health = value;
			sprite.material.mainTexture = textures[Mathf.Clamp(value, 0, 5)];
			if(value <= 0)
			{
				GameObject dead = Instantiate(deadHand, transform.position, transform.rotation) as GameObject;
				dead.GetComponent<Rigidbody>().velocity = MoveVector();
				for(int i = 0; i < particles.Length; i++){
					if(particles[i] != null){
						particles[i].transform.parent = null;
						particles[i].emit = false;
					}
				}
				GameBro.EndGamePlayerDied(playerID);
				Destroy(gameObject);
			}
			else{
				for(int i = 0; i < particles.Length; i++){
					if(particles[i] != null){
						particles[i].emit = i >= value;
					}
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 move = MoveVector() * Time.deltaTime;
		if(move.x + transform.position.x > bounds.max.x){
			move.x = bounds.max.x - transform.position.x;
		}
		if(move.x + transform.position.x < bounds.min.x){
			move.x = bounds.min.x - transform.position.x;
		}
		if(move.y + transform.position.y > bounds.max.y){
			move.y = bounds.max.y - transform.position.y;
		}
		if(move.y + transform.position.y < bounds.min.y){
			move.y = bounds.min.y - transform.position.y;
		}

		transform.Translate(move, Space.World);
		
		if(Winnput.A[playerID] && grab.connectedBody != null)
		{
			grab.connectedBody = null;
			heldKnife.heldBy = null;
			heldKnife = null;
		}
		
		if(heldKnife != null){
			transform.rotation = Quaternion.FromToRotation(knifeStartingRotation, heldKnife.transform.right) * startingRotation;
		}
	}
	
	Vector3 MoveVector()
	{
		Vector3 move = new Vector3(Winnput.Horizontal[playerID], Winnput.Vertical[playerID], 0);
		if(move.sqrMagnitude > 1)
		{
			move = move.normalized;
		}
		move *= (Winnput.B[playerID] ? fastSpeed : slowSpeed);
		return move;
	}
	
	public void HoldKnife(Knife k)
	{
		grab.connectedBody = k.GetComponent<Rigidbody>();
		heldKnife = k;
		k.heldBy = this;
		if(k.lastHeldBy != null && k.lastHeldBy != this && Health < 5){
			Health++;
			Jukebox.PlayHealSfx();
		}
		k.lastHeldBy = this;
		k.timeSinceHeld = 0;
		knifeStartingRotation = k.transform.right;
		startingRotation = transform.rotation;
	}
	
	public void ReleaseKnife()
	{
		grab.connectedBody = null;
		heldKnife.heldBy = null;
		heldKnife.GetComponent<Rigidbody>().angularVelocity *= 2; //More spinning = more fun
		heldKnife = null;
	}
	
}
