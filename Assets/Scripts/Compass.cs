using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
	public GameObject display;
	public Transform target;

	public static Compass Inst
	{
		get
		{
			if(_inst == null)
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


	void Update()
    {
		if (target == null)
			return;

		display.transform.LookAt(target.position);
    }

	public static void SetTarget(Transform transform)
	{
		Inst.target = transform;
		Inst.display.SetActive(true);
	}

	public static void ClearTarget()
	{
		Inst.target = null;
		Inst.display.SetActive(false);
	}
}
