using UnityEngine;
using System.Collections;

public interface IPipeDisplayAnchor {

	Vector3 WorldPos { get; }
    Dock Dock { get; }
}
