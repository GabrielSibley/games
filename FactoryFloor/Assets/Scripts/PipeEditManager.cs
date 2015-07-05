using UnityEngine;
using System.Collections;
using System;

public class PipeEditManager : MonoBehaviour, IInputListener {

	public float maxSnapDistance = 128;

	private Pipe editedPipe;

	private void Awake()
	{
		InputManager.InputListeners.Add(this);
	}

	private void OnDestroy()
	{
		InputManager.InputListeners.Remove (this);
	}

	private void Update()
	{
		if(editedPipe != null)
		{
			editedPipe.UpdateDisplay();
		}
	}

	public bool OnInputDown()
	{
		if(GameMode.Current == GameMode.Mode.SelectPipe)
		{
			//If no pipe being edited
			if(editedPipe == null)
			{
				//Get closest machine port
				MachinePort closestPort = GetClosestMachinePortToCursorWithQuality(x => true);
				if(closestPort != null)
				{
					if(closestPort.Pipe != null)
					{
						//Edit existing pipe
						editedPipe = closestPort.Pipe;
						if(closestPort == editedPipe.From)
						{
							editedPipe.From = new MouseFollowingPort(PortType.Out);
						}
						else
						{
							editedPipe.To = new MouseFollowingPort(PortType.In);
						}
					}
					else
					{
						//Create new pipe (from/to) that port to mousefollowport
						editedPipe = new Pipe();
						if(closestPort.Type == PortType.In)
						{
							editedPipe.From = new MouseFollowingPort(PortType.Out);
							editedPipe.To = closestPort;
						}
						else
						{
							editedPipe.From = closestPort;
							editedPipe.To = new MouseFollowingPort(PortType.In);
						}
					}
					editedPipe.UpdateDisplay();
				}
			}
			else
			{
				//get closest machine port of opposite type
				MachinePort realPort = (editedPipe.From as MachinePort ?? editedPipe.To as MachinePort);
				MachinePort closestPort = GetClosestMachinePortToCursorWithQuality(x => x.Pipe == null && x.Type != realPort.Type);
				//if pipe is not connecting machine to itself
				if(closestPort != null
				   && closestPort.Machine != realPort.Machine
				)
				{
					//setup pipe
					if(realPort == editedPipe.From)
					{
						editedPipe.To = closestPort;
					}
					else
					{
						editedPipe.From = closestPort;
					}
					if(editedPipe.GrabberCount == 0)
					{
						editedPipe.AddGrabber();
					}
					editedPipe.UpdateDisplay();
					editedPipe = null;
				}
				else
				{
					CancelPipeSetup();
				}
			}
			
		}
		return false;
	}

	private void CancelPipeSetup()
	{
		editedPipe.Destroy();
		editedPipe = null;
	}

	private MachinePort GetClosestMachinePortToCursorWithQuality(Predicate<MachinePort> condition)
	{
		MachinePort closestPort = null;
		float minDist = maxSnapDistance;
		foreach(Machine mach in Machine.PlacedMachines)
		{
			foreach(MachinePart part in mach.Parts)
			{
				foreach(MachinePort port in part.Ports)
				{
					if(condition(port))
					{
						float dist = Vector2.Distance (port.WorldPosition, InputManager.InputWorldPos);
						if(dist < minDist)
						{
							minDist = dist;
							closestPort = port;
						}
					}
				}
			}
		}
		return closestPort;
	}
}
