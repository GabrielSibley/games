using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MachineShopView : MonoBehaviour {

	public MachineShopButton[] buttons;

	public int LayoutRows;
	public Vector2 LayoutSpacing;

	public void Display(List<Machine> machines)
	{
		if(machines.Count > buttons.Length)
		{
			//TODO: Scroll, or something.
			Debug.LogWarning ("Too many machines to display");
		}
		for(int i = 0; i < buttons.Length; i++)
		{
			if(i >= machines.Count)
			{
				buttons[i].Display(null);
			}
			else
			{
				buttons[i].Display(machines[i]);
			}
		}
	}

	[ContextMenu("Layout")]
	public void Layout()
	{
		int y = 0;
		for(int i = 0; i < buttons.Length;){
			for(int x = 0; x < LayoutRows; x++)
			{
				buttons[i].transform.localPosition = new Vector2(x * LayoutSpacing.x, y * LayoutSpacing.y);
				i++;
			}
			y++;
		}
	}
}
