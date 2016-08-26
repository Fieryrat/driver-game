using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelController : Singleton<PanelController> {

	private GameObject currentOpenPanel = null;
	private Dictionary<string,GameObject> panelDictionary = new Dictionary<string,GameObject>();
	public GameObject[] panelsArray;
	[SerializeField]
	GameObject wait;
	private void Awake ()
	{
		foreach (GameObject panel in panelsArray)
		{
			panelDictionary [panel.name] = panel;
		}

		Singleton<GameController>.instance.loadGame += ClosePanel;
	}

	void OnDestroy(){
		Singleton<GameController>.instance.loadGame -= ClosePanel;
	}

	public void ClosePanel () {
		if (currentOpenPanel != null) {
			currentOpenPanel.SetActive (false);
			currentOpenPanel = null;
		}
	}

	public void ClosePanel (string panel) {
		if (currentOpenPanel != null && currentOpenPanel.name == panel) {
			currentOpenPanel.SetActive (false);
			currentOpenPanel = null;
		}
	}
	
	// Update is called once per frame
	public void OpenPanel (string panelIndex) {
		if (currentOpenPanel == null) {
			currentOpenPanel = panelDictionary [panelIndex];
			currentOpenPanel.SetActive (true);
		}
	}

	public void Wait (bool active)
	{
		wait.SetActive (active);
	}
}