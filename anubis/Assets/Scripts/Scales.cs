using UnityEngine;
using System.Collections;

public class Scales : MonoBehaviour {

	public float[] rotation = new float[]{0, 15, 25, 35};
	public Transform bar;
	public Transform[] panPivots;

	public void ShowScores(int[] scores)
	{
		int d = scores[0] - scores[1];
		float r = d < 0 ? -rotation[-d] : rotation[d];
		bar.rotation = Quaternion.Euler(0, 0, r);
		foreach(Transform t in panPivots)
		{
			t.rotation = Quaternion.identity;
		}
	}
}
