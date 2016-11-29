using UnityEngine;
using System.Collections;

public class PanelSwitchButton : MonoBehaviour, IInputReceiver {

    [SerializeField]
    SimulationView.UIPanel Panel;
    [SerializeField]
    SimulationView View;

    public void OnInputDown()
    {
        View.ActivePanel = Panel;
    }
}
