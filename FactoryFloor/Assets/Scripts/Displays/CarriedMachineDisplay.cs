using UnityEngine;
using System.Collections;

public class CarriedMachineDisplay : MonoBehaviour {

    public MachineMiniLayoutDisplay smallDisplay;
    public MachineDisplay largeDisplay;

    public float swapX;

    public void Display(Machine machine)
    {
        if(machine == null)
        {
            gameObject.SetActive(false);
            return;
        }
        else if(Input.GetMouseButtonDown(1))
        {
            SimulationView.Instance.CarriedMachine = null;
            return;
        }
        else
        {
            gameObject.SetActive(true);
        }

        Vector3 pos = InputManager.InputWorldPos;
        pos.z = ZLayer.CarriedMachine;
        transform.position = pos;
        if(transform.position.x >= swapX)
        {
            //check if could drop machine on floorspace
            Vector2i targetOrigin = new Vector2i(FloorView.WorldToFloorPoint(pos));
            if (machine.CanMoveToOrigin(targetOrigin))
            {
                //snap display to that space
                Vector3 snapPos = FloorView.FloorToWorldPoint(targetOrigin, FloorViewSpace.TileCenter);
                snapPos.z = ZLayer.CarriedMachine;
                transform.position = snapPos;


                if (Input.GetMouseButtonUp(0))
                {
                    machine.AddToFloor(targetOrigin);
                    SimulationView.Instance.CarriedMachine = null; //Causes refresh of display, early out!
                    return;
                }
            }           

            //set large display active
            largeDisplay.gameObject.SetActive(true);
            largeDisplay.Display(machine);
            smallDisplay.gameObject.SetActive(false);           
        }
        else
        {
            //set small display active
            smallDisplay.gameObject.SetActive(true);
            smallDisplay.Display(machine);
            largeDisplay.gameObject.SetActive(false);
        }
    }
}
