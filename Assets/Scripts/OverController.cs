using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class OverController : Singleton<OverController> {


	public Action colorReport;

	[SerializeField] GameObject overParrent;
	[SerializeField]
	GameObject[] panelsArray;
	[SerializeField] float panelSpace = 10f;
	private float panelHeight = 0;
	private Dictionary<string, GameObject> panels;

	void Awake(){
		if (panelsArray.Length != 0)
			panelHeight = panelsArray[0].GetComponent<RectTransform> ().sizeDelta.y;
		Singleton<GameController>.instance.resetGame += StopGame;
		Singleton<GameController>.instance.loadGame += LoadGame;


		panels = new Dictionary<string, GameObject> ();
		foreach (GameObject panel in panelsArray) {
			panels [panel.name] = panel;
		}
	}

	void OnDestroy ()
	{
		Singleton<GameController>.instance.resetGame -= StopGame;
		Singleton<GameController>.instance.loadGame -= LoadGame;
	}

	public void CreatePanels(List<string> panelsList){
		for (int i = 0; i < panelsList.Count; i++) {
			float newX;
			float newY = GenerateY(panelsList.Count - 1, i);
			if (i % 2 == 0)
				newX = -400f;
			else
				newX = 400f;

			GameObject _obj = panels [panelsList[i]];
			_obj.transform.SetParent (overParrent.transform);
			_obj.transform.localScale = Vector3.one;
			_obj.transform.localPosition = new Vector2 (newX, newY);
			_obj.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0f,panelHeight);
			_obj.SetActive (true);
		}
	}

	public void ClosePanel (string panel)
	{
		panels [panel].SetActive (false);
	}



	private float GenerateY(int elementscount, int index){
		return ((panelHeight + panelSpace) * elementscount - panelSpace) / 2 - (panelHeight + panelSpace) * index;
	}


	public void StopGame(){
		overParrent.SetActive (true);
		List<string> panelsList = new List<string> ();

		if (Singleton<GameController>.instance.newRecord)
			panelsList.Add ("Record");
		if ( Singleton<GiftController>.instance.Check())
			panelsList.Add ("Gift");

		//panelsList.Add ("Record");
		//panelsList.Add ("AD");
		//panelsList.Add ("Gift");
		//panelsList.Add ("Rate");

			CreatePanels (panelsList);
	}

	public void LoadGame ()
	{
		overParrent.SetActive (false);
		foreach (GameObject panel in panelsArray) {
			panel.SetActive (false);
		}
	}
	/*
	public void TestColor(){
		if (colorReport != null)
			colorReport();
	}
	*/
}
