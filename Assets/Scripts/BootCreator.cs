using UnityEngine;
using System.Collections;

public class BootCreator: MonoBehaviour
{
	public GameObject[] Prefabs;
	public int[] BootAmount;
	public float[] Timer;
	public int TruckSuccessRate = 30;
	public int CoinSuccessRate = 20;

	public GameObject[,] Objects;
	int[] BootCounter;
	float CreateTime = 0.47f;
	int bootID = 0;
	string pervBoot = "car";
	float pervPos = -0.54f;
	float curPos = 0.54f;
	bool CoinSpace = false;

	void Start ()
	{
		float Objects_x = 10f;
		Objects = new GameObject[Prefabs.Length, MaxArrayElement (BootAmount)];
		BootCounter = new int[BootAmount.Length];
		for (int j = 0; j <= Prefabs.Length - 1; j++) {
			//Debug.Log ("J: " + j);
			for (int i = 1; i <= BootAmount [j]; i++) {
				//Debug.Log ("J: " + i);
				Objects [j, i - 1] = (GameObject)Instantiate (Prefabs [j], new Vector2 (Objects_x, 8f), Quaternion.identity);
				Objects_x += 1.2f;
			}
			BootCounter [j] = 0;
		}

	}

	public void GameStart ()
	{
		StartCoroutine_Auto (CROstr ());
	}

	IEnumerator CROstr ()
	{
		for (; ;) {
			CoinCreate ();
			yield return new WaitForSeconds (CreateTime);
			BootCreate ();
			yield return new WaitForSeconds (CreateTime);
		}
	}

	bool SuccessRateCalculator (int chance)
	{
		int random = Random.Range (0, 100);
		if (random < chance)
			return true;
		else
			return false;
	}

	int MaxArrayElement (int[] array)
	{
		int max = 0;
		for (int i = 0; i <= array.Length - 1; i++) {
			if (array [i] > max)
				max = array [i];
		}
		return max;
	}


	void BootCreate ()
	{
		if (SuccessRateCalculator (50)) {
			curPos = 0.54f;
		} else {
			curPos = -0.54f;
		}
		switch (bootID) {
		case 0:
			Sprite[] BootSpriteArray = Resources.LoadAll<Sprite> ("Boots");
			Objects [bootID, BootCounter [bootID]].GetComponent<SpriteRenderer> ().sprite = BootSpriteArray [Random.Range (0, BootSpriteArray.Length - 1)];
			break;
		case 1:
			Sprite[] TruckSpriteArray = Resources.LoadAll<Sprite> ("Trucks");
			Objects [bootID, BootCounter [bootID]].GetComponent<SpriteRenderer> ().sprite = TruckSpriteArray [Random.Range (0, TruckSpriteArray.Length - 1)];
			break;
		}


		Objects [bootID, BootCounter [bootID]].transform.position = new Vector2 (pervPos, 8f);
		if (pervPos == -0.54f)
			Objects [2, BootCounter [2]].transform.position = new Vector2 (0.54f, 8f);
		else
			Objects [2, BootCounter [2]].transform.position = new Vector2 (-0.54f, 8f);
			
		BootCounter [2]++;
		BootCounter [bootID]++;

		if (BootCounter [2] > BootAmount [2] - 1)
			BootCounter [2] = 0;
		if (BootCounter [bootID] > BootAmount [bootID] - 1)
			BootCounter [bootID] = 0;


		if (SuccessRateCalculator (TruckSuccessRate)) {
			bootID = 1;
			pervBoot = "truck";
			CreateTime = Timer [1];
		} else {
			if (pervBoot == "car" && pervPos == curPos)
				CoinSpace = true;
			else
				CoinSpace = false;
			bootID = 0;
			pervBoot = "car";
			CreateTime = Timer [0];

		}

		pervPos = curPos;

	}

	void CoinCreate ()
	{
		if (CoinSpace) {
			if (SuccessRateCalculator (CoinSuccessRate)) {
				Objects [3, BootCounter [3]].transform.position = new Vector2 (curPos, 8f);
				BootCounter [3]++;
				if (BootCounter [3] > BootAmount [3] - 1)
					BootCounter [3] = 0;
				CoinSpace = false;
			}
		}
	}

}