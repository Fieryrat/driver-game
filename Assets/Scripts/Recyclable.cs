using UnityEngine;
using System.Collections;
using System;

public class Recyclable : MonoBehaviour
{
	public Action<Recyclable> OnRecycle;
	public RecycleState recycleState { get; private set; }

	public virtual void Recycle()
	{
		this.recycleState = RecycleState.Recycled;
		if (this.OnRecycle != null)
		{
			this.OnRecycle(this);
		}
		base.transform.position = GameConstants.POS_OFF_SCREEN;
		base.gameObject.SetActive(false);
	}

	public virtual void Spawn()
	{
		this.recycleState = RecycleState.Spawned;
		base.gameObject.SetActive(true);
	}

	public enum RecycleState
	{
		Recycled,
		Spawned
	}
}