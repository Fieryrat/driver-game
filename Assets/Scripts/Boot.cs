using UnityEngine;
using System.Collections;

public class Boot : MonoBehaviour {

	public RecycleState recycleState { get; private set; }
	[SerializeField] bool color = false;
	private int tweenID = -1;
	//private float speed = 5f;

	public void Recycle()
	{
		recycleState = RecycleState.Recycled;
		if (tweenID != -1)
			LeanTween.cancel (tweenID);
		gameObject.transform.position = GameConstants.POS_OFF_SCREEN;
		gameObject.SetActive(false);
	}

	public void Spawn(Vector3 position, float spd, Color clr)
	{
		//speed = spd;
		recycleState = RecycleState.Spawned;
		gameObject.transform.position = position;
		gameObject.SetActive(true);
		if (color)
			gameObject.GetComponent<SpriteRenderer> ().color = clr;
		tweenID = LeanTween.moveLocalY (gameObject, -(position.y), spd).setEase (LeanTweenType.linear).setOnComplete (Recycle).id;
	}


	public enum RecycleState
	{
		Recycled,
		Spawned
	}
}
