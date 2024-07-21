using Cinemachine;

using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
	public static bool IsInDialouge => Instance._isInDialouge;

	public InputActionReference interactAction;

	public Transform player;
	public YarnManager.Planet? selectedPlanet;
	public TextMeshProUGUI prompt;
	public CinemachineVirtualCamera planetCamera;

	private Dictionary<YarnManager.Planet, Transform> _planets;
	private bool _isInDialouge;

	private void Awake()
	{
		Instance = this;
		_planets = new();
	}

	public static void RegisterPlanet(PlanetInteraction planet)
	{
		if (Instance._planets == null)
			Instance._planets = new();
		if (!Instance._planets.TryAdd(planet.planet, planet.transform))
			Debug.LogWarning($"Duplicate entry for <color='red'>{planet.planet}</color> found", planet);
		else
			Debug.Log($"Added entry for planet <color='White'>{planet.planet}</color>");
	}

	private void Start()
	{
		Debug.Log("Game Start");
#if DEBUG
		var first = GetPlanet(YarnManager.Planet.Alpha);
		if (first != null)
			Compass.SetTarget(first);
#endif

		planetCamera.gameObject.SetActive(false);
		interactAction.action.performed += Interact;

		HidePrompt();
		selectedPlanet = YarnManager.Planet.Alpha;
		YarnManager.RunDialogue(YarnManager.Planet.Alpha);
	}

	public void OnDialougeStart()
	{
		Debug.Log("Dialouge Start");
		if (selectedPlanet is YarnManager.Planet planet)
		{
			HidePrompt();
			planetCamera.gameObject.SetActive(true);
			var target = GetPlanet(planet);
			planetCamera.LookAt = target;
			planetCamera.Follow = target;
			Compass.Hide();
			MusicManager.PlayThemeFor(planet);
		}
		_isInDialouge = true;
	}

	public void OnDialougeStop()
	{
		Debug.Log("Dialouge Stop");
		_isInDialouge = false;
		planetCamera.gameObject.SetActive(false);
		MusicManager.PlayMainTheme();
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
		prompt.gameObject.SetActive(false);
	}

	private void ShowPrompt()
	{
		if (IsInDialouge)
			return;
		prompt.SetText(selectedPlanet?.ToString() ?? "Interact");
		prompt.gameObject.SetActive(true);
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
		if (Instance._planets.TryGetValue(planet, out var transform))
			return transform;
		return null;
	}
}