using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Collectable : MonoBehaviour
{
	public Resource resource;
	public int value;
	
	private Collider _collider;


	private void Start()
	{
		_collider = GetComponent<Collider>();
	}

	private void OnTriggerEnter(Collider other)
	{
		ResourceTracker.ModifyResource(resource, value);
		Destroy(gameObject);
	}

}
