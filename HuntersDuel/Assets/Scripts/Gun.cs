using UnityEngine;
using System.Collections;

public enum BulletState{
	Empty,
	Live,
	Fired
}

public abstract class Gun : MonoBehaviour {

	public abstract int Damage { get; }
	public abstract bool Fire(BasicMove player, float angle); //Returns true if gun actually fires

	public abstract void ManipulateUp();
	public abstract void ManipulateDown();
	public abstract void ManipulateLeft();
	public abstract void ManipulateRight();
}
