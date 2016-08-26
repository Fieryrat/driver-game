using UnityEngine;
using System.Collections;

public class UIElemet : MonoBehaviour {
	public Vector2 start;
	[SerializeField]
	Vector2 stop;
	[SerializeField]
	float speed;
	[SerializeField]
	LeanTweenType tweenType;
	private RectTransform rect;



	void Awake ()
	{
		Singleton<GameUI>.instance.reloadUI += LoadGame;

		rect = gameObject.GetComponent<RectTransform> ();
		start = rect.anchoredPosition;
	}

	void OnDestroy ()
	{
		Singleton<GameController>.instance.loadGame -= LoadGame;
	}

	// Use this for initialization
	public void Move () {
		LeanTween.move (rect, stop, speed).setEase (tweenType);
	}

	
	// Update is called once per frame
	void LoadGame () {
		gameObject.GetComponent<RectTransform> ().anchoredPosition = start;

	}
}
