using UnityEngine;
using System;
using System.Collections;


public class GiftController : Singleton<GiftController> {

	private DateTime nextGift = DateTime.UtcNow;
	public int gift { get; private set;}

	void Awake () {
		if (PlayerPrefs.HasKey ("nextgift"))
			nextGift = Convert.ToDateTime (PlayerPrefs.GetString ("nextgift"));
		gift = GetRandomCoins ();
	}
	
	public bool Check () {
		TimeSpan Difference = nextGift.Subtract (DateTime.UtcNow);
		if (TimeSpan.Zero >= Difference)
			return true;
		return false;
	}

	public void GetGift(){
		if (Check()) {
			Singleton<OverController>.instance.ClosePanel ("Gift");
			Singleton<GameController>.instance.AddCoin (gift);
			gift = GetRandomCoins ();
			nextGift = DateTime.UtcNow.Add(TimeSpan.FromMinutes(35));
			PlayerPrefs.SetString ("nextgift", Convert.ToString(nextGift));
			//ANDROID
			//AndroidNotificationManager.instance.ScheduleLocalNotification ("Hello", "This is local notification", Convert.ToInt32(TimeSpan.FromHours (1).TotalSeconds));
		}
	}

	private int GetRandomCoins(){
		int rnd = 0;
		do{
			rnd = UnityEngine.Random.Range(25,60);
		} while (rnd % 5 != 0);
		return rnd;
	}

	public void TestCheck(){
		Debug.Log ("Gift available " + Check ());
	}
}
