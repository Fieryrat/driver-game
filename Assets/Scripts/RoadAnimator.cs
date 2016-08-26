using UnityEngine;
using System.Collections;

public class RoadAnimator : MonoBehaviour {

	[SerializeField] GameObject RoadPrefab;
	[SerializeField] float speed = 2f;
	[SerializeField] float waiting = 0.2f;
	private GameObject Grass1,Grass2;


	void Start () {
		StartCoroutine ("CreateRoad");
	}

	IEnumerator CreateRoad ()
	{
		//Grass1 = UnityEngine.Object.Instantiate (RoadPrefab, new Vector3 (0f, 15.94f, 0f), Quaternion.identity) as GameObject;
		Grass1 = (GameObject)Instantiate (RoadPrefab, new Vector3 (0f, 15.94f, 0f), Quaternion.identity);
		Grass1.transform.SetParent (gameObject.transform);
		LeanTween.moveLocalY (Grass1, -15.94f, speed).setEase (LeanTweenType.linear).setLoopClamp ();
		yield return new WaitForSeconds (speed/2 + waiting);
		//Grass2 = UnityEngine.Object.Instantiate (RoadPrefab, new Vector3 (0f, 15.94f, 0f), Quaternion.identity) as GameObject;
		Grass2 = (GameObject)Instantiate (RoadPrefab, new Vector3 (0f, 15.94f, 0f), Quaternion.identity);
		Grass2.transform.SetParent (gameObject.transform);
		LeanTween.moveLocalY (Grass2, -15.94f, speed).setEase (LeanTweenType.linear).setLoopClamp ();
	}
}
