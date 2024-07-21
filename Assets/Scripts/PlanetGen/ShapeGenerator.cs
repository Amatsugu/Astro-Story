using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ShapeGenerator
{
	ShapeSettings settings;
	INoiseFilter[] noiseFilters;

	public MinMax elevationMinMax;

	public void UpdateSettings(ShapeSettings settings)
	{
		this.settings = settings;
		noiseFilters = new INoiseFilter[settings.noiseLayers.Length];
		for (int i = 0; i < settings.noiseLayers.Length; i++)
		{
			noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
		}
		elevationMinMax = new MinMax();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="point">Point on unit sphere</param>
	/// <returns></returns>
	public float CalculateUnscaledElevation(Vector3 point)
	{
		float elevation = 0;
		float firstLayer = 0;
		if(noiseFilters.Length > 0)
		{
			firstLayer = noiseFilters[0].Evaluate(point);
			if (settings.noiseLayers[0].enabled)
				elevation = firstLayer;
		}
		for (int i = 1; i < noiseFilters.Length; i++)
		{
			if(settings.noiseLayers[i].enabled)
			{
				float mask = (settings.noiseLayers[i].useFirstLayerAsMask) ? firstLayer : 1;
				elevation += noiseFilters[i].Evaluate(point) * mask;
			}
		}
		elevationMinMax.AddValue(elevation);
		return elevation;
	}

	public float GetScaledElevation(float unscaledElevation)
	{
		float elevation = Mathf.Max(0, unscaledElevation);
		elevation = settings.planetRadius * (1 + elevation);
		return elevation;
	}
}
