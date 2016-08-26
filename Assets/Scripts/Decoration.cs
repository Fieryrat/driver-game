using UnityEngine;
using System.Collections;

public class Decoration : MonoBehaviour {

	public RecycleState recycleState { get; private set; }
	private float speed = 5f;

	public void Recycle()
	{
		this.recycleState = RecycleState.Recycled;

		base.transform.position = GameConstants.POS_OFF_SCREEN;
		base.gameObject.SetActive(false);
	}

	public void Spawn(Vector3 position, float decSpeed)
	{
		speed = decSpeed;
		this.recycleState = RecycleState.Spawned;
		gameObject.transform.position = position;
		base.gameObject.SetActive(true);
		LeanTween.moveLocalY (gameObject, -15.94f, speed).setEase (LeanTweenType.linear).setOnComplete (Recycle);
	}

	void OnEnable () {
		
	}

	public enum RecycleState
	{
		Recycled,
		Spawned
	}
}

