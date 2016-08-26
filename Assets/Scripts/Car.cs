using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;


public class Car {

	public string carName;
	public Sprite carSprite;
	public bool unlocked  = false;
	public bool secret = false;
	public string sku;

	public int price;

	public Car (string name, Sprite sprite, bool secr, string carsku) {
		carName = name;
		carSprite = sprite;
		secret = secr;
		sku = carsku;
		unlocked = Convert.ToBoolean(PlayerPrefs.GetInt (name, 0));
		if (name == "Random" || name == "Classic") {
			unlocked = true;
		}
	}

	public void UnlockCar(){
		PlayerPrefs.SetInt (carName, 1);
		unlocked = true;
	}
}
