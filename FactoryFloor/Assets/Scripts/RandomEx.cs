using UnityEngine;
using System.Collections;
using System.Linq;

public static class RandomEx {

	public static int RandomIndexWeighted(float[] weights)
	{
		float roll = Random.value;
		float invWeightTotal = 1 / weights.Sum();
		float cumulativeWeight = 0;
		for(int i = 0; i < weights.Length; i++)
		{
			cumulativeWeight += weights[i] * invWeightTotal;
			if(roll <= cumulativeWeight)
			{
				return i;
			}
		}
		return weights.Length - 1;
	}
}
