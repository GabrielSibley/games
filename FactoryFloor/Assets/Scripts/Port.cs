using UnityEngine;
using System.Collections;

public enum PortType {In, Out};

public abstract class Port {

	public PortType Type;
	public Pipe Pipe; //modify this with Pipe.From and Pipe.To
	public abstract bool IsReal { get; }

	public Vector2 Offset; //Tile-space offset from machine origin

	public abstract Vector2 WorldPosition{
		get;
	}

	public Port(PortType t)
	{
		Type = t;
	}

	public abstract bool TryGetCrate(out Crate crate);
	public abstract bool TryPutCrate(Crate crate);
}
