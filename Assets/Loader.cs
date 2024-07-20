using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class Loader : MonoBehaviour
{
	public string[] scenePaths = new[]
	{
		"Assets/Scenes/Base.unity",
		"Assets/Scenes/Enviroment.unity",
	};

	private void Start()
	{
		foreach (var path in scenePaths)
		{
			var scene = SceneManager.GetSceneByPath(path);
#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				if (!scene.isLoaded)
					EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
			}
#else
			SceneManager.LoadScene(scene.buildIndex, LoadSceneMode.Additive);
#endif
		}
	}
}