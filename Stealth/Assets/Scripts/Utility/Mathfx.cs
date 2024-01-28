using UnityEngine;

public static class Mathfx {
	
	//Returns a new Vector3 with each component Abs'd
	public static Vector3 Abs(this Vector3 v) {
		v.x = Mathf.Abs(v.x);
		v.y = Mathf.Abs(v.y);
		v.z = Mathf.Abs(v.z);
		return v;
	}
	
	//Smoothstep between 0 and 1, usually for creating interpolation parameters
	public static float SmoothStep01(float t){
		return t*t*(3 - 2*t);
	}
	
	//Cubic Hermite interpolation function
	public static float Cerp(float a, float b, float dadt, float dbdt, float t) {
		float	t2 = t*t,
				t3 = t2*t;
		return	a*(2*t3 - 3*t2 + 1f) +
				b*(3*t2 - 2*t3) +
				dadt*(t3 - 2*t2 + t) +
				dbdt*(t3 - t2);
	}
	
	//a smooth step function with variable slope at the endpoints
	public static float Cerp01(float d0, float d1, float t) {
		float	t2 = t*t,
				t3 = t2*t;
		return 	(3*t2 - 2*t3) +
				d0*(t3 - 2*t2 + t) +
				d1*(t3 - t2);
	}
	
	//Gaussian distribution
	public static float Gaussian(float x, float median, float variance){
		float c = Mathf.Sqrt(variance);
		float factor = 1.0f/(c*Mathf.Sqrt(2.0f*Mathf.PI));
		
		float numerator = -1.0f * (x-median) * (x-median);
		float denominator = (2.0f * c*c);
		return factor*Mathf.Exp(numerator/denominator);
	}
	
	//do two rects intersect?
	public static bool Intersects(this Rect a, Rect b) {
		if (a.x + a.width < b.x ||
			b.x + b.width < a.x ||
			a.y + a.width < b.y ||
			b.y + b.width < a.y)
			return false;
		else
			return true;
	}
	
	public static Rect Lerp(Rect a, Rect b, float t) {
		return new Rect(
			Mathf.Lerp(a.x, b.x, t),
			Mathf.Lerp(a.y, b.y, t),
			Mathf.Lerp(a.width, b.width, t),
			Mathf.Lerp(a.height, b.height, t)
		);
	}
	
	//are two vectors nearly the same?
	public static bool Approximately(Vector3 a, Vector3 b) {
		Vector3 difference = a - b;
		return difference.sqrMagnitude < Mathf.Epsilon*Mathf.Epsilon;
	}
	
	//shortest rotation to get from a to b
	public static float ShortestAngle(float a, float b) {
		return NormalizeAngle(b - a);
	}
	
	//normalizes an angle such that it is between [-180, 180]
	public static float NormalizeAngle(float angle) {
		angle %= 360f;
		if (angle < -180f)
			angle += 360f;
		else if (angle > 180f)
			angle -= 360f;
		return angle;
	}
	
	//normalizes an angle such that it is between [0, 360)
	public static float NormalizeAngle360(float angle){
		angle %= 360f;
		if(angle < 0)
			angle += 360;
		return angle;
	}
	
	//clamp an angle such that it is between min and max going clockwise
	//angles outside min and max will equal the closer of the two
	public static float ClampAngle (float angle, float min, float max) {
		max = NormalizeAngle(max);
		min = NormalizeAngle(min);
		if (max < min)
			max += 360f;
		float mean = (min + max)*0.5f;
		min -= mean;
		max -= mean;
		angle -= mean;
		angle = NormalizeAngle(angle);
		angle = Mathf.Clamp(angle, min, max) + mean;
		return NormalizeAngle(angle);
	}
	
	//Similar to Mathf.Repeat, but for integers. Essentially the modulo operator but result will always be 0 or positive. Result will not
	//equal or exceed the modulus.
	public static int Repeat(int t, int modulus){
		modulus = Mathf.Abs(modulus);
		if(t >= 0){
			return t % modulus;
		}
		else{
			int r = t % modulus;
			if(r == 0){
				return 0;
			}
			else{
				return r + modulus;
			}
		}
	}
	
	//Quadratic 2d bezier
	public static Vector2 Bezier(Vector2 pt1, Vector2 pt2, Vector2 pt3, float t){
		float c1 = (1-t)*(1-t);
		float c2 = 2*t*(1-t);
		float c3 = t*t;
		return pt1 * c1 + pt2 * c2 + pt3 * c3;
	}
	
	//2d quadratic Bezier curve with 2nd control point derived from endpoints and a displacement factor
	//Positive swing moves to the right of the direction of travel, negative swing moves to the left.
	public static Vector2 Swing(Vector2 pt1, Vector2 pt3, float swing, float t){
		Vector2 midPointOffset = (pt3 - pt1)/2; //Get relative midpoint of p1->p3
		Vector2 swingVec = new Vector2(-midPointOffset.y, midPointOffset.x); //Make a vector perpendicular to p1->p2
		Vector2 pt2 = pt1 + midPointOffset + swingVec * swing;
		return Bezier(pt1, pt2, pt3, t);
	}
	
	//Returns area of the triangle defined by the 3 points.
	public static float TriangleArea(Vector3 pt1, Vector3 pt2, Vector3 pt3){
		return Vector3.Cross((pt2-pt1), (pt3-pt1)).magnitude/2;
	}
	
	/* Subset of Mathfx.cs from http://www.unifycommunity.com/wiki/index.php?title=Mathfx#C.23_-_Mathfx.cs*/
	public static float Berp(float start, float end, float value)
    {
        value = Mathf.Clamp01(value);
        value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
        return start + (end - start) * value;
    }

	public static float MyBerp(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = 1 - (Mathf.Cos(value * Mathf.PI * 4)*.5f + 0.5f)*(1-value);
		return start + (end - start) * value;
	}
	public static float Sinerp(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));
    }

	//Returns the center of a circle defined by 3 points.
	//Undefined behavior if all three points lie on the same line.
	public static Vector2 Circle3Points(Vector2 a, Vector2 b, Vector2 c){
		//Reorder points to avoid infinte slopes
		if(a.x == b.x){
			Vector2 temp = b;
			b = c;
			c = temp;
		}
		if(b.x == c.x){
			Vector2 temp = b;
			b = a;
			b = temp;
		}
		//let line 1 be the line AB
		float m1 = (b.y - a.y) / (b.x - a.x);
		//let line 2 be the line BC
		float m2 = (c.y - b.y) / (c.x - b.x);
		Vector2 result = new Vector2();
		result.x = (m1*m2*(a.y - c.y) + m2*(a.x+b.x) - m1*(b.x+c.x)) / (2 * (m2 - m1));
		result.y = -(result.x - (a.x + b.x)/2) / m1 + (a.y+b.y)/2;
		return result;
	}
}
