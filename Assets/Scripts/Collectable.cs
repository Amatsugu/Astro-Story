using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Collectable : MonoBehaviour
{
	public Resource resource;
	public int value;
	
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
		ResourceTracker.ModifyResource(resource, value);

		ResourceTracker.ModifyResource(Resource.ResouceA, 10);
		Destroy(gameObject);
	}

}
