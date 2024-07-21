using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class PlanetInteraction : MonoBehaviour
{
	public float range;
	public YarnManager.Planet planet;

	private void OnValidate()
	{
		var col = GetComponent<SphereCollider>();
		col.isTrigger = true;
		col.radius = range;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player"))
			return;
		GameManager.SetActivePlanet(planet);
	}

	private void OnTriggerExit(Collider other)
	{
		if (!other.CompareTag("Player"))
			return;
		GameManager.ClearSelectedPlanet();
	}
}
