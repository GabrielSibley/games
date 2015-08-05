using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Vector2i{
	public static Vector2i Up {
		get{ return new Vector2i(0, 1); }
	}
	
	public static Vector2i Down {
		get{ return new Vector2i(0, -1); }
	}
	
	public static Vector2i Left {
		get{ return new Vector2i(-1, 0); }
	}
	
	public static Vector2i Right {
		get{ return new Vector2i(1, 0); }
	}
	
	public static Vector2i operator + (Vector2i a, Vector2i b){
		return new Vector2i(a.x + b.x, a.y + b.y);
	}

	public static bool operator == (Vector2i a, Vector2i b)
	{
		return a.Equals (b);
	}

	public static bool operator != (Vector2i a, Vector2i b)
	{
		return !a.Equals (b);
	}

	public static implicit operator Vector2 (Vector2i a)
	{
		return new Vector2(a.x, a.y);
	}
	
	public int x;
	public int y;
	
	public Vector2i(int x, int y){
		this.x = x;
		this.y = y;
	}
	
	public override bool Equals(object other)
	{
		if(other is Vector2i)
		{
			var otherOffset = (Vector2i)other;
			return otherOffset.x == x && otherOffset.y == y;
		}
		return false;
	}

	public override int GetHashCode(){
		return x * 101 + y;
	}
}

