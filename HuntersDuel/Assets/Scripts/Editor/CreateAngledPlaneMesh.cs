using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreateAngledPlaneMesh {

	[MenuItem("Assets/Create Angled Plane Mesh")]
	public static void Go(){
		Mesh mesh = new Mesh();
		
		mesh.vertices = new Vector3[]{
			new Vector3(-0.5f, -0.5f, 1),
			new Vector3(+0.5f, -0.5f, 1),
			new Vector3(-0.5f, +0.5f, 0),
			new Vector3(+0.5f, +0.5f, 0)
		};
		
		mesh.uv = new Vector2[]{
			new Vector2(0, 0),
			new Vector2(1, 0),
			new Vector2(0, 1),
			new Vector2(1, 1)
		};
		
		mesh.triangles = new int[]{
			0, 2, 1,
			2, 3, 1
		};
		
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		
		AssetDatabase.CreateAsset(mesh, "Assets/Angled Plane.asset");
	}	
}
