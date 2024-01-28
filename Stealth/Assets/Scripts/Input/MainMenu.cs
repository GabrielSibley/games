using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {
	
	public static List<Item> playerLoadout;
	
	public GUISkin mainMenuSkin;
	public MainMenuMode mode;
	public Item[] store;
	
	public int pointAllowance = 12;
	
	private int[] itemQuantity;
	
	void Awake(){
		itemQuantity = new int[store.Length];
	}
	
	void OnGUI(){
		int pointsRemaining = pointAllowance;
		for(int i = 0; i < store.Length; i++){
			pointsRemaining -= itemQuantity[i] * store[i].slots;
		}
		GUI.skin = mainMenuSkin;
		
		if(mode == MainMenuMode.Title){
			GUI.Label(new Rect(0, 240, Screen.width, 120), "TACTICAL DRAGON ALPHA", mainMenuSkin.GetStyle("Title Text"));
	
			if(GUI.Button(new Rect(Screen.width/2 - 80, 360, 160, 20),"PLAY")){
				mode = MainMenuMode.Loadout;
				
			}
			if(GUI.Button(new Rect(Screen.width/2 - 80, 390, 160, 20),"QUIT")){
				Application.Quit();
			}
		}
		else if (mode == MainMenuMode.Loadout){
			GUI.Label(new Rect(0, 240, Screen.width, 120), "Select Loadout", mainMenuSkin.GetStyle("Large Fancy Text"));
			GUI.BeginGroup(new Rect(Screen.width / 2 - 280, 360, 480, 1000));
			GUI.Label(new Rect(0, 0, 180, 20), "Item");
			GUI.Label(new Rect(180, 0, 80, 20), "Qty");
			GUI.Label(new Rect(280, 0, 40, 20), "Cost");
			int i = 0;
			for(; i < store.Length; i++){
				GUI.Label(new Rect(0, i*20+20, 180, 20), store[i].displayName);
				GUI.Label(new Rect(180, i*20+20, 40, 20), "x " + itemQuantity[i]);
				if(store[i].slots <= pointsRemaining && GUI.Button(new Rect(220, i*20+20, 20, 20), "+")){
					itemQuantity[i]++;
				}
				if(itemQuantity[i] > 0 && GUI.Button(new Rect(240, i*20+20, 20, 20), "-")){
					itemQuantity[i]--;
				}
				GUI.Label(new Rect(280, i*20+20, 40, 20), store[i].slots.ToString());
			}
			GUI.Label(new Rect(0, i*20+20, 480, 20), "Points Remaining: " + pointsRemaining);
			i++;
			if(GUI.Button(new Rect(0, i*20+20, 240, 20), "BACK"))
				mode = MainMenuMode.Title;
			if(GUI.Button(new Rect(240, i*20+20, 240, 20), "GO")){
				playerLoadout = new List<Item>();
				for(int j = 0; j < store.Length; j++){
					for(int k = 0; k < itemQuantity[j]; k++){
						playerLoadout.Add(store[j]);
					}
				}
				Debug.Log("Item loadout set");
				Application.LoadLevel(1); //Randomly generated level
			}
			GUI.EndGroup();
		}
		
	}
}

public enum MainMenuMode{
	Title,
	Loadout
}
