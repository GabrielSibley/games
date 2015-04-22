using UnityEngine;
using System.Collections;

public class Bike : MonoBehaviour {

	public float speed = 56; //m/sec
	public float accel = 5;
	public float strafeSpeed = 80;
	public float heightMax, heightMin;
	public float widthMax;
	public float heightLimitHardness;
	public GameObject shatter;
	public bool dead;
	public TextMesh speedText, distanceText;

	private float distance;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(!dead){
			speed += accel * Time.deltaTime;
			Vector3 steerInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis ("Vertical"), 0);
			if(transform.position.y > heightMax){
				steerInput.y += (heightMax - transform.position.y) * heightLimitHardness;
			}
			if(transform.position.y < heightMin){
				steerInput.y += (heightMin - transform.position.y) * heightLimitHardness;
			}
			if(transform.position.x > widthMax){
				steerInput.x += (widthMax - transform.position.x) * heightLimitHardness;
			}
			if(transform.position.x < -widthMax){
				steerInput.x += (-widthMax - transform.position.x) * heightLimitHardness;
			}
			rigidbody.position += steerInput * strafeSpeed * Time.deltaTime;
			distance += speed * Time.deltaTime;
		}
		speedText.text = string.Format ("{0:#0} KPH", speed*3.6f);
		distanceText.text = string.Format ("{0:0.00} KM", distance/1000);
	}

	void FixedUpdate(){
		if(dead){
			rigidbody.velocity = Vector3.zero;
		}
		else{
			rigidbody.velocity = Vector3.forward * speed;
		}
	}

	void OnCollisionEnter(){
		dead = true;
		shatter.SetActive(true);
	}
}
