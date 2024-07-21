using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
	Planet planet;
	Editor shapeEditor;
	Editor colorEditor;

	public override void OnInspectorGUI()
	{
		using (var check = new EditorGUI.ChangeCheckScope())
		{
			base.OnInspectorGUI();
			if (check.changed)
				planet.GeneratePlanet();
		}

		if (GUILayout.Button("Generate Planet"))
			planet.GeneratePlanet();

		DrawSettingsEditor(planet.shape, planet.OnShapeSettingsUpdated, ref planet.shapeSettingsFold, ref shapeEditor);
		DrawSettingsEditor(planet.color, planet.OnColorSettingsUpdated, ref planet.ColorSettingsFold, ref colorEditor);
	}

	public void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldOut, ref Editor editor)
	{
		if (settings == null)
			return;
		foldOut = EditorGUILayout.InspectorTitlebar(foldOut, settings);
		using (var check = new EditorGUI.ChangeCheckScope())
		{
			if (foldOut)
			{
				CreateCachedEditor(settings, null, ref editor);
				editor.OnInspectorGUI();
				if (check.changed)
				{
					onSettingsUpdated?.Invoke();
				}
			}
		}
	}

	private void OnEnable()
	{
		planet = target as Planet;
		if(planet.autoUpdate)
			planet.GeneratePlanet();
	}

	

}
