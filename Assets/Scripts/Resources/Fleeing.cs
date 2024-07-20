using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Fleeing : MonoBehaviour
{
	public float confinementRange = 5;
	public float fleeRange = 2;
	public float maxSpeed = 10;


	private Vector3 _move;
	private Vector3 _vel;
	private Transform _transform;
	private Vector3 _basePos;

	private void Start()
	{
		_transform = transform;
		_basePos = transform.position;
	}

	private void OnValidate()
	{
		var sphereCollider = GetComponent<SphereCollider>();
		sphereCollider.isTrigger = true;
		sphereCollider.radius = fleeRange;
	}

	private void OnTriggerStay(Collider other)
	{
		if (!other.CompareTag("Player"))
			return;
		Avoidance(other.transform.position);
	}

	private void Update()
	{
		Debug.DrawRay(_basePos, Vector3.up, Color.yellow);
	}

	private void FixedUpdate()
	{
		Confine();
		LimitSpeed();
		Recenter();

		_transform.Translate(_move * Time.deltaTime, Space.World);
		//_move = Vector3.zero;
	}

	private void Recenter()
	{
		var dir = _transform.position - _basePos;
		_move -= dir * Time.deltaTime;
	}


	private void Avoidance(Vector3 pos)
	{
		var dir = _transform.position - pos;
		_move += dir;
	}

	private void Confine()
	{
		var dir = _basePos - _transform.position;
		var dist = dir.magnitude;
		if(dist < confinementRange)
			return;

		_move += dir;
	}

	private void LimitSpeed()
	{
		var m = _move.magnitude;
		if (m > maxSpeed)
			_move = _move.normalized * maxSpeed;
	}
}
