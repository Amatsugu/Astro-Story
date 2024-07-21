using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(SphereCollider))]
public class Invisible : MonoBehaviour
{

	public float detectionRange = 5f;

	private MeshRenderer _meshRenderer;
	private Transform _transform;


	private void OnValidate()
	{
		var sphereCollider = GetComponent<SphereCollider>();
		sphereCollider.isTrigger = true;
		sphereCollider.radius = detectionRange;
	}

	void Start()
    {
		_transform = transform;
		_meshRenderer = GetComponent<MeshRenderer>();
		SetOpacity(0);
    }

    void Update()
    {
        
    }

	private void OnTriggerStay(Collider other)
	{
		Debug.Log(other.tag);
		if (!other.CompareTag("Player"))
			return;

		var d = Vector3.Distance(_transform.position, other.transform.position) / detectionRange;
		SetOpacity(1 - d);
	}

	private void OnTriggerExit(Collider other)
	{
		if (!other.CompareTag("Player"))
			return;
		SetOpacity(0);
	}

	private void SetOpacity(float opacity)
	{
		var material = _meshRenderer.material;
		var col = material.color;
		col.a = opacity;
		material.color = col;
	}
}
