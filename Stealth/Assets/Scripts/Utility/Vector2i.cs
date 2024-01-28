using UnityEngine;
using System.Collections;

public struct Vector2i : System.IEquatable<Vector2i> {
	//Castable to Vector2
	public static implicit operator Vector2(Vector2i original){
		return new Vector2(original.x, original.y);
	}
	
	//Operator overloads
	public static Vector2i operator +(Vector2i a, Vector2i b){
		return new Vector2i(a.x + b.x, a.y + b.y);
	}
	
	public static Vector2i operator -(Vector2i a, Vector2i b){
		return new Vector2i(a.x - b.x, a.y - b.y);
	}

	public static bool operator ==(Vector2i a, Vector2i b) {
		return a.x == b.x && a.y == b.y;
	}
	public static bool operator !=(Vector2i x, Vector2i y) {
		return !(x == y);
	}
	
	public int x, y;
	
	public Vector2i(int x, int y){
		this.x = x;
		this.y = y;
	}
	
	public override bool Equals(object obj) {
		return obj is Vector2i && this == (Vector2i)obj;
	}
	
	public bool Equals(Vector2i obj){
		return this == obj;
	}
	
	public override int GetHashCode() {
    	return x.GetHashCode() ^ y.GetHashCode();
	}
	
	public float magnitude{
		get{
			return Mathf.Sqrt(x*x + y*y);
		}
	}
}
