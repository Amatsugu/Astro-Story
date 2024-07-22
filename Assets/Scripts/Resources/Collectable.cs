using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Collectable : MonoBehaviour
{
	public Resource resource;
	public int value;
	public bool killParent;
	public AudioClip pickupSound;
	private Collider _collider;

	private void OnValidate()
	{
		GetComponent<Collider>().isTrigger = true;
	}

	private void Start()
	{
		_collider = GetComponent<Collider>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player"))
			return;
		if (pickupSound != null)
		{
			var player = other.gameObject.GetComponent<PlayerController>();
			player.PlayPickupSound(pickupSound);
		}
		ResourceTracker.ModifyResource(resource, value);

		if (killParent)
			Destroy(gameObject.transform.parent.gameObject);
		else
			Destroy(gameObject);
	}
}