/***************

Winnput.cs!

The arrays that are returned are all of length 2, one element
for each player, so you can stuff like:

    if ( BDown[m_playerNumber] ){
        // shoot!
    }

public static bool HomeDown         Returns true during the frame someone starts pressing the shared, center key
public static bool AnyButtonDown    Returns true during the frame someone starts pressing any button.
public static bool[] A              Returns true if the A button is pressed.
public static bool[] B              Returns true if the B button is pressed.
public static bool[] ADown          Returns true during the frame someone starts pressing the A button
public static bool[] BDown          Returns true during the frame someone starts pressing the B button
public static bool[] AUp            Returns true during the frame someone releases the A button
public static bool[] BUp            Returns true during the frame someone releases the B button
public static bool[] Up             Returns true if the joystick is up (as in north)
public static bool[] Down           Returns true if the joystick is down (as in south)
public static bool[] Left           Returns true if the joystick is left (as in west)
public static bool[] Right          Returns true if the joystick is right (as in east)
public static int[] Vertical        Returns -1 (down/south), 0 (neutral), or +1 (up/north)
public static int[] Horizontal      Returns -1 (left/west), 0 (neutral), or +1 (right/east)

If you want to add more functions, like "Down" for directions... feel free! :)

**************/

using UnityEngine;
using System.Collections;

public static class Winnput {

	private const KeyCode Home_Button	= KeyCode.Alpha1;

	private const KeyCode Player1_A		= KeyCode.X;
	private const KeyCode Player1_B		= KeyCode.Z;
	private const KeyCode Player1_Up	= KeyCode.W;
	private const KeyCode Player1_Down	= KeyCode.S;
	private const KeyCode Player1_Left	= KeyCode.A;
	private const KeyCode Player1_Right	= KeyCode.D;

	private const KeyCode Player2_A 	= KeyCode.RightShift;
	private const KeyCode Player2_B		= KeyCode.RightControl;
	private const KeyCode Player2_Up	= KeyCode.UpArrow;
	private const KeyCode Player2_Down	= KeyCode.DownArrow;
	private const KeyCode Player2_Left	= KeyCode.LeftArrow;
	private const KeyCode Player2_Right	= KeyCode.RightArrow;

		
	public static bool HomeDown {
		get {
			return Input.GetKeyDown(Home_Button);
		}
	}
	
	public static bool AnyButtonDown {
		get {
			return HomeDown || ADown[0] || ADown[1] || BDown[0] || BDown[1];
		}
	}

	public static bool[] A {
		get {
			return new bool[] { Input.GetKey(Player1_A), Input.GetKey(Player2_A) };
		}
	}

	public static bool[] B {
		get {
			return new bool[] { Input.GetKey(Player1_B), Input.GetKey(Player2_B) };
		}
	}

	public static bool[] ADown {
		get {
			return new bool[] { Input.GetKeyDown(Player1_A), Input.GetKeyDown(Player2_A) };
		}
	}

	public static bool[] BDown {
		get {
			return new bool[] { Input.GetKeyDown(Player1_B), Input.GetKeyDown(Player2_B) };
		}
	}

	public static bool[] AUp {
		get {
			return new bool[] { Input.GetKeyUp(Player1_A), Input.GetKeyUp(Player2_A) };
		}
	}

	public static bool[] BUp {
		get {
			return new bool[] { Input.GetKeyUp(Player1_B), Input.GetKeyUp(Player2_B) };
		}
	}

	public static bool[] Up {
		get {
			return new bool[] { Input.GetKey(Player1_Up), Input.GetKey(Player2_Up) };
		}		
	}

	public static bool[] Down {
		get {
			return new bool[] { Input.GetKey(Player1_Down), Input.GetKey(Player2_Down) };
		}		
	}

	public static bool[] Left {
		get {
			return new bool[] { Input.GetKey(Player1_Left), Input.GetKey(Player2_Left) };
		}		
	}

	public static bool[] Right {
		get {
			return new bool[] { Input.GetKey(Player1_Right), Input.GetKey(Player2_Right) };
		}		
	}

	public static int[] Vertical {
		get {
			return new int[] { (Up[0]?1:0) - (Down[0]?1:0), (Up[1]?1:0) - (Down[1]?1:0) };
		}
	}

	public static int[] Horizontal {
		get {
			return new int[] { (Right[0]?1:0) - (Left[0]?1:0), (Right[1]?1:0) - (Left[1]?1:0) };
		}
	}
}
