using UnityEngine;
using System.Collections;

/** Event for a crate being transferred from one crate
 *  holder to another with no particular flair.*/
public class CrateMoveEvent : SimEvent {

	public Crate Crate;
	public ICrateHolder From;
	public ICrateHolder To;

	public override void Execute()
	{
		From.RemoveCrate(Crate);
		To.AddCrate(Crate);
	}
}
