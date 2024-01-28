using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class GUIDriver : MonoBehaviour {
	
	Rect mainBox;
	Rect alertArea;
	Rect timeDisplayArea;
	Rect objectivesArea;
	Rect statusArea;
	Rect equipmentArea;
	Rect heldArea;
	Rect inventoryArea;
	Rect descriptionArea;
	Rect playButtonArea;
	bool slowMode;
	
	Rect selectedSquareArea;
	ItemHolder selectedItem;
	int activeHand = 0;
	int actionIndex = 0;
	float timeRightMouseDown;
	Vector3 mousePosAnchor;
	System.Type selectedSubmenuAction;
	
	GUIStyle fancyText;
	GUIStyle largeFancyText;
	GUIStyle hugeFancyText;
	GUIStyle titleText;
	GUIStyle actionTimeText;
	GUIStyle actionNameText;
	
	
	
	public GUISkin customGUI;
	public Texture2D white;
	public Texture2D[] speedIcons;
	public Color healthBarColor = new Color(0.9f, 0.05f, 0.05f);
	public Color weightBarColor = new Color(0.05f, 0.05f, 1);
	public Color alertBarColor = new Color(0.8f, 0, 0);
	public Color evadeBarColor = new Color(0.95f, 0.55f, 0.02f);
	public Color warningBarColor = new Color(0.9f, 0.9f, 0);
	public Color cautionBarColor = new Color(0.4f, 0.7f, 0.1f);
	public Color noAlertBarColor = new Color(0.2f, 0.7f, 0.3f);
	public Color actionOK = new Color(0.1f, 0.1f, 0.8f);
	public Color actionWarn = new Color(0.6f, 0.6f, 0.1f);
	public Color actionDull = new Color(0.4f, 0.4f, 0.4f);
	public Color actionCancel = new Color(1, 0, 0);
	public Color autonomousActionBarColor = new Color(0.2f, 0.7f, 0.05f);
	
	void Start(){
		FitResolution();
		fancyText = customGUI.GetStyle("Fancy Text");
		titleText = customGUI.GetStyle("Title Text");	
		largeFancyText = customGUI.GetStyle("Large Fancy Text");
		hugeFancyText = customGUI.GetStyle("Very Large Fancy Text");
		actionTimeText = customGUI.GetStyle("Action Time Text");
		actionNameText = customGUI.GetStyle("Action Name Text");
	}
	
	void FitResolution(){
		mainBox = new Rect(0, Screen.height - 70, Screen.width, 70);
		alertArea = new Rect(0, 0, 140, 100);
		objectivesArea = new Rect(Screen.width-140, 0, 140, 100);
		statusArea = new Rect(Screen.width - 200 , mainBox.y - 70, 196, 64);
		//equipmentArea = new Rect(statusArea.xMax + 3, actionArea.yMax + 3, 64*2+3, 64*2+3);
		heldArea = new Rect(mainBox.x, mainBox.y - 66, 80*2 + 3*1, 64);
		inventoryArea = new Rect(mainBox.x, mainBox.y, 80*12 + 3*11, 67);
		//descriptionArea = new Rect(mainBox.x, inventoryArea.yMax + 3, mainBox.width, mainBox.height - inventoryArea.yMax);
	}
	
	void Update(){
		//Slow-time
		if(Input.GetKeyDown(KeyCode.Tab)){
			slowMode = !slowMode;
			if(slowMode){
				Time.timeScale = 0.25f;
				SoundManager.soundIndex = 1;
			}
			else{
				Time.timeScale = 1;
				SoundManager.soundIndex = 3;
			}
		}
		
		Player player = Player.m_instance;
		
		//Action selection
		if(Input.GetKeyDown(KeyCode.Q)){
			actionIndex += 1;
		}
		
		//Left mouse
		if(Input.GetMouseButton(0) && selectedItem != player.Hands[0]){
			PerformAvailableAction(player, player.Hands[0]);
		}
		//Right mouse
		if(Input.GetMouseButton(1) && selectedItem != player.Hands[1]){
			PerformAvailableAction(player, player.Hands[1]);
		}
		
		if(Input.GetKeyDown(KeyCode.R)){
			//Combine items
			PotentialActions comboActions = player.Hands[1].ItemActions(player.Hands[0]);
			if(comboActions.Count > 0){
				System.Object[] parameters = new System.Object[]{Player.m_instance, player.Hands[1], player.Hands[0], player.Hands[1].held};
				PotentialAction pa = comboActions.GetAction(0);
				if(pa.status != ActionStatus.Error)
					pa.actionType.GetConstructor(Action.s_invokeTypes).Invoke(parameters);
			}
		}
		
		//Toggle active hand hand
		if(Input.GetMouseButtonDown(2)){
			if(activeHand == 0)
				activeHand = 1;
			else
				activeHand = 0;
		}
		selectedItem = null;
	}
	
	protected void PerformAvailableAction(Player player, ItemHolder activeHand){
		if(!activeHand.InUse){
			//Inventory interaction
			if(selectedItem != null){
				if(!selectedItem.InUse){
					//Use one item on the other, if both slots not empty
					PotentialActions actionList = activeHand.ItemActions(selectedItem);
					if(actionList.Count > 0){
						System.Object[] parameters = new System.Object[]{Player.m_instance, activeHand, selectedItem, activeHand.held};
						PotentialAction pa = actionList.GetAction(actionIndex % actionList.Count);
						actionIndex = 0;
						if(pa.status != ActionStatus.Error)
							pa.actionType.GetConstructor(Action.s_invokeTypes).Invoke(parameters);
					}
				}
			}			
			//World action
			else{
				PotentialActions actionList = activeHand.Actions();
				if(actionList.Count > 0){
					System.Object[] parameters = new System.Object[]{Player.m_instance, activeHand, selectedItem, activeHand.held};
					PotentialAction pa = actionList.GetAction(actionIndex % actionList.Count);
					actionIndex = 0;
					if(pa.status != ActionStatus.Error)
						pa.actionType.GetConstructor(Action.s_invokeTypes).Invoke(parameters);
				}
			}
		}
	}
	
	void OnGUI(){
		GUI.skin = customGUI;
		
		//Backings
		GUI.Box(mainBox,"");
		
		//Alert Status
		GUI.Label(alertArea, "1F", titleText);
		Color barColor = Color.black;
		Rect topBar = new Rect(alertArea.x+1, alertArea.y+1, alertArea.width, 24);
		Rect bottomBar = new Rect(alertArea.x+1, alertArea.yMax - 24, alertArea.width, 24);
		if(LevelLogic.alert == AlertMode.Alert)
			barColor = alertBarColor;
		if(LevelLogic.alert == AlertMode.Evasion)
			barColor = evadeBarColor;
		if(LevelLogic.alert == AlertMode.Warning)
			barColor = warningBarColor;
		if(LevelLogic.alert == AlertMode.Caution)
			barColor = cautionBarColor;
		if(LevelLogic.alert == AlertMode.None)
			barColor = noAlertBarColor;
		GUI.color = barColor;
		GUI.DrawTexture(topBar, white);
		GUI.DrawTexture(bottomBar, white);
		GUI.color = Color.white;
		if(LevelLogic.alert == AlertMode.Alert)
			GUI.Label(topBar, "ALERT", largeFancyText);
		if(LevelLogic.alert == AlertMode.Evasion){
			GUI.Label(topBar, "EVADE", largeFancyText);
			GUI.Label(bottomBar, Utility.FormatTime(60-LevelLogic.timeSincePlayerSighted), largeFancyText);
		}
		if(LevelLogic.alert == AlertMode.Warning)
			GUI.Label(topBar, "WARNING", largeFancyText);
		if(LevelLogic.alert == AlertMode.Caution){
			GUI.color = Color.black;
			GUI.Label(topBar, "CAUTION", largeFancyText);
			GUI.color = Color.white;
		}
		if(LevelLogic.alert == AlertMode.None){
			GUI.color = Color.black;
			GUI.Label(topBar, "FLOOR", largeFancyText);
			GUI.color = Color.white;
		}
		
		//Objectives
		GUI.Box(objectivesArea, "Collect Secrets " + Player.Objectives.FindAll(delegate (ItemObjective io){return io.m_completed;}).Count + "/" + Player.Objectives.Count);
		
		//Player-specific
		if(Player.m_instance != null){
			Player player = Player.m_instance;
			//Status bars
			DrawStatusBar(new Rect(statusArea.x, statusArea.y, statusArea.width, statusArea.height/2 - 1), "HP", player.health, 10, healthBarColor);
			DrawStatusBar(new Rect(statusArea.x, statusArea.y+statusArea.height/2+1, statusArea.width, statusArea.height/2-1), "WGT", player.TotalItemWeight(), 12, weightBarColor);
			
			//Equipment
			/*
			DrawInventoryButton(equipmentArea.x, equipmentArea.y);
			DrawInventoryButton(equipmentArea.x + 67, equipmentArea.y);
			DrawInventoryButton(equipmentArea.x, equipmentArea.y + 67);
			DrawInventoryButton(equipmentArea.x + 67, equipmentArea.y + 67);
			*/
			
			//Held items
			for(int i = 0; i < player.Hands.Count; i++){
				DrawItemHolder(heldArea.x+83*i, heldArea.y, player.Hands[i]);
				PotentialActions pa;
				if(selectedItem != null){
					pa = player.Hands[i].ItemActions(selectedItem);
				}
				else{
					pa = player.Hands[i].Actions();
				}
				for(int j = 0; j < pa.Count; j++){
					float alpha = 0.6f;
					if(j == actionIndex % pa.Count){
						alpha = 0.8f;
					}
					switch(pa.GetAction(j).status){
						case ActionStatus.Error:
							GUI.color = new Color(0.8f, 0.25f, 0.25f, alpha);
						break;
						case ActionStatus.Warning:
							GUI.color = new Color(0.7f, 0.7f, 0.3f, alpha);
						break;
						default:
							GUI.color = new Color(0.25f, 0.8f, 0.25f, alpha);
						break;
					}
					Rect r = new Rect(heldArea.x + 83 * i, heldArea.y - pa.Count * 18 + j * 18, 80, 16);
					GUI.DrawTexture(r, white);
					GUI.color = Color.white;
					GUI.Label(r, pa.GetAction(j).name);
				}
			}
			
			//Inventory
			for(int i = 0; i < player.Inventory.Count; i++){
				DrawItemHolder(inventoryArea.x + i * 83, inventoryArea.y, player.Inventory[i]);
			}
		}
		else{
			//GUI.Label(new Rect(actionArea.x, actionArea.y, actionArea.width, descriptionArea.y - actionArea.y), "MISSION\nFAILED", hugeFancyText);
		}
	}
			
	void DrawStatusBar(Rect position, string label, float current, float max, Color color){
		Rect leftRect = new Rect(position.x, position.y, 48, position.height/2 - 1);
		Rect rightRect = new Rect(position.x, leftRect.yMax + 2, leftRect.width, leftRect.height);
		float barAreaLength = (position.width - leftRect.width - 2);
		float barLength = barAreaLength * current / Mathf.Max(current, max);
		float maxBarLength = barAreaLength * Mathf.Min(1, max / (float)current);
		Rect bottomRect = new Rect(leftRect.xMax+2, position.y, barLength, position.height);
		Rect maxIndicatorBaseRect = new Rect(bottomRect.x, bottomRect.yMax-1, maxBarLength, 1);
		Rect maxIndicatorNotchRect = new Rect(maxIndicatorBaseRect.xMax-1, bottomRect.y + bottomRect.height/2, 1, bottomRect.height/2);
		//Bars
		GUI.color = color;
		GUI.DrawTexture(leftRect, white);
		GUI.DrawTexture(rightRect, white);
		GUI.DrawTexture(bottomRect, white);
		
		GUI.color = Color.white;
		GUI.DrawTexture(maxIndicatorBaseRect, white);
		GUI.DrawTexture(maxIndicatorNotchRect, white);
		
		//Labels
		GUI.color = Color.white;
		GUI.Label(leftRect, label, GUI.skin.GetStyle("Fancy Text"));
		GUI.Label(rightRect, current + "/" + max, GUI.skin.GetStyle("Fancy Text"));
	}
	
	void DrawItemHolder(float x, float y, ItemHolder itemHolder){
		if(itemHolder.held == null){
			GUI.Button(new Rect(x, y, 80, 64), "No Item");
		}
		else
			DrawItem(x, y, itemHolder.held);
		GUI.color = Color.blue;
		if(itemHolder.TimeOnAction() > 0){
			GUI.color = Color.yellow;
			GUI.DrawTexture(new Rect(x+1, y+1, itemHolder.TimeOnAction() * 39, 6), white);
		}
		GUI.color = Color.white;
		if(MouseInGUIRect(new Rect(x, y, 80, 64))){
			selectedItem = itemHolder;
		}
	}
	
	void DrawItem(float x, float y, Item item){
		Texture2D tex = null;
		if(item != null){
			tex = item.icon;
		}
		
		Rect pos = new Rect(x, y, 80, 64);
		GUI.Button(pos, tex);
		GUI.color = Color.white;
		string ammoString = "";
		Firearm asGun = item as Firearm;
		if(asGun != null){
			if(asGun.magazine != null){
				ammoString = asGun.magazine.count.ToString();
				if(asGun.isChambered)
					ammoString += "+1";
			}
			else if(asGun.isChambered){
				ammoString = "+1";
			}
			else{
				ammoString = "--";
			}
		}
		Magazine asMag = item as Magazine;
		if(asMag != null){
			ammoString = asMag.count.ToString();
		}
		if(ammoString != "")
			GUI.Box(new Rect(pos.xMax-32, pos.yMax-12, 32, 12), ammoString);
	}
	
	ActionBarResult DrawActionBar(Rect position, string text, string subtext, float timeToComplete, float extendedTime, ActionStatus actionStatus){
		bool cancellable = MouseInGUIRect(position) && (timeToComplete > 0 || extendedTime > 0);
		//Bars
		Rect labelRect = new Rect(position.x, position.y, 92, 20);
		float maxDisplayableTime = 2;
		float barFraction = Mathf.Min(timeToComplete / maxDisplayableTime, 1);
		float extendedBarFraction = Mathf.Min(extendedTime / maxDisplayableTime, 1-barFraction);
		Rect cancelRect = new Rect(position.xMax - 60, position.y, 60, 20);
		Rect barRect = new Rect(position.x + 94, labelRect.y, barFraction * (position.width - labelRect.width - 2), 20);
		Rect extendedBarRect = new Rect(barRect.xMax, barRect.y, extendedBarFraction * (position.width - labelRect.width - 2), 20);
		if(cancellable){
			if(extendedBarRect.width + barRect.width > position.width - labelRect.width - cancelRect.width - 4)
				extendedBarRect.width = Mathf.Max(0, position.width - labelRect.width - cancelRect.width - barRect.width - 4);
			if(barRect.width > position.width - labelRect.width - cancelRect.width - 4)
				barRect.width = Mathf.Max(0, position.width - labelRect.width - cancelRect.width - 4);
		}
		
		if(actionStatus == ActionStatus.Warning)
			GUI.color = actionWarn;
		else if(timeToComplete > 0 || extendedTime > 0)
			GUI.color = actionOK;
		else 
			GUI.color = actionDull;
		GUI.DrawTexture(labelRect, white);
		GUI.DrawTexture(barRect, white);
		if(extendedTime > 0){
			GUI.color = actionDull;
			GUI.DrawTexture(extendedBarRect, white);
		}
		if(cancellable){
			GUI.color = actionCancel;
			GUI.DrawTexture(cancelRect, white);
		}
		//Labels
		GUI.color = Color.white;
		GUI.Label(labelRect, text, actionNameText);
		GUI.Label(new Rect(barRect.x+4, barRect.y, barRect.width, barRect.height), subtext, actionTimeText);
		if(cancellable)
			GUI.Label(cancelRect, "CANCEL", fancyText);
		return new ActionBarResult(MouseInGUIRect(labelRect), cancellable && MouseInGUIRect(cancelRect));
	}
	
	void DrawAutoStatusBar(Rect position, string textLeft, string textRight){
		GUI.color = autonomousActionBarColor;
		
		//Bars
		Rect leftRect = new Rect(position.x, position.y, position.width / 2 - 1, position.height);
		Rect rightRect = new Rect(position.x + leftRect.width + 2, position.y, position.width / 2 - 1, position.height);
		GUI.DrawTexture(leftRect, white);
		GUI.DrawTexture(rightRect, white);
		//Labels
		GUI.color = Color.white;
		GUI.Label(leftRect, textLeft, fancyText);
		GUI.Label(rightRect, textRight, fancyText);
	}
		
	bool MouseOnGUI(){
		Vector2 mousePos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
		return mainBox.Contains(mousePos);
	}
	
	bool MouseInGUIRect(Rect r){
		Vector2 mousePos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
		return r.Contains(mousePos);
	}
	
	bool GUIMouseDown(int button){
		return Event.current.type == EventType.MouseDown && Event.current.button == button;
	}
}

public struct ActionBarResult{
	public bool mouseOverLabel;
	public bool mouseOverCancel;
	
	public ActionBarResult(bool overLabel, bool overCancel){
		mouseOverLabel = overLabel;
		mouseOverCancel = overCancel;
	}
}
