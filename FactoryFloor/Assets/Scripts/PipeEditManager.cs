using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PipeEditManager : MonoBehaviour, IInputListener {

    public Simulation Simulation;

	public float maxSnapDistance = 128;

	private PipeDisplay editedPipe;
    private HashSet<IPipeDisplayAnchor> pipeAnchors = new HashSet<IPipeDisplayAnchor>();
    private HashSet<PipeDisplay> pipeDisplays = new HashSet<PipeDisplay>();

    private void Awake()
    {
        InputManager.InputListeners.Add(this);
        DockDisplay.DockDisplayChanged += OnDockAnchorChange;
    }


    private void OnDestroy()
	{
		InputManager.InputListeners.Remove (this);
        DockDisplay.DockDisplayChanged -= OnDockAnchorChange;
    }

    private void OnDockAnchorChange(IPipeDisplayAnchor anchor, bool visible)
    {
        if(visible)
        {
            pipeAnchors.Add(anchor);
        }
        else
        {            
            pipeAnchors.Remove(anchor);
            if (editedPipe != null && (editedPipe.From == anchor || editedPipe.To == anchor))
            {
                CancelPipeSetup();
            }
        }
    }

	private void Update()
	{
		if(editedPipe != null)
		{
			editedPipe.Display();
		}
	}

    public IPipeDisplayAnchor GetAnchorForDock(Dock dock)
    {
        //TODO: Make not O(N)
        foreach(var anchor in pipeAnchors)
        {
            if(anchor.Dock == dock)
            {
                return anchor;
            }
        }
        return null;
    }


    public bool OnInputDown()
	{
        if(Simulation == null)
        {
            return false;
        }
		if(GameMode.Current == GameMode.Mode.SelectPipe)
		{
			//If no pipe being edited
			if(editedPipe == null)
			{
                //Get closest port
                IPipeDisplayAnchor targetAnchor = GetClosestAnchorToCursorWithQuality(x => true);
				if(targetAnchor != null)
				{
					if(targetAnchor.Dock.Pipe != null)
					{
						//Edit existing pipe
                        foreach(PipeDisplay pipeDisplay in pipeDisplays)
                        {                            
                            if(pipeDisplay.Pipe.From == targetAnchor.Dock)
                            {
                                editedPipe = pipeDisplay;
                                editedPipe.FromOverride = new DraggedPipeAnchor();
                                editedPipe.Pipe.From = null;
                                break;
                            }
                            if(pipeDisplay.Pipe.To == targetAnchor.Dock)
                            {
                                editedPipe = pipeDisplay;
                                editedPipe.ToOverride = new DraggedPipeAnchor();
                                editedPipe.Pipe.To = null;
                                break;
                            }
                        }
					}
					else
					{
                        //Create new pipe (from/to) that port to input point
                        Pipe pipe = new Pipe(Simulation);
                        editedPipe = Instantiate(PrefabManager.PipeDisplay);
                        pipeDisplays.Add(editedPipe);
                        editedPipe.Pipe = pipe;
						if(targetAnchor.Dock.Type == DockType.In)
						{
							editedPipe.FromOverride = new DraggedPipeAnchor();							
                            pipe.To = targetAnchor.Dock;
						}
						else
						{
                            pipe.From = targetAnchor.Dock;                            
							editedPipe.ToOverride = new DraggedPipeAnchor();
						}
                    }
					editedPipe.Display();
				}
			}
			else
			{
				//get closest dock of opposite type from still-attached dock
				Dock attachedDock = (editedPipe.Pipe.From ?? editedPipe.Pipe.To);
                IPipeDisplayAnchor targetDisplay = GetClosestAnchorToCursorWithQuality(x => x.Dock.Pipe == null && x.Dock.Type != attachedDock.Type);                
				//if pipe is not connecting source to itself
				if(targetDisplay != null
				   && targetDisplay.Dock.Source != attachedDock.Source
                )
				{
                    Dock targetDock = targetDisplay.Dock;
                    //setup pipe
                    if (attachedDock == editedPipe.Pipe.From)
					{
						editedPipe.Pipe.To = targetDock;
                        editedPipe.ToOverride = null;
					}
					else
					{
						editedPipe.Pipe.From = targetDock;
                        editedPipe.FromOverride = null;
					}
					if(editedPipe.Pipe.GrabberCount == 0)
					{
						editedPipe.Pipe.AddGrabber();
					}
					editedPipe.Display();
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
        if(editedPipe != null)
        {
            pipeDisplays.Remove(editedPipe);         
            editedPipe.Pipe.Destroy();
            Destroy(editedPipe.gameObject);
            editedPipe = null;
        }
	}

	private IPipeDisplayAnchor GetClosestAnchorToCursorWithQuality(Predicate<IPipeDisplayAnchor> condition)
	{
        IPipeDisplayAnchor resultAnchor = null;
		float minDist = maxSnapDistance;
		foreach(IPipeDisplayAnchor anchor in pipeAnchors)
		{
			if(condition(anchor))
			{
				float dist = Vector2.Distance (anchor.WorldPos, InputManager.InputWorldPos);
				if(dist < minDist)
				{
					minDist = dist;
					resultAnchor = anchor;
				}
			}
		}
		return resultAnchor;
	}
}
