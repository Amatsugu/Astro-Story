using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
	Mesh mesh;
	int resolution;
	Vector3 localUp;
	Vector3 axisA, axisB;
	ShapeGenerator shapeGenerator;

	public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
	{
		this.mesh = mesh;
		this.resolution = resolution;
		this.localUp = localUp;
		this.shapeGenerator = shapeGenerator;

		axisA = new Vector3(localUp.y, localUp.z, localUp.x);
		axisB = Vector3.Cross(localUp, axisA);
	}

	public void ConstructMesh()
	{
		Vector3[] vertices = new Vector3[resolution * resolution];
		int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];

		int triangleIndex = 0;
		var uv = (mesh.uv.Length == vertices.Length) ? mesh.uv : new Vector2[vertices.Length];

		for (int y = 0; y < resolution; y++)
		{
			for (int x = 0; x < resolution; x++)
			{
				int i = x + y * resolution;
				Vector2 percent = new Vector2(x, y) / (resolution - 1);
				Vector3 pointOnUnitCube = localUp;
				pointOnUnitCube += (percent.x - .5f) * 2 * axisA; //Offset along AxisA
				pointOnUnitCube += (percent.y - .5f) * 2 * axisB; //Offset along AxisB

				var poitOnUnitSphere = pointOnUnitCube.normalized; //Make spherical

				float unscaledElevation	= shapeGenerator.CalculateUnscaledElevation(poitOnUnitSphere);
				vertices[i] = poitOnUnitSphere * shapeGenerator.GetScaledElevation(unscaledElevation);

				uv[i].y = unscaledElevation;


				if(x != resolution-1 && y != resolution-1)
				{
					triangles[triangleIndex++] = i;
					triangles[triangleIndex++] = i + resolution + 1;
					triangles[triangleIndex++] = i + resolution;

					triangles[triangleIndex++] = i;
					triangles[triangleIndex++] = i + 1;
					triangles[triangleIndex++] = i + resolution + 1;

				}
			}
		}

		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uv;
		mesh.RecalculateNormals();
	}

	public void UpdateUVs(ColorGenerator colorGen)
	{
		Vector2[] uv = mesh.uv;
		for (int y = 0; y < resolution; y++)
		{
			for (int x = 0; x < resolution; x++)
			{
				int i = x + y * resolution;
				Vector2 percent = new Vector2(x, y) / (resolution - 1);
				Vector3 pointOnUnitCube = localUp;
				pointOnUnitCube += (percent.x - .5f) * 2 * axisA; //Offset along AxisA
				pointOnUnitCube += (percent.y - .5f) * 2 * axisB; //Offset along AxisB

				var poitOnUnitSphere = pointOnUnitCube.normalized; //Make spherical

				uv[i].x = colorGen.BiomePercentFromPoint(poitOnUnitSphere);
			}
		}
		mesh.uv = uv;
	}

}
