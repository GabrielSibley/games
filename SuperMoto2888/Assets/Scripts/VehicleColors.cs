using UnityEngine;
using System.Collections;

public class VehicleColors : MonoBehaviour {

	public Color[] colors;

	public Color GetRandom(){
		return colors[Random.Range (0, colors.Length)];
	}
}
