using UnityEngine;
using System.Collections;

public class TutorialInputSim : MonoBehaviour {

	private static TutorialInputSimEvent[] PistolKeys = new TutorialInputSimEvent[]{
		new TutorialInputSimEvent(false, false, 0.4f), //Breather
		new TutorialInputSimEvent(false, true, 0.4f), //Aim
		new TutorialInputSimEvent(true, true, 0.2f), //Fire!
		new TutorialInputSimEvent(false, true, 0.1f), //Pause
		new TutorialInputSimEvent(true, true, 0.2f), //Fire!
		new TutorialInputSimEvent(false, true, 0.1f), //Pause
		new TutorialInputSimEvent(true, true, 0.2f), //Fire!
		new TutorialInputSimEvent(false, true, 0.1f), //Pause
		new TutorialInputSimEvent(true, true, 0.2f), //Fire!
		new TutorialInputSimEvent(false, true, 0.1f), //Pause
		new TutorialInputSimEvent(true, true, 0.2f), //Fire!
		new TutorialInputSimEvent(false, true, 0.1f), //Pause
		new TutorialInputSimEvent(true, true, 0.2f), //Fire!
		new TutorialInputSimEvent(false, false, 0.5f), //Longer Pause
		new TutorialInputSimEvent(true, false, 0.3f), //Hold A
		new TutorialInputSimEvent(true, false, KeyCode.LeftArrow, 0.6f), //Cylinder out
		new TutorialInputSimEvent(true, false, 0.3f), //pause
		new TutorialInputSimEvent(true, false, KeyCode.DownArrow, 0.6f), //Dump shells
		new TutorialInputSimEvent(true, false, 0.3f), //pause
		new TutorialInputSimEvent(true, false, KeyCode.UpArrow, 0.2f), //Reload bullet
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.1f), //pause
		new TutorialInputSimEvent(true, false, KeyCode.UpArrow, 0.2f), //Reload bullet
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.1f), //pause
		new TutorialInputSimEvent(true, false, KeyCode.UpArrow, 0.2f), //Reload bullet
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.1f), //pause
		new TutorialInputSimEvent(true, false, KeyCode.UpArrow, 0.2f), //Reload bullet
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.1f), //pause
		new TutorialInputSimEvent(true, false, KeyCode.UpArrow, 0.2f), //Reload bullet
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.1f), //pause
		new TutorialInputSimEvent(true, false, KeyCode.UpArrow, 0.2f), //Reload bullet
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.5f), //long pause
		new TutorialInputSimEvent(true, false, KeyCode.RightArrow, 0.6f) //Cylinder in
	};

	private static TutorialInputSimEvent[] RifleKeys = new TutorialInputSimEvent[]{
		new TutorialInputSimEvent(false, false, 0.4f), //Breather
		//Fire
		new TutorialInputSimEvent(false, true, 0.4f), //Aim
		new TutorialInputSimEvent(true, true, 0.3f), //Fire!
		new TutorialInputSimEvent(false, false, 0.5f), //pause
		new TutorialInputSimEvent(true, false, 0.3f), //Hold A
		//Rebolt
		new TutorialInputSimEvent(true, false, KeyCode.UpArrow, 0.4f),
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.1f), //pause
		new TutorialInputSimEvent(true, false, KeyCode.LeftArrow, 0.4f),
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.1f), //pause
		new TutorialInputSimEvent(true, false, KeyCode.RightArrow, 0.4f),
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.1f), //pause
		new TutorialInputSimEvent(true, false, KeyCode.DownArrow, 0.4f),
		new TutorialInputSimEvent(true, false, 0.3f), //pause
		//Fire
		new TutorialInputSimEvent(false, true, 0.4f), //Aim
		new TutorialInputSimEvent(true, true, 0.3f), //Fire!
		new TutorialInputSimEvent(false, false, 0.5f), //pause
		new TutorialInputSimEvent(true, false, 0.3f), //Hold A
		//Open bolt
		new TutorialInputSimEvent(true, false, KeyCode.UpArrow, 0.4f),
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.1f), //pause
		new TutorialInputSimEvent(true, false, KeyCode.LeftArrow, 0.4f),
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.1f), //pause
		//Load 2x bullets
		new TutorialInputSimEvent(true, false, KeyCode.UpArrow, 0.4f),
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.1f), //pause
		new TutorialInputSimEvent(true, false, KeyCode.DownArrow, 0.4f),
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.1f), //pause
		new TutorialInputSimEvent(true, false, KeyCode.UpArrow, 0.4f),
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.1f), //pause
		new TutorialInputSimEvent(true, false, KeyCode.DownArrow, 0.4f),
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.1f), //pause
		//Close bolt
		new TutorialInputSimEvent(true, false, KeyCode.RightArrow, 0.4f),
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.1f), //pause
		new TutorialInputSimEvent(true, false, KeyCode.DownArrow, 0.4f)
	};

	private static TutorialInputSimEvent[] ShotgunKeys = new TutorialInputSimEvent[]{
		new TutorialInputSimEvent(false, false, 0.4f), //Breather
		//Fire
		new TutorialInputSimEvent(false, true, 0.4f), //Aim
		new TutorialInputSimEvent(true, true, 0.3f), //Fire!
		new TutorialInputSimEvent(false, false, 0.5f), //pause
		new TutorialInputSimEvent(true, false, 0.3f), //Hold A
		//Pump
		new TutorialInputSimEvent(true, false, KeyCode.LeftArrow, 0.4f),
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.1f),
		new TutorialInputSimEvent(true, false, KeyCode.RightArrow, 0.4f),
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.1f),
		//Fire
		new TutorialInputSimEvent(false, true, 0.4f), //Aim
		new TutorialInputSimEvent(true, true, 0.3f), //Fire!
		new TutorialInputSimEvent(false, false, 0.5f), //pause
		new TutorialInputSimEvent(true, false, 0.3f), //Hold A
		//Pump
		new TutorialInputSimEvent(true, false, KeyCode.LeftArrow, 0.4f),
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.1f),
		new TutorialInputSimEvent(true, false, KeyCode.RightArrow, 0.4f),
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.1f),
		//Load 2x
		new TutorialInputSimEvent(true, false, KeyCode.UpArrow, 0.4f),
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.1f),
		new TutorialInputSimEvent(true, false, KeyCode.RightArrow, 0.4f),
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.1f),
		new TutorialInputSimEvent(true, false, KeyCode.UpArrow, 0.4f),
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.1f),
		new TutorialInputSimEvent(true, false, KeyCode.RightArrow, 0.4f),
		new TutorialInputSimEvent(true, false, KeyCode.None, 0.1f)
	};

	private TutorialInputSimEvent[] eventList;
	private int currentEventIndex;
	private float timeRemaining;

	public Gun ActiveGun{
		get { return activeGun; }
		set {
			if(activeGun != null){
				activeGun.gameObject.SetActive(false);
			}
			activeGun = value;
			if(activeGun != null){
				activeGun.gameObject.SetActive(true);
				currentEventIndex = 0;
				switch(activeGun.GunType){
					case GunType.Pistol:
						eventList = PistolKeys;
						break;
					case GunType.Rifle:
						eventList = RifleKeys;
						break;
					case GunType.Shotgun:
						eventList = ShotgunKeys;
						break;
				}
				ExecuteCurrentEvent();
			}
		}
	}
	private Gun activeGun;
	public SpriteRenderer aButton, bButton;
	public SpriteRenderer arrow;
	public Sprite leftArrow, rightArrow, upArrow, downArrow;

	private void Update(){
		if(ActiveGun != null){
			timeRemaining -= Time.deltaTime;
			if(timeRemaining <= 0){
				currentEventIndex = (currentEventIndex + 1) % eventList.Length;
				ExecuteCurrentEvent();
			}
		}
	}

	private void ExecuteCurrentEvent(){
		TutorialInputSimEvent currentEvent = eventList[currentEventIndex];
		aButton.gameObject.SetActive (currentEvent.aDown);
		bButton.gameObject.SetActive (currentEvent.bDown);
		if(currentEvent.bDown && currentEvent.aDown){
			ActiveGun.Fire (null, 0);
			arrow.sprite = null;
		}
		else if(!currentEvent.bDown && currentEvent.aDown){
			switch(currentEvent.key){
			case KeyCode.RightArrow:
				ActiveGun.ManipulateRight();
				arrow.sprite = rightArrow;
				break;
			case KeyCode.LeftArrow:
				ActiveGun.ManipulateLeft();
				arrow.sprite = leftArrow;
				break;
			case KeyCode.UpArrow:
				ActiveGun.ManipulateUp();
				arrow.sprite = upArrow;
				break;
			case KeyCode.DownArrow:
				ActiveGun.ManipulateDown();
				arrow.sprite = downArrow;
				break;				
			case KeyCode.None:
				arrow.sprite = null;
				break;
			}
		}
		else{
			arrow.sprite = null;
		}
		timeRemaining = currentEvent.duration;
	}

	private class TutorialInputSimEvent{
		public bool aDown, bDown;
		public KeyCode key; //Used for left/right/up/down
		public float duration;

		public TutorialInputSimEvent(bool a, bool b, float d){
			aDown = a;
			bDown = b;
			key = KeyCode.None;
			duration = d;
		}

		public TutorialInputSimEvent(bool a, bool b, KeyCode k, float d){
			aDown = a;
			bDown = b;
			key = k;
			duration = d;
		}
	}
}
