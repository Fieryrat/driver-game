using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
	// Fields
	private static T _instance;

	// Properties
	public static T instance
	{
		get
		{
			if (Singleton<T>._instance == null)
			{
				Singleton<T>._instance = InstanceController.GetInstance(typeof(T)) as T;
			}
			return Singleton<T>._instance;
		}
	}
}



