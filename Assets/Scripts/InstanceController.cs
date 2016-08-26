using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class InstanceController : MonoBehaviour
{
	// Fields
	private Dictionary<Type, object> allInstances = new Dictionary<Type, object>();
	private static bool applicationIsQuitting;
	private static InstanceController sharedInstance;

	private static InstanceController instance
	{
		get
		{
			if ((sharedInstance == null) && !applicationIsQuitting)
			{
				sharedInstance = UnityEngine.Object.FindObjectOfType<InstanceController>();
				foreach (Component component in sharedInstance.GetComponentsInChildren(typeof(MonoBehaviour)))
				{
					sharedInstance.allInstances[component.GetType()] = component;
				}
			}
			return sharedInstance;
		}
	}
		
	public static object GetInstance(Type instanceType)
	{
		object obj2 = null;
		if (instance.allInstances.ContainsKey(instanceType))
		{
			obj2 = instance.allInstances[instanceType];
		}
		if (obj2 == null)
		{
			obj2 = UnityEngine.Object.FindObjectOfType(instanceType);
			instance.allInstances[instanceType] = obj2;
		}
		return obj2;
	}

	private void Awake()
	{
		applicationIsQuitting = false;
	}


	private void OnDestroy()
	{
		applicationIsQuitting = true;
	}

	public static void RegisterInstance(object instance)
	{
		InstanceController.instance.allInstances[instance.GetType()] = instance;
	}
		
}