using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//A chain of transfers which are sequentially dependent
public class CrateTransferSequence {

	public Node First;

	public class Node{
		CrateTransfer Transfer;
		List<Node> Dependents;
	}
}
