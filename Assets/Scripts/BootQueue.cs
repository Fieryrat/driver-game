using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BootQueue: MonoBehaviour {

	[SerializeField] int objectsCount = 7;
	[SerializeField] GameObject[] bootsArray;
	[SerializeField] Color[] colorArray;
	[SerializeField] float bootSpeed = 5f;
	[SerializeField] float chance = 0.5f;
	[SerializeField] float coinChance = 0.5f;
	[SerializeField] float distance = 0.7f;

	private Dictionary<string,Boot[]> objectsArray;
	private Dictionary<string,int> objectsLastIndex;

	private bool newgame = false;


	private void Awake()
	{
		Singleton<GameController>.instance.startGame += StartGame;
		Singleton<GameController>.instance.resetGame += ResetGame;
		Singleton<GameController>.instance.loadGame += LoadGame;


		objectsArray = new Dictionary<string,Boot[]> ();
		objectsLastIndex = new Dictionary<string,int> ();
		foreach (GameObject boot in bootsArray)
		{
			objectsArray[boot.name] = new Boot[objectsCount];
			objectsLastIndex [boot.name] = 0;
			for (int i = 0; i < objectsCount; i++) {
				GameObject _obj = UnityEngine.Object.Instantiate(boot, GameConstants.POS_OFF_SCREEN, Quaternion.identity) as GameObject;
				_obj.transform.SetParent (gameObject.transform);
				objectsArray [boot.name] [i] = _obj.GetComponent<Boot> ();
				objectsArray [boot.name] [i].Recycle ();
			}
		}
	}

	void OnDestroy(){
		Singleton<GameController>.instance.startGame -= StartGame;
		Singleton<GameController>.instance.resetGame -= RecycleAll;
		Singleton<GameController>.instance.loadGame -= LoadGame;

	}

	void LoadGame(){
		StopCoroutine ("BootCreateLoop");

		if (!newgame) {
			RecycleAll ();
			if (Singleton<TutorialController>.instance.tutorialComlete) {
				StartCoroutine ("WaitStartLoop");
			}
			newgame = true;
		}
	}

	void StartGame(){
		if (Singleton<TutorialController>.instance.tutorialComlete) {
			StopCoroutine ("WaitStartLoop");
			StartCoroutine ("BootCreateLoop");
		}
	}

	void ResetGame(){
		newgame = false;

	}

	IEnumerator WaitStartLoop(){
		float newX = 0f;
		while (true) {

			Boot currentBootObject = null;
				
			currentBootObject = GetRecycled ("Boot" + UnityEngine.Random.Range(1,4).ToString());
			if (currentBootObject != null)
				currentBootObject.Spawn (new Vector3 (newX, 15.94f, 0f), bootSpeed, colorArray[UnityEngine.Random.Range(0,colorArray.Length)]);
			currentBootObject = GetRecycled ("Counter");
			if (currentBootObject != null)
				currentBootObject.Spawn (new Vector3 (0f, 15.94f, 0f), bootSpeed, Color.white);
			
			if (newX == 0f)
				newX = -0.874f;
			else
				newX = 0f;
			
			//Ожидание
			yield return new WaitForSeconds (1.6f);
		}

	}

	IEnumerator BootCreateLoop(){

		while (true) {
			yield return new WaitForSeconds (distance);

			int carOnLineCount = 0;
			Boot currentBootObject = null;



			//Блядство ли?
			if (UnityEngine.Random.Range (0f, 1f) >= coinChance) {
				currentBootObject = GetRecycled ("Coin");
				if (currentBootObject != null)
					currentBootObject.Spawn (new Vector3 (UnityEngine.Random.Range (-0.874f, 0.874f), 15.94f, 0f), bootSpeed, Color.white);
			} else {
				for (float i = -0.874f; i <= 0.874f; i += 0.874f) {
					if (UnityEngine.Random.Range (0f, 1f) >= chance && carOnLineCount < 2) {
						string currentBootName = bootsArray [UnityEngine.Random.Range (1, bootsArray.Length - 1)].name;
						currentBootObject = GetRecycled (currentBootName);
						if (currentBootObject != null) {
							float randY = 15.94f + UnityEngine.Random.Range (-0.4f, 0.4f);
							currentBootObject.Spawn (new Vector3 (i, randY, 0f), bootSpeed, colorArray[UnityEngine.Random.Range(0,colorArray.Length)]);
							carOnLineCount++;
							continue;
						}
					}
				}
				if (carOnLineCount != 0) {
					currentBootObject = GetRecycled ("Counter");
					if (currentBootObject != null)
						currentBootObject.Spawn (new Vector3 (0f, 15.94f, 0f), bootSpeed, Color.white);
				}
			}
				
			//Ожидание

		}
	}


	public Boot GetRecycled(string name)
	{
		for (int i = 0; i < objectsCount; i++)
		{
			int index = (objectsLastIndex[name] + i) % objectsCount;
			if (objectsArray[name][index].recycleState == Boot.RecycleState.Recycled)
			{
				Boot local = objectsArray[name][index];
				objectsLastIndex[name] = index;
				return local;
			}
		}
		return null;
	}


	public void RecycleAll()
	{
		foreach (GameObject boot in bootsArray)
		{
			for (int i = 0; i < this.objectsArray[boot.name].Length; i++)
			{
				if (objectsArray [boot.name] [i].recycleState != Boot.RecycleState.Recycled)
				{
					objectsArray [boot.name] [i].Recycle();
				}
			}
		}
	}
}
