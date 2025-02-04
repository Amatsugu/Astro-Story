﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator
{
	ColorSettings settings;
	Texture2D texture;
	const int textureRes = 64;
	INoiseFilter biomeNoiseFilter;

	public void UpdateSettings(ColorSettings settings)
	{
		this.settings = settings;
		if(texture == null || texture.height != settings.biomeColorSettings.biomes.Length)
			texture = new Texture2D(textureRes*2, settings.biomeColorSettings.biomes.Length, TextureFormat.RGBA32, false);
		biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.biomeColorSettings.noise);
	}

	public void UpdateElevation(MinMax elevation)
	{
		settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevation.Min, elevation.Max));
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="point">Point on unit sphere</param>
	/// <returns></returns>
	public float BiomePercentFromPoint(Vector3 point)
	{
		float heightPercent = (point.y + 1) / 2f;
		heightPercent += (biomeNoiseFilter.Evaluate(point) - settings.biomeColorSettings.noiseOffset) * settings.biomeColorSettings.noiseStrength;
		float biomeIndex = 0;
		int biomeCount = settings.biomeColorSettings.biomes.Length;

		float blendRange = settings.biomeColorSettings.blendAmmount / 2f + 0.001f;

		for (int i = 0; i < biomeCount; i++)
		{
			float dst = heightPercent - settings.biomeColorSettings.biomes[i].startHeight;
			float weight = Mathf.InverseLerp(-blendRange, blendRange, dst);
			biomeIndex *= (1 - weight);
			biomeIndex += i * weight;
		}

		return biomeIndex / Mathf.Max(1, biomeCount - 1);
	}

	public void UpdateColors()
	{
		Color[] colors = new Color[texture.width * texture.height];
		int colorIndex = 0;
		foreach (var biome in settings.biomeColorSettings.biomes)
		{
			for (int i = 0; i < textureRes * 2; i++)
			{
				Color gradientCol;
				if (i < textureRes)
				{
					gradientCol = settings.oceanGradient.Evaluate(i / (textureRes - 1f));
				}
				else
				{
					gradientCol = biome.gradient.Evaluate((i - textureRes) / (textureRes - 1f));
				}
				var tint = biome.tint;
				colors[colorIndex++] = gradientCol * (1 - tint.a) + tint * tint.a;
			}
		}
		texture.SetPixels(colors);
		texture.Apply();
		settings.planetMaterial.SetTexture("_texture", texture);
	}
}
