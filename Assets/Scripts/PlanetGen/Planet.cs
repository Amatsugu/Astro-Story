using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

public class Planet : MonoBehaviour
{
	public enum FaceRenderMask
	{
		All, Up, Down, Left, Right, Forward, Back
	}

	[Range(2, 255)]
	public int resolution = 8;
	public ShapeSettings shape;
	public ColorSettings color;
	public bool autoUpdate = true;
	public FaceRenderMask faceRenderMask;

	[HideInInspector]
	public bool shapeSettingsFold, ColorSettingsFold;

	[SerializeField, HideInInspector]
	private MeshFilter[] _meshFilters;
	private TerrainFace[] _terrainFaces;
	private ShapeGenerator _shapeGenerator = new ShapeGenerator();
	private ColorGenerator _colorGenerator = new ColorGenerator();

	void Init()
	{
		if (shape == null)
			return;
		if(_meshFilters == null || _meshFilters.Length == 0)
			_meshFilters = new MeshFilter[6];
		_terrainFaces = new TerrainFace[6];

		_shapeGenerator.UpdateSettings(shape);
		_colorGenerator.UpdateSettings(color);

		Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

		for (int i = 0; i < 6; i++)
		{
			if (_meshFilters[i] == null)
			{
				GameObject meshObj = new GameObject("mesh");
				meshObj.transform.SetParent(transform, false);

				meshObj.AddComponent<MeshRenderer>();
				_meshFilters[i] = meshObj.AddComponent<MeshFilter>();
				_meshFilters[i].sharedMesh = new Mesh();
			}

			_meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = color.planetMaterial;
			_terrainFaces[i] = new TerrainFace(_shapeGenerator, _meshFilters[i].sharedMesh, resolution, directions[i]);
			bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
			_meshFilters[i].gameObject.SetActive(renderFace);
		}
	}

	private void Start()
	{
		GeneratePlanet();
	}

	public void OnColorSettingsUpdated()
	{
		if (!autoUpdate)
			return;
		Init();
		GenerateColors();
	}

	public void OnShapeSettingsUpdated()
	{
		if (!autoUpdate)
			return;
		Init();
		GenerateMesh();
	}

	public void GeneratePlanet()
	{
		Init();
		GenerateMesh();
		GenerateColors();
	}

	void GenerateMesh()
	{
		for (int i = 0; i < 6; i++)
		{
			if (_meshFilters[i].gameObject.activeInHierarchy)
			{
				_terrainFaces[i].ConstructMesh();
				_meshFilters[i].transform.localPosition = Vector3.zero;
			}
		}

		_colorGenerator.UpdateElevation(_shapeGenerator.elevationMinMax);
	}


	void GenerateColors()
	{
		_colorGenerator.UpdateColors();
		for (int i = 0; i < 6; i++)
		{
			if (_meshFilters[i].gameObject.activeInHierarchy)
				_terrainFaces[i].UpdateUVs(_colorGenerator);
		}
	}
}
