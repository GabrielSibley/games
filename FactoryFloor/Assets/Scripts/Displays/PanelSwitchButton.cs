using UnityEngine;
using System.Collections;

public class PanelSwitchButton : MonoBehaviour, IInputReceiver {

    [SerializeField]
    SimulationView.UIPanel Panel;
    [SerializeField]
    SimulationView View;
    [SerializeField]
    Sprite UpSprite, DownSprite;
    [SerializeField]
    SpriteRenderer Renderer;

    public void OnInputDown()
    {
        View.ActivePanel = Panel;
    }

    void Update()
    {
        if(View.ActivePanel == Panel)
        {
            Renderer.sprite = DownSprite;
        }
        else
        {
            Renderer.sprite = UpSprite;
        }
    }
}
