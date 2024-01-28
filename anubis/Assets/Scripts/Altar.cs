using UnityEngine;
using System.Collections;

public class Altar : MonoBehaviour {

	public SpriteRenderer bigFire;
	public float burnTime = 2;

	private float fireStarted = float.MinValue;

	public void OnSacrifice()
	{
		fireStarted = Time.time;
	}

	private void Update()
	{
		bigFire.enabled = Time.time - fireStarted < burnTime;
	}

}
