using UnityEngine;
using Random = UnityEngine.Random;
using System;
using System.Collections;

public struct CrateFeature: IEquatable<CrateFeature> {
	public const int NumNormalColors = 3;
	public const int NumNormalShapes = 3;
	public const int ColorWildcard = 4;
	public const int ShapeWildcard = 4;

	public int Color;
	public int Shape;

	public static CrateFeature RandomNonWild()
	{
		return new CrateFeature(Random.Range (0, NumNormalColors),
		                        Random.Range (0, NumNormalShapes));
	}

	public CrateFeature(int color, int shape)
	{
		Color = color;
		Shape = shape;
	}

	public CrateFeature(CrateFeature toClone)
	{
		Color = toClone.Color;
		Shape = toClone.Shape;
	}

	//Comparison ops
	public override bool Equals(object obj) 
	{
		return obj is CrateFeature && this == (CrateFeature)obj;
	}
	public bool Equals(CrateFeature c)
	{
		return this == c;
	}
	public override int GetHashCode() 
	{
		return Color.GetHashCode() ^ Shape.GetHashCode();
	}
	public static bool operator ==(CrateFeature x, CrateFeature y) 
	{
		return x.Shape == y.Shape && x.Color == y.Color;
	}
	public static bool operator !=(CrateFeature x, CrateFeature y) 
	{
		return !(x == y);
	}

	//Features match if either has a wildcard color,
	//or they have the same color, and likewise for shape.
	public bool Matches(CrateFeature other)
	{
		return (Color == ColorWildcard
			|| other.Color == ColorWildcard
			|| other.Color == this.Color)
			&& (Shape == ShapeWildcard
			|| other.Shape == ShapeWildcard
			|| other.Shape == this.Shape);
	}

	public Sprite GetSprite()
	{
		return CrateFeatureDisplayManager.GetSprite (this);
	}
}
