using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Yarn.Unity;

public class Compass : MonoBehaviour
{
	public GameObject display;
	public Transform target;

	public static Compass Inst
	{
		get
		{
			if (_inst == null)
				return _inst = GameObject.FindFirstObjectByType<Compass>();
			return _inst;
		}
	}

	private static Compass _inst;

	private void Awake()
	{
		_inst = this;
		display.SetActive(target != null);
	}

	private void Update()
	{
		if (target == null)
		{
			if(display.activeInHierarchy)
				display.SetActive(false);
			return;
		}
		var pos = target.position;
		pos.y = 0;
		display.transform.LookAt(pos);
	}


	public static void SetTarget(Transform transform)
	{
		Inst.target = transform;
		Inst.display.SetActive(true);
		Debug.Log($"Setting Compass target to <color='white'>{transform.name}</color>", transform);
	}

	[YarnCommand("SetCompass")]
    public static void YarnSetTarget(string planet)
	{
		if (Enum.TryParse<YarnManager.Planet>(planet, out var p))
			SetTarget(GameManager.GetPlanet(p));
		else
			Debug.LogError($"Failed to set compass, could not parse <color=magenta>'{planet}'</color> to a planet");
	}


    public static void ClearTarget()
	{
		Inst.target = null;
		Inst.display.SetActive(false);
	}

	public static void Hide()
	{
		Inst.display.SetActive(false);
	}

	public static void Show()
	{
		if (Inst.tag != null)
			Inst.display.SetActive(true);
	}
}