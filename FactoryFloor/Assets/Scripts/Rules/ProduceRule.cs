using UnityEngine;
using System.Collections;

//Produces an infinite supply of the given crate.
//Cannot take crates.
public class ProduceRule : IMachineRule {

	public Crate Production;

	public int NumInPorts{ get { return 0; } }
	public int NumOutPorts{ get { return 1; } }

	private FourFeatureDisplay display;

	public ProduceRule(Crate produces)
	{
		Production = produces;
	}

	public bool TryPutCrate(Port port, Crate crate)
	{
		return false;
	}
	public bool TryGetCrate(Port port, out Crate crate)
	{
		crate = new Crate(Production);
		return true;
	}
	public IMachineRuleDisplay GetDisplay()
	{
		return Object.Instantiate(PrefabManager.ProduceRuleDisplay) as ProduceRuleDisplay;
	}

	public IMachineRule FreshCopy()
	{
		return new ProduceRule(new Crate(Production));
	}
}
