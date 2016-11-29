using UnityEngine;
using System.Collections;

public class SupplyContract : Contract {


    public override System.Action<Dock, Grabber> OnGrabberDocked { get { return GrabberDocked; } }

    void GrabberDocked(Dock dock, Grabber grabber)
    {
        if (grabber.HeldCrate == null)
        {
            Quantity--;
            grabber.Dispatch(new Crate(Crate), dock);
        }
    }
}
