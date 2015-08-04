using UnityEngine;
using System.Collections;

public class ModeButton : MonoBehaviour, IInputReceiver {

	public GameMode.Mode mode;

	private void Awake(){
		GameMode.ModeChanged += OnModeChanged;
	}

	private void OnDestroy()
	{
		GameMode.ModeChanged -= OnModeChanged;
	}

	public void OnInputDown(){
		if(!GameMode.ModeLocked)
		{
			if(GameMode.Current == mode){
				GameMode.Current = GameMode.Mode.None;
			}
			else{
				GameMode.Current = mode;
			}
		}
	}

	private void OnModeChanged(GameMode.Mode newMode){
		if(newMode == mode){
			GetComponent<SpriteRenderer>().color = Color.green;
		}
		else{
			GetComponent<SpriteRenderer>().color = Color.white;
		}
	}
}
