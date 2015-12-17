using UnityEngine;
using System.Collections;

public class SupplyContract : Contract {

	public Crate Supplies { get { return supplies; } }
	public int Quantity { get { return quantity; } }
	public int LotPrice { get { return lotPrice; } }

	SupplyContractDisplay display;
	Crate supplies;
	int quantity;
	int lotPrice;
}
