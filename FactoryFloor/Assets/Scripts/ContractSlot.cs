using UnityEngine;
using System.Collections;

public class ContractSlot {

	public Vector2 FloorPosition { get; set; }
    public Contract Contract
    {
        get { return contract; }
        set
        {
            contract = value;
            Dock = value != null ? new ContractDock(this) : null;
            //TODO: Destroy old dock somehow?
        }
    }
    private Contract contract;
    public ContractDock Dock;
}
