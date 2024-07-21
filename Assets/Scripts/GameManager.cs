using Cinemachine;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
	public static bool IsInDialouge => Instance._isInDialouge;

	public InputActionReference interactAction;

	public Transform player;
	public YarnManager.Planet? selectedPlanet;
	public GameObject prompt;
	public CinemachineVirtualCamera planetCamera;

	private Dictionary<YarnManager.Planet, Transform> _planets;
	private bool _isInDialouge;

	private void Awake()
	{
		Instance = this;
		_planets = new();
		var planets = FindObjectsByType<PlanetInteraction>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
		foreach (var planet in planets)
		{
			if (!_planets.TryAdd(planet.planet, planet.transform))
				Debug.LogWarning($"Duplicate entry for <color='red'>{planet.planet}</color> found", planet);
			else
				Debug.Log($"Added entry for planet <color='White'>{planet.planet}</color>");
		}

#if DEBUG
		var first = GetPlanet(YarnManager.Planet.Alpha);
		if( first != null )
			Compass.SetTarget(first);
#endif
	}

	private void Start()
	{
		planetCamera.gameObject.SetActive(false);
		interactAction.action.performed += Interact;

		HidePrompt();
	}

	public void OnDialougeStart()
	{
		if(selectedPlanet is YarnManager.Planet planet)
		{
			planetCamera.gameObject.SetActive(true);
			var target = GetPlanet(planet);
			planetCamera.LookAt = target;
			planetCamera.Follow = target;
			Compass.Hide();
		}
		_isInDialouge = true;
	}

	public void OnDialougeStop()
	{
		_isInDialouge = false;
		planetCamera.gameObject.SetActive(false);
		Compass.Show();
	}

	private void Interact(InputAction.CallbackContext context)
	{
		if (IsInDialouge)
			return;
		if (selectedPlanet is YarnManager.Planet planet)
		{
			YarnManager.RunDialogue(planet);
			HidePrompt();
		}
	}

	private void Update()
	{
		if (selectedPlanet == null)
			return;


	}

	private void HidePrompt()
	{
		prompt.SetActive(false);
	}

	private void ShowPrompt()
	{
		prompt.SetActive(true);
	}

	public static void SetActivePlanet(YarnManager.Planet planet)
	{
		Instance.selectedPlanet = planet;
		Instance.ShowPrompt();
	}

	public static void ClearSelectedPlanet()
	{
		Instance.selectedPlanet = null;
		Instance.HidePrompt();
	}

	public static Transform GetPlanet(YarnManager.Planet planet)
	{
		if(Instance._planets.TryGetValue(planet, out var transform))
			return transform;
		return null;
	}
}