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

	public static void ClearTarget()
	{
		Inst.target = null;
		Inst.display.SetActive(false);
	}
}
