using UnityEngine;
using System.Collections;
using System;

public class ContractDock : Dock
{
    public ContractSlot Slot;

    public override Vector2 FloorPosition
    {
        get
        {
            return Slot.FloorPosition;
        }
    }

    public ContractDock(ContractSlot slot) : base(slot, slot.Contract is DeliveryContract ? DockType.In : DockType.Out)
    {        
        Slot = slot;
        OnGrabberDocked = slot.Contract.OnGrabberDocked;
    }
}
