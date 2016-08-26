using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DecorationsQueue : MonoBehaviour {
	
	[SerializeField] int objectsCount = 10;
	[SerializeField] GameObject[] decorationsArray;
	[SerializeField] float decorationsSpeed = 5f;
	[SerializeField] float delay = 0.05f;



	private Dictionary<string,Decoration[]> objectsArray;
	private Dictionary<string,int> objectsLastIndex;
	private bool left = true;
	private float currentZ = 0f;
	private float maxZ = 2f;

	void Awake () {
		objectsArray = new Dictionary<string,Decoration[]> ();
		objectsLastIndex = new Dictionary<string,int> ();
		foreach (GameObject decor in decorationsArray)
		{
			objectsArray[decor.name] = new Decoration[objectsCount];
			objectsLastIndex [decor.name] = 0;
			for (int i = 0; i < objectsCount; i++) {
				GameObject _obj = UnityEngine.Object.Instantiate(decor, GameConstants.POS_OFF_SCREEN, Quaternion.identity) as GameObject;
				_obj.transform.SetParent (gameObject.transform);
				objectsArray [decor.name] [i] = _obj.GetComponent<Decoration> ();
				objectsArray [decor.name] [i].Recycle ();
			}
		}
	}

	void Start(){
		StartCoroutine("SpawnTree");
		StartCoroutine("SpawnGarbage");
	}

	//Первые 4 элемента в массиве префабов доолжны быть деревьями, а все остальное - мусор.

	IEnumerator SpawnTree()
	{
		while(true){
			string currentDecorationName = decorationsArray [Random.Range (0, 3)].name;
			Decoration currentDecorationObject = GetRecycled (currentDecorationName);
			if (currentDecorationObject != null) {
				currentDecorationObject.Spawn (PositionGenerator("tree"), decorationsSpeed);
			}
			yield return new WaitForSeconds (Random.Range (delay, delay + 0.1f));
		}
	}
	IEnumerator SpawnGarbage()
	{
		
		while(true){
			yield return new WaitForSeconds (Random.Range (0.3f, 1f));
			string currentDecorationName = decorationsArray [Random.Range (3, decorationsArray.Length - 1)].name;
			Decoration currentDecorationObject = GetRecycled (currentDecorationName);
			if (currentDecorationObject != null) {
				currentDecorationObject.Spawn (PositionGenerator("garbage"), decorationsSpeed);
			}
		}
	}

	Vector3 PositionGenerator(string name){
		left = !left;
		currentZ += 0.01f;
		if (currentZ >= maxZ) {
			currentZ = 0f;
		}
		switch (name) {
		case "tree":
			if (left)
				return new Vector3 (-(Random.Range (2.15f, 2.51f)),15.94f,currentZ);
			else
				return new Vector3 (Random.Range (2.15f, 2.51f),15.94f,currentZ);
			//break;
		case "garbage":
			if (left)
				return new Vector3 (-(Random.Range (1.75f, 1.8f)),15.94f,currentZ);
			else
				return new Vector3 (Random.Range (1.75f, 1.8f),15.94f,currentZ);
			//break;
		default:
			return GameConstants.POS_OFF_SCREEN;
			//break;
		}
	}

	public Decoration GetRecycled(string name)
	{
		for (int i = 0; i < objectsCount; i++)
		{
			int index = (objectsLastIndex[name] + i) % objectsCount;
			if (objectsArray[name][index].recycleState == Decoration.RecycleState.Recycled)
			{
				Decoration local = objectsArray[name][index];
				objectsLastIndex[name] = index;
				return local;
			}
		}
		return null;
	}
}
