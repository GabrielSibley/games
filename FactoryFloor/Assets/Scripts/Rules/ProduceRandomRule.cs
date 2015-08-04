using UnityEngine;
using System.Collections;

//Produces an infinite supply of random crates.
//Cannot take crates.
public class ProduceRandomRule : IMachineRule {

	public int NumInPorts{ get { return 0; } }
	public int NumOutPorts{ get { return 1; } }

	public bool TryPutCrate(Port port, Crate crate)
	{
		return false;
	}
	public bool TryGetCrate(Port port, out Crate crate)
	{
		crate = new Crate();
		int numFeatures = Random.Range (1, Crate.MaxFeatures+1);
		for(int i = 0; i < numFeatures; i++)
		{
			crate.Features.Add (new CrateFeature(
				Random.Range (0, CrateFeature.NumNormalColors),
				Random.Range (0, CrateFeature.NumNormalShapes)
				)
			);
		}
		return true;
	}
	public void UpdateDisplay(Machine machine, Vector2 machinePosition)
	{
		//no display
	}

	public IMachineRule FreshCopy()
	{
		return new ProduceRandomRule();
	}
}
