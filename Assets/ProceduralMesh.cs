using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralMesh
{
	private Vector3[] vertices;
	private Vector3[] normals;
	private int[] triangles;
	private Vector2[] uvs;

	public void Cleanup()
	{
		
	}

	public void InitMesh()
	{
		
	}

	public Mesh GetMesh(bool autoBakeNormals)
	{
		Mesh mesh = new Mesh();

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		
		
		
		return mesh;
	}
}
