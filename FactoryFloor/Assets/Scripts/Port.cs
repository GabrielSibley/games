using UnityEngine;
using System.Collections;

public enum PortType {In, Out};

public abstract class Port {

	public PortType Type;
	public Pipe Pipe; //modify this with Pipe.From and Pipe.To
	public abstract bool IsReal { get; }

	public abstract Vector2 WorldPosition{
		get;
	}

	public Port(PortType t)
	{
		Type = t;
	}
}
