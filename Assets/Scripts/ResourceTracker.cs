using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class ResourceTracker 
{

	public static ResourceTracker Inst
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
		Inst._resources[type] += amount;
		if(Inst._resources[type] < 0)
			Inst._resources[type] = 0;
		return Inst._resources[type];
	}

	/// <summary>
	/// Check there is a certain amount of a given resouce type
	/// </summary>
	/// <param name="type"></param>
	/// <param name="qty"></param>
	/// <returns></returns>
	public static bool HasResource(Resource type, int qty = 1) 
	{ 
		return Inst._resources[type] >= qty;
	}

	/// <summary>
	/// Get the current qty of a specific resource
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public static int GetResouceCount(Resource type)
	{
		return Inst._resources[type];
	}


}


public enum Resource
{
	Voidmoss,
	Quirkrock, 
	SG4R,
}
