using UnityEngine;
using System.Collections;

public class GameMode {

	public static event System.Action<Mode> ModeChanged;

	public enum Mode {
		None,
		MoveMachine,
		LinkMachine
	};

	public static Mode Current{
		get{
			return currentMode;
		}
		set{
			currentMode = value;
			ModeChanged(currentMode);
		}
	}
	private static Mode currentMode;
}
