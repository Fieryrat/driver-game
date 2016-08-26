using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameUI : Singleton<GameUI> {

	public Action reloadUI;

	[SerializeField]
	GameObject [] uiObjects;
	private Dictionary<string, GameObject> uiDict;

	[SerializeField] Text scoreText;
	[SerializeField] Text coinText;
	[SerializeField] Text bestText;

	[SerializeField]
	GameObject prizeButton;
	[SerializeField]
	Text prizeCount;

	private float scoreX;




	// Use this for initialization
	void Awake () {
		Singleton<GameController>.instance.addScore += UpdateScore;
		Singleton<GameController>.instance.addCoin += UpdateCoins;
		Singleton<GameController>.instance.startGame += StartGame;
		Singleton<GameController>.instance.resetGame += StopGame;
		Singleton<GameController>.instance.loadGame += LoadLevel;

		uiDict = new Dictionary<string, GameObject> ();
		foreach (GameObject ui in uiObjects) {
			uiDict [ui.name] = ui; 
		}
	}

	void OnDestroy () {
		Singleton<GameController>.instance.addScore -= UpdateScore;
		Singleton<GameController>.instance.addCoin -= UpdateCoins;
		Singleton<GameController>.instance.startGame -= StartGame;
		Singleton<GameController>.instance.resetGame -= StopGame;
		Singleton<GameController>.instance.loadGame -= LoadLevel;	

	}
	
	void UpdateScore (int score) {
		scoreText.text = score.ToString ();
	}

	void UpdateCoins (int coins) {
		coinText.text = coins.ToString ();
		CheckPrize (coins);
	}

	public void UpdateBest(int score){
		bestText.text = "Top: " + score.ToString ();
	}

	public void CheckPrize (int coins)
	{
		if (coins >= 100) {
			prizeCount.text = (coins / 100).ToString ();
			prizeButton.SetActive (true);
		} else {
			prizeButton.SetActive (false);
		}
	}

	void StartGame(){
		uiDict ["ReloadGame"].GetComponent<UIElemet> ().Move ();

		uiDict ["TapToPlay"].GetComponent<UIElemet> ().Move ();
		uiDict ["Start"].GetComponent<UIElemet> ().Move ();

		uiDict ["CoinBox"].GetComponent<UIElemet> ().Move ();
		uiDict ["ScoreBox"].GetComponent<UIElemet> ().Move ();
	}

	void StopGame(){
		uiDict ["Stop"].SetActive (true);
		uiDict ["Stop"].GetComponent<UIElemet> ().Move ();
	}

	void LoadLevel(){
		if (reloadUI != null) reloadUI ();

		//logo.transform.position = Vector3.zero;
		uiDict ["ReloadGame"].SetActive (true);
		uiDict ["ReloadGame"].GetComponent<Animator> ().Play ("Reload");


		//coins.transform.localPosition = new Vector2 (scoreX * 2f, 165f);
		//score.transform.localPosition = new Vector2 (scoreX * 2f, 130f);

		//stopButtons.transform.localPosition = new Vector3 (0f, -250f, 0f);
		uiDict ["Stop"].SetActive (false);
		//startButtons.transform.localPosition = new Vector3 (0f, -170f, 0f);
		uiDict ["Start"].SetActive (true);

		//pressToPlay.transform.localPosition = new Vector2 (0f, -150f);
		uiDict ["TapToPlay"].SetActive (true);
		scoreText.text = "0";
	}


}
