using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Crate {

	public const int MaxFeatures = 4;

	public List<CrateFeature> Features = new List<CrateFeature>(MaxFeatures);

	public Crate()
	{
	}

	public Crate(Crate cloneOf)
	{
		Features.AddRange (cloneOf.Features);
	}
}
