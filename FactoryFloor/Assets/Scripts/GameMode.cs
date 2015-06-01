using UnityEngine;
using System.Collections;

public class GameMode {

	public static event System.Action<Mode> ModeChanged;

	private static readonly Mode[] LockedModes = new Mode[]{
		Mode.MoveMachine,
		Mode.MovePipe
	};

	public enum Mode {
		None,
		SelectMachine,
		MoveMachine,
		SelectPipe,
		MovePipe
	};

	public static bool ModeLocked
	{
		get { return System.Array.Exists(LockedModes, x => x == Current); }
	}

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
