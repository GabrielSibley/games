using UnityEngine;
using System.Collections;

public class UnionFind {
	public static void Union(GridSquare x, GridSquare y){
		GridSquare xRoot = Find(x);
		GridSquare yRoot = Find(y);
		bool disjoint = (xRoot != yRoot);
		if(xRoot.rank > yRoot.rank){
			yRoot.parent = xRoot;
			if(disjoint)
				xRoot.setSize += yRoot.setSize;
		}
		else if (xRoot.rank < yRoot.rank){
			xRoot.parent = yRoot;
			if(disjoint)
				yRoot.setSize += xRoot.setSize;
		}
		else if (xRoot != yRoot){
			yRoot.parent = xRoot;
			xRoot.rank = xRoot.rank + 1;
			if(disjoint)
				xRoot.setSize += yRoot.setSize;
		}
	}
	
	public static GridSquare Find(GridSquare x){
		if(x.parent == x)
			return x;
		else{
			x.parent = Find(x.parent);
			return x.parent;
		}
	}
	
	public static void MakeSet(GridSquare x){
		x.parent = x;
		x.rank = 0;
		x.setSize = 1;
	}
}
