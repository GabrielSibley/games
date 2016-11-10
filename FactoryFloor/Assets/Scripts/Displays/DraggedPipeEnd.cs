using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DraggedPipeAnchor : IPipeDisplayAnchor {

    public Vector3 WorldPos {
        get{
            return InputManager.InputWorldPos;
        }
   }

    public Dock Dock { get { return null; } }
}
