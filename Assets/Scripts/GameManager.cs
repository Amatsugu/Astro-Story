using Cinemachine;

using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
	public static bool IsInDialouge => Instance._isInDialouge;
	public static bool IsEnd => Instance._isEnd;

	public InputActionReference interactAction;

	public Transform player;
	public GameObject endScreen;
	public YarnManager.Planet? selectedPlanet;
	public TextMeshProUGUI prompt;
	public CinemachineVirtualCamera planetCamera;

	private Dictionary<YarnManager.Planet, Transform> _planets;
	private bool _isInDialouge;
	private bool _isEnd;

	private void Awake()
	{
		Instance = this;
		endScreen.SetActive(false);
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

		if(planet.planet == YarnManager.Planet.Beta)
			planet.gameObject.SetActive(false);
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
		interactAction.action.started += Interact;

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
		selectedPlanet = null;
		MusicManager.PlayMainTheme();
		Compass.Show();
	}

	private void Interact(InputAction.CallbackContext context)
	{
		if (_isInDialouge)
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
		prompt.SetText(selectedPlanet == null ? "Interact" : GetPlanetNameString((YarnManager.Planet)selectedPlanet));
		prompt.gameObject.SetActive(true);
	}

	private string GetPlanetNameString(YarnManager.Planet planet)
	{
		var color = planet switch
		{
			YarnManager.Planet.Alpha => "#e657b4",
			YarnManager.Planet.Beta => "#e6c210",
			YarnManager.Planet.Gamma => "#008c07",
			YarnManager.Planet.Delta => "#a8133d",
			YarnManager.Planet.Epsilon => "#7a340b",
			_ => "#ffffff"
		};

		return $"<color={color}>{planet}</color>";
	}

	public static void SetActivePlanet(YarnManager.Planet planet)
	{
		if(Instance.selectedPlanet == planet) 
			return;
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

	public static void ShowEndScreen()
	{
		Instance.endScreen.SetActive(true);
		Instance._isEnd = true;
	}

	public static void ShowBeta() 
	{
		GetPlanet(YarnManager.Planet.Beta).gameObject.SetActive(true);
	}
}