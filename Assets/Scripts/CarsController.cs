using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CarsController : Singleton<CarsController> {

	public List<Car> listCars;
	public Car currentCar;
	public Car buyCar;
	public Car prizeCar = null;
	public int unlockedCars { get; private set; }

	void Awake (){
		listCars = new List<Car> ();
		InfoDoc infoDoc = Tools.ParseDocument("CarsInfo");
		string loadCar = PlayerPrefs.GetString ("currentcar", "Classic");
		for (int i = 0; i < infoDoc.GetRowCount(); i++)
		{
			string carname;
			string carsku;
			string spritename = "";
			int secret = 0;
			infoDoc.TryGetCell<string> (i, "CarName", out carname, false);
			infoDoc.TryGetCell<string> (i, "SKU", out carsku, false);
			infoDoc.TryGetCell<int> (i, "Secret", out secret, false);
			if (infoDoc.TryGetCell<string>(i, "CarSprite", out spritename, false))
			{
				Sprite carsprite = Resources.Load<Sprite>("Sprites/Cars/" + spritename);
				listCars.Add (new Car (carname, carsprite, Convert.ToBoolean(secret), carsku));

				if (loadCar == carname)
					currentCar = listCars [listCars.Count - 1];
			}
		}

		Singleton<GameController>.instance.loadGame += SetPrizeCar;
	}
		
	void OnDestroy(){
		Singleton<GameController>.instance.loadGame -= SetPrizeCar;
	}

	public void SelectCar (Car car){
		currentCar = car;
		PlayerPrefs.SetString ("currentcar", car.carName);
		Singleton<GameController>.instance.LevelLoad ();
	}

	public void BuyCar (Car car){
		buyCar = car;
		//single inApp
	}

	public Car GetRandomCar(){
		Car randomCar = null;
		if (listCars != null) {
			do {
				randomCar = listCars [UnityEngine.Random.Range (0, listCars.Count)];
			} while (!randomCar.unlocked || randomCar.carName == "Random");
		}
		return randomCar;
	}


	void SetPrizeCar(){
		if (listCars != null) {
			do {
				prizeCar = listCars [UnityEngine.Random.Range (0, listCars.Count)];
			} while (prizeCar.secret == true || prizeCar.carName == "Random" || prizeCar.carName == "Classic");
		}
	}
	public void UnlockPrizeCar ()
	{
		if (prizeCar != null) {
			Singleton<GameController>.instance.SubtractCoin (100);
			prizeCar.UnlockCar ();
			RecountUnlocked ();
			SetPrizeCar ();
		}
	}

	private void RecountUnlocked ()
	{
		unlockedCars = 0;
		listCars.ForEach ((Car obj) => { if (obj.unlocked) unlockedCars++; });
		Singleton<GameCenterController>.instance.CheckAchievement (Achievements.Collector, unlockedCars);
	}
}
