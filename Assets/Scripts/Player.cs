using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	[SerializeField]
	AudioSource coin;
	[SerializeField]
	AudioSource engine;
	private AudioSource scrr;
	[SerializeField]
	float speed = 1f;
	[SerializeField]
	float driftSpeed = 0.2f;
	private float sideX = 0.874f;
	private bool drive = false;
	private bool drift = false;
	private float tapWait = 0f;
	private Vector2 previousPosition = Vector2.zero;
	private TutorialController tutorial = null;
	private int currentGameDriftCount;

	void Awake(){
		Singleton<TouchController>.instance.touchEvent += ScreenTap;
		Singleton<GameController>.instance.startGame += StartGame;

		scrr = gameObject.GetComponent<AudioSource> ();
		tutorial = Singleton<TutorialController>.instance;
	}

	void OnDestroy(){
		Singleton<TouchController>.instance.touchEvent -= ScreenTap;
		Singleton<GameController>.instance.startGame -= StartGame;
	}

	private void StartGame ()
	{
		drive = true;
		engine.Stop ();
	}

	void OnEnable ()
	{
		engine.Play ();
		currentGameDriftCount = 0;
	}


	private void ScreenTap(int touchIndex, TouchType touchType){
		if (drive && touchType == TouchType.Began && !drift) {
			Vector2 newPosition = NextPosition ();

			if (!tutorial.tutorialComlete && (newPosition.x == 0.874f || newPosition.x == -0.874f))
			   tutorial.NextStage ();
			
			if (tutorial.tutorialComlete || tutorial.stage != 0)
				LeanTween.moveLocal (gameObject, newPosition, speed).setEase (LeanTweenType.linear);
			
			if (tapWait > 0.4f) {
				drift = true;
				currentGameDriftCount++;
				tapWait = 0.5f;

				if ((previousPosition.x - newPosition.x) > 0f)
					LeanTween.rotateAround (gameObject, Vector3.forward, -10f, driftSpeed).setLoopPingPong (1);
				else
					LeanTween.rotateAround (gameObject, Vector3.forward, 10f, driftSpeed).setLoopPingPong (1);

				scrr.Play ();
				//Debug.Log ("Drift = true");
			} else {
				tapWait += 0.4f;
			}
		}
	}

	private Vector2 NextPosition ()
	{
		float newX = 0f;
		if (!Mathf.Approximately(Mathf.Abs(gameObject.transform.position.x),sideX)) {
			if (previousPosition.x > 0f)
				newX = -sideX;
			else
				newX = sideX;
		} 
		previousPosition = gameObject.transform.position;
		return new Vector2 (newX, gameObject.transform.position.y);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		switch (other.tag) {
		case "Boot":
			drive = false;
			Singleton<GameController>.instance.Crash ();
			Singleton<GameCenterController>.instance.CheckAchievement (Achievements.Drift, currentGameDriftCount);
			other.gameObject.GetComponent<Boot> ().Recycle ();
			gameObject.SetActive (false);
			break;
		case "Coin":
			other.gameObject.GetComponent<Boot> ().Recycle ();
			coin.Play ();
			Singleton<GameController>.instance.AddCoin();
			break;
		case "Score":
			other.gameObject.GetComponent<Boot> ().Recycle ();
			if(drive)
				Singleton<GameController>.instance.AddScore();
			break;
		}
	}

	void Update ()
	{
		if (tapWait > 0f)
			tapWait -= Time.deltaTime;
		else if (tapWait <= 0f && drift)
			drift = false;
	}
}
