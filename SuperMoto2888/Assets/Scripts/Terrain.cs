using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//START TIME 12:35 AM
//BREAK TIME: 1:57 AM (1 hr 22m used)
//RESUME: 5:30 PM
public class Terrain : MonoBehaviour {

	public Transform bike;
	public GameObject buildingGroupPrefab;
	public GameObject carPrefab;

	public float chunkSpacing = 100;
	public float drawDistance = 1000;
	public int lastChunk;

	public float trafficHorizSpace, trafficVertSpace;
	public float crossStreetOffset;
	public float trafficDensity;
	public float trafficHeight;

	private Stack<GameObject> carPool = new Stack<GameObject>();
	private Stack<GameObject> chunkPool = new Stack<GameObject>();

	private List<GameObject> cars = new List<GameObject>();
	private List<GameObject> chunks = new List<GameObject>();

	void Start(){

	}

	// Update is called once per frame
	void Update () {
		while(lastChunk * chunkSpacing < bike.position.z + drawDistance){
			SpawnChunk(new Vector3(0, 0, lastChunk * chunkSpacing));
			lastChunk++;
		}
		//Monitor recyclables
		for(int i = 0; i < cars.Count; i++){
			if(cars[i].transform.position.z < bike.transform.position.z){
				carPool.Push (cars[i]);
				cars.RemoveAt (i);
			}
			//cross traffic recycle
			else if(cars[i].transform.position.x > chunkSpacing/2){
				cars[i].transform.position += new Vector3(-chunkSpacing, 0, 0);
			}
			else if(cars[i].transform.position.x < -chunkSpacing/2){
				cars[i].transform.position += new Vector3(chunkSpacing, 0, 0);
			}
		}

		for(int i = 0; i < chunks.Count; i++){
			if(chunks[i].transform.position.z < bike.transform.position.z){
				chunkPool.Push (chunks[i]);
				chunks.RemoveAt (i);
			}
		}

		if(bike.transform.position.z > 5000){
			//move everything back 5k
			lastChunk -= (int)(5000/chunkSpacing);
			bike.transform.position -= Vector3.forward * 5000;
			foreach(GameObject obj in chunks){
				obj.transform.position -= Vector3.forward * 5000;
			}
			foreach(GameObject obj in cars){
				obj.transform.position -= Vector3.forward * 5000;
			}
		}
	}

	void SpawnChunk(Vector3 pos){
		GameObject chunk = GetChunk();
		chunk.transform.position = pos;
		chunks.Add (chunk);

		float volume = trafficHorizSpace * trafficVertSpace * chunkSpacing;
		int carsToSpawn = (int)(volume * trafficDensity);
		for(int i = 0; i < carsToSpawn; i++){
			GameObject car = GetCar();
			cars.Add (car);
			bool crossTraffic = Random.value < 0.5f;
			Vector3 origOffset = new Vector3(Random.Range(-trafficHorizSpace/2, trafficHorizSpace/2),
			                             trafficHeight + Random.Range (-trafficVertSpace/2, trafficVertSpace/2),
			                             Random.Range (-chunkSpacing/2, chunkSpacing/2)
			                             );
			Vector3 offset;
			if(crossTraffic){
				offset = Quaternion.Euler (0, 90, 0) * origOffset;
				offset.z += crossStreetOffset;
			}
			else{
				offset = origOffset;
			}
			car.transform.position = pos + offset;
			if(origOffset.x < 0){
				car.transform.rotation = Quaternion.Euler (0, 180, 0);
			}
			else{
				car.transform.rotation = Quaternion.identity;
			}
			if(crossTraffic){
				car.transform.Rotate(0, 90, 0);
			}
		}
	}

	GameObject GetChunk(){
		if(chunkPool.Count == 0){
			chunkPool.Push(Instantiate(buildingGroupPrefab, Vector3.zero, Quaternion.identity) as GameObject);
		}
		return chunkPool.Pop();
	}

	GameObject GetCar(){
		if(carPool.Count == 0){
			carPool.Push(Instantiate(carPrefab, Vector3.zero, Quaternion.identity) as GameObject);
		}
		return carPool.Pop();
	}
}
