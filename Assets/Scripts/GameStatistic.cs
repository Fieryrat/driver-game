using UnityEngine;
using System.Collections;

public class GameStatistic : Singleton<GameStatistic> {

	public int gamescount { get; private set;}
	public int launces { get; private set;}
	public int prize { get; private set;}
	public int allcoins { get; private set;}
	private int savedcoins = -1;
	
	// Use this for initialization
	void Awake () {
		Singleton<GameController>.instance.resetGame += AddGame;
		Singleton<GameController>.instance.addCoin += AddCoin;

		gamescount = PlayerPrefs.GetInt("gamescount",0);
		prize = PlayerPrefs.GetInt("prizecount",0);
		allcoins = PlayerPrefs.GetInt("allcoinsscount",0);
		launces = PlayerPrefs.GetInt("launcescount",0);
	}

	void OnDestroy () {
		Singleton<GameController>.instance.resetGame -= AddGame;
		Singleton<GameController>.instance.addCoin -= AddCoin;

	}

	void Start(){
		launces++;
		PlayerPrefs.SetInt("launcescount", launces);
	}

	public void AddGame(){
		gamescount++;
		PlayerPrefs.SetInt("gamescount", gamescount);
		Singleton<GameCenterController>.instance.CheckAchievement (Achievements.Inveterate, gamescount);
	}

	public void AddPrize(){
		prize++;
		PlayerPrefs.SetInt("prizecount", prize);
	}


	public void AddCoin(int coin){
		if (savedcoins == -1) {
			savedcoins = coin;
		} else {
			int dif = coin - savedcoins;
			if (dif > 0)
				allcoins += dif;
		}
		PlayerPrefs.SetInt("allcoinscount", allcoins);
		Singleton<GameCenterController>.instance.CheckAchievement (Achievements.Rich, allcoins);
	}

}
