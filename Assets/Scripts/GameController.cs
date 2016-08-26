using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameController : Singleton<GameController> {

	[SerializeField] GameObject Player;
	[SerializeField] bool debug = false;
	public Action startGame;
	public Action resetGame;
	public Action loadGame;
	public Action<int> addScore;
	public Action<int> addCoin;
	public List<Car> carsList = null;
	private bool start = false;
	private bool load = false;
	private int _coin;
	public int coin
	{
		get { return _coin; }
		private set
		{
			_coin = value;
			PlayerPrefs.SetInt("coins", coin);
			if (addCoin != null)
				addCoin(coin);
		}
	}
	public int score { get; private set;}
	private int best = 0;
	public bool newRecord = false;
	private float pauseTime = 0f;



	private Vector3 playerStartPosition = new Vector3 (0.874f, -2f, 0f);

	// Use this for initialization
	void Awake () {
		if (debug) {
			PlayerPrefs.DeleteAll ();
		}

		Singleton<TouchController>.instance.touchEvent += ScreenPress;
		best = PlayerPrefs.GetInt ("bestscore",0);
		coin = PlayerPrefs.GetInt ("coins",0);
	}

	void OnDestroy(){
		Singleton<TouchController>.instance.touchEvent -= ScreenPress;
	}

	void Start(){
		LevelLoad ();
	}

	public void LevelLoad(){
		score = 0;
		newRecord = false;
		Player.transform.position = playerStartPosition;
		Player.SetActive (true);
		Singleton<GameUI>.instance.UpdateBest (best);

		if (Singleton<CarsController>.instance.currentCar.carName == "Random")
			Player.GetComponent<SpriteRenderer> ().sprite = Singleton<CarsController>.instance.GetRandomCar ().carSprite;
		else
			Player.GetComponent<SpriteRenderer> ().sprite = Singleton<CarsController>.instance.currentCar.carSprite;
		
		if (addCoin != null) addCoin (coin);
		if (addScore != null) addScore (score);
		if(loadGame != null) loadGame ();
		 
		load = true;
	}
		

	void ScreenPress (int touchIndex, TouchType touchType){
		if (!start && touchType == TouchType.Began && load) {
			if (!Singleton<TutorialController>.instance.tutorialComlete)
				Singleton<TutorialController>.instance.NextStage ();
			start = true;
			if(startGame != null)
				startGame();
		}
	}
	

	public void Crash () {
		
		load = false;
		start = false;
		if (best < score) {
			best = score;
			newRecord = true;
			PlayerPrefs.SetInt ("bestscore", best);
			Singleton<GameUI>.instance.UpdateBest (score);
			Singleton<PanelController>.instance.OpenPanel ("NewTop");
			Singleton<GameCenterController>.instance.CheckAchievement (Achievements.Loser, score);
		}
		if(resetGame != null)
			resetGame ();
	}



	public void AddCoin () {
		coin++;
	}
	public void AddCoin (int count) {
		coin += count;
	}
	public void SubtractCoin (int count)
	{
		coin -= count;
	}



	public void AddScore () {
		score++;
		if (addScore != null)
			addScore (score);
	}

	void OnApplicationPause( bool pauseStatus )
	{
		if (pauseStatus && start && pauseTime == 0f) {
			pauseTime = Time.timeScale;
			Singleton<PanelController>.instance.OpenPanel ("Pause");
			Time.timeScale = 0f;
		}
	}

	public void PauseOver(){
		if (pauseTime != 0f) {
			Time.timeScale = pauseTime;
			pauseTime = 0f;
			Singleton<PanelController>.instance.ClosePanel ("Pause");
		}
	}
}
