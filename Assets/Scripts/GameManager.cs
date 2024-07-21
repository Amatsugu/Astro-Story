using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	public PlayerInput input;


	public YarnManager.Planet? selectedPlanet;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		
	}

	public static void SetActivePlanet(YarnManager.Planet planet)
	{
		Instance.selectedPlanet = planet;
	}


	public static void ClearSelectedPlanet()
	{
		Instance.selectedPlanet = null;
	}


}