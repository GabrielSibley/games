using UnityEngine;
using System.Collections;

[PrefabManager]
public class DockDisplay : MonoBehaviour, IPipeDisplayAnchor {

    public static event System.Action<DockDisplay, bool> DockDisplayChanged;

	public Sprite InPortSprite;
	public Sprite InPortFirstSprite;
	public Sprite InPortLastSprite;
	public Sprite OutPortSprite;
	public Sprite OutPortFirstSprite;
	public Sprite OutPortLastSprite;

	public SpriteRenderer SpriteRenderer;

	public Dock Dock {
        get { return _dock; }
        set {
            _dock = value;
            if (DockDisplayChanged != null)
            {
                DockDisplayChanged(this, value != null);
            }
        }
    }
    private Dock _dock;

    protected void OnEnable()
    {
        if (DockDisplayChanged != null)
        {
            DockDisplayChanged(this, true);
        }
    }

    protected void OnDisable()
    {
        if (DockDisplayChanged != null)
        {
            DockDisplayChanged(this, false);
        }
    }

    public Vector3 WorldPos
    {
        get { return transform.position; }
    }

	public void Display()
	{
		if(Dock == null)
		{
			Debug.LogError ("Trying to display null port", this);
			SpriteRenderer.sprite = null;
            return;
		}
		if(Dock.Type == DockType.In)
		{
			if(Dock.Effect == DockEffect.First){
				SpriteRenderer.sprite = InPortFirstSprite;
			}
			else if(Dock.Effect == DockEffect.Last){
				SpriteRenderer.sprite = InPortLastSprite;
			}
			else{
				SpriteRenderer.sprite = InPortSprite;
			}
		}
		else
		{
			if(Dock.Effect == DockEffect.First){
				SpriteRenderer.sprite = OutPortFirstSprite;
			}
			else if(Dock.Effect == DockEffect.Last){
				SpriteRenderer.sprite = OutPortLastSprite;
			}
			else{
				SpriteRenderer.sprite = OutPortSprite;
			}            
        }
        SpriteRenderer.color = Dock.Pipe == null ? Color.grey : Color.white;
        bool appearHigh = SimulationView.Instance.CarriedMachine != Dock.Source //Dock is not actually being carried
            && Dock.Pipe != null                                                //Dock has a pipe hooked to it
            && (Dock.Pipe.From == Dock || Dock.Pipe.To == Dock);                //That pipe is hooked to us
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, appearHigh ? ZLayer.DockOffset + ZLayer.CarriedOffset : ZLayer.DockOffset);
    }
}
