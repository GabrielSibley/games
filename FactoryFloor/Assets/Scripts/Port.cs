using UnityEngine;
using System.Collections;

public enum PortType {In, Out};

public abstract class Port {

	public PortType Type;
	public Pipe Pipe;

	public abstract Vector2 WorldPosition{
		get;
	}

	public Port(PortType t)
	{
		Type = t;
	}
}
