using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Randomx {

	public static T Element<T>(IList<T> list) {
		return list[Index((ICollection<T>)list)];
	}
	
	public static int Index<T>(ICollection<T> collection) {
		return Random.Range(0, collection.Count);
	}
	
	public static int Index(ICollection collection) {
		return Random.Range(0, collection.Count);
	}
	
	public static int Index(int length) {
		return Random.Range(0, length);
	}
	
	// Return a normally distributed number with variance 1.0 and median 0;
	public static float NormalDistribution(){
		float sum = 0;
		for(int i = 0; i < 12; i++){
			sum += Random.value;
		}
		sum -= 6.0f;
		return sum;
	}
	
	// Durstenfeld's in-place Fisher-Yates shuffle
	public static void Shuffle<T>(IList<T> array) {
		for (int n = array.Count; n > 1; --n) {
			int k = Index(n);
			T temp = array[n - 1];
			array[n - 1] = array[k];
			array[k] = temp;
		}
	}
	
	public static Vector2 PointInTriangle(Vector2 pt1, Vector2 pt2, Vector2 pt3){
		float a = Random.value;
		float b = Random.value;
		if(a+b > 1){
			a = 1-a;
			b = 1-b;
		}
		float c = 1-a-b;
		return pt1*a + pt2*b + pt3*c;
	}
	
	public static Vector3 PointInTriangle(Vector3 pt1, Vector3 pt2, Vector3 pt3){
		float a = Random.value;
		float b = Random.value;
		if(a+b > 1){
			a = 1-a;
			b = 1-b;
		}
		float c = 1-a-b;
		return pt1*a + pt2*b + pt3*c;
	}
}
