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
	public (string path, int index)[] scenePaths = new[]
	{
		("Assets/Scenes/Base.unity", 1),
		("Assets/Scenes/Enviroment.unity", 2),
	};

	private void Start()
	{
		foreach (var (path, index) in scenePaths)
		{
#if UNITY_EDITOR
			var scene = SceneManager.GetSceneByPath(path);
			if (!Application.isPlaying)
			{
				if (!scene.isLoaded)
					EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
			}
#else
			SceneManager.LoadScene(index, LoadSceneMode.Additive);
#endif
		}
	}
}