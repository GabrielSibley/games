using UnityEngine;
using System.Collections;

public class AccuracyDisplay : MonoBehaviour {
	
	public MeshRenderer m_chargeRenderer;
	public MeshFilter m_chargeMesh, m_minMesh;
	public float m_maxRadius;
	public float m_minRadius;
	public float m_minRadiusPower;
	public float m_chargeThickness, m_minThickness;
	public AnimationCurve m_alphaCurve;
	public Material m_regularMaterial, m_killMaterial, m_maxMaterial;
	
	protected int m_vertexCount = 32;
	
	public float Power{
		get{return m_power;} set{m_power = Mathf.Clamp (value, 0, MaxPower);}
	}
	protected float m_power;
	
	public float MaxPower{
		get; set;
	}
	public float Damage{
		get{ return Power * Multiplier; }
	}
	public float KillDamage{
		get; set;
	}
	public float KillPower{
		get{return KillDamage / Multiplier;}
	}	
	public float Multiplier{
		get; set;
	}
	
	protected void Update(){
		if(m_chargeMesh.mesh != null){
			Destroy(m_chargeMesh.mesh);
		}
		if(m_minMesh.mesh != null){
			Destroy(m_minMesh.mesh);
		}
		if(Power >= 1){
			m_chargeMesh.mesh = MakeChargeMesh();
			//m_minMesh.mesh = MakeMinMesh();
			if(Power >= KillPower){
				m_chargeRenderer.material.color = m_killMaterial.color;
			}
			else if(Power >= MaxPower){
				m_chargeRenderer.material.color = m_maxMaterial.color;
			}
			else{
				m_chargeRenderer.material.color = m_regularMaterial.color;
			}
			float accuracyLerp = Mathf.InverseLerp(1, KillPower, Power);
			Color c = m_chargeRenderer.material.color;
			m_chargeRenderer.material.color = new Color(c.r, c.g, c.b, m_alphaCurve.Evaluate(accuracyLerp));
		}

	}
	
	protected Mesh MakeChargeMesh(){
		Mesh m = new Mesh();
		Vector3[] vertices = new Vector3[16];
		int[] triangles = new int[24];
		float accuracyLerp = Mathf.InverseLerp(1, KillPower, Power);
		float radius = Mathf.Lerp(m_maxRadius, m_minRadius, accuracyLerp);
		float arc = Mathf.Deg2Rad * 60 * (m_minRadius + radius) / (radius * 2);
		float spin = (Mathf.Sin(Time.time) * (0.4f - 0.2f*accuracyLerp)) + (radius*2);
		SquareSide(radius, m_chargeThickness, Mathf.Deg2Rad * 0 + spin, arc, vertices, 0, triangles, 0);
		SquareSide(radius, m_chargeThickness, Mathf.Deg2Rad * 90 + spin, arc, vertices, 4, triangles, 6);
		SquareSide(radius, m_chargeThickness, Mathf.Deg2Rad * 180 + spin, arc, vertices, 8, triangles, 12);
		SquareSide(radius, m_chargeThickness, Mathf.Deg2Rad * 270 + spin, arc, vertices, 12, triangles, 18);
		m.vertices = vertices;
		m.triangles = triangles;
		m.normals = new Vector3[16];
		m.uv = new Vector2[16];
		return m;
	}
	
	protected Mesh MakeMinMesh(){
		Mesh m = new Mesh();
		Vector3[] vertices = new Vector3[16];
		int[] triangles = new int[24];
		float radius = m_minRadius;
		float arc = Mathf.Deg2Rad * 60;
		SquareSide(radius, m_minThickness, Mathf.Deg2Rad * 0, arc, vertices, 0, triangles, 0);
		SquareSide(radius, m_minThickness, Mathf.Deg2Rad * 90, arc, vertices, 4, triangles, 6);
		SquareSide(radius, m_minThickness, Mathf.Deg2Rad * 180, arc, vertices, 8, triangles, 12);
		SquareSide(radius, m_minThickness, Mathf.Deg2Rad * 270, arc, vertices, 12, triangles, 18);
		m.vertices = vertices;
		m.triangles = triangles;
		m.normals = new Vector3[16];
		m.uv = new Vector2[16];
		return m;
	}
	
	protected void SquareSide(float radius, float thickness, float theta, float arc, Vector3[] vertices, int startVertex, int[] triangles, int startTriangle){
		Vector3 arcStart = new Vector3(Mathf.Cos(theta - arc/2), 0, Mathf.Sin(theta-arc/2));
		Vector3 arcEnd = new Vector3(Mathf.Cos(theta + arc/2), 0, Mathf.Sin(theta+arc/2));
		vertices[startVertex  ] = arcEnd * (radius + thickness);
		vertices[startVertex+1] = arcStart * (radius + thickness);
		vertices[startVertex+2] = arcStart * radius;
		vertices[startVertex+3] = arcEnd * radius;
		triangles[startTriangle  ] = startVertex  ;
		triangles[startTriangle+1] = startVertex+1;
		triangles[startTriangle+2] = startVertex+3;
		triangles[startTriangle+3] = startVertex+1;
		triangles[startTriangle+4] = startVertex+2;
		triangles[startTriangle+5] = startVertex+3;
	}
}
