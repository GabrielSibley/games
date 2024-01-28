using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class RandomEx {

	public static int RandomIndexWeighted(IList<float> weights)
	{
		float roll = Random.value;
		float invWeightTotal = 1 / weights.Sum();
		float cumulativeWeight = 0;
		for(int i = 0; i < weights.Count; i++)
		{
			cumulativeWeight += weights[i] * invWeightTotal;
			if(roll <= cumulativeWeight)
			{
				return i;
			}
		}
		return weights.Count - 1;
	}
}
