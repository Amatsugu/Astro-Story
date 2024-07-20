using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class ResourceTracker 
{

	public ResourceTracker Inst
	{
		get { 
			if(_inst == null)
				return _inst = new ResourceTracker();
			return _inst;
		}
	}

	private static ResourceTracker _inst;


	private Dictionary<Resource, int> _resources;

	public ResourceTracker()
	{
		_resources = Enum.GetValues(typeof(Resource))
			.Cast<Resource>()
			.ToDictionary(v => v, _ => 0);
		
	}

	/// <summary>
	/// Add/Remove resources of a cerain type
	/// </summary>
	/// <param name="type"></param>
	/// <param name="amount">Positive values add, negative values remove</param>
	public static int ModifyResource(Resource type, int amount)
	{
		_inst._resources[type] += amount;
		if(_inst._resources[type] < 0) 
			_inst._resources[type] = 0;
		return _inst._resources[type];
	}

	/// <summary>
	/// Check there is a certain amount of a given resouce type
	/// </summary>
	/// <param name="type"></param>
	/// <param name="qty"></param>
	/// <returns></returns>
	public static bool HasResource(Resource type, int qty = 1) 
	{ 
		return _inst._resources[type] >= qty;
	}

	/// <summary>
	/// Get the current qty of a specific resource
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public static int GetResouceCount(Resource type)
	{
		return _inst._resources[type];
	}


}


public enum Resource
{
	ResouceA,
	ResouceB, 
	ResouceC,
}
