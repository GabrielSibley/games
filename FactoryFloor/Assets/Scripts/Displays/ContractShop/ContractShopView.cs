using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ContractShopView : MonoBehaviour {

	public ContractShopButton[] buttons;

	public int LayoutRows;
	public Vector2 LayoutSpacing;

	public void Display(List<Contract> contracts)
	{
		if(contracts.Count > buttons.Length)
		{
			//TODO: Scroll, or something.
			Debug.LogWarning ("Too many contracts to display");
		}
		for(int i = 0; i < buttons.Length; i++)
		{
			if(i >= contracts.Count)
			{
				buttons[i].Display(null);
			}
			else
			{
				buttons[i].Display(contracts[i]);
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
