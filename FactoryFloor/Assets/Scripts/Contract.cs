using UnityEngine;
using System.Collections;

public class Contract {
    public Crate Crate { get; set; }
    public int Quantity { get; set; }

    public virtual System.Action<Dock, Grabber> OnGrabberDocked { get { return null; } }
}
