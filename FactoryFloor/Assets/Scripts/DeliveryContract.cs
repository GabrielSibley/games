using UnityEngine;
using System.Collections;

public class DeliveryContract : Contract {

    public override System.Action<Dock, Grabber> OnGrabberDocked { get { return GrabberDocked; } }

    void GrabberDocked(Dock dock, Grabber grabber)
    {
        if (grabber.HeldCrate != null && grabber.HeldCrate.Features.Count == Crate.Features.Count)
        {
            for (int i = 0; i < grabber.HeldCrate.Features.Count; i++)
            {
                if (grabber.HeldCrate.Features[i] != Crate.Features[i])
                {
                    return;
                }
            }
            Quantity--;
            grabber.Dispatch(null, dock);
        }
    }
}
