using UnityEngine;
using System;
using System.Collections;

public class TutorialController : Singleton<TutorialController>
{
	public bool tutorialComlete = false;
	[SerializeField]
	GameObject [] stages;
	[SerializeField]
	GameObject  tutorial;
	public int stage = 0;

	void Awake ()
	{
		tutorialComlete = Convert.ToBoolean (PlayerPrefs.GetInt ("tutorial", 0));
	}

	// Use this for initialization
	public void CloseStage () {
		if (stage == stages.Length - 1) {
			tutorialComlete = true;
			PlayerPrefs.SetInt ("tutorial", 1);
			Singleton<GameController>.instance.startGame ();
		} else if (stage == 0) {
			Singleton<GameController>.instance.startGame ();
		}
			
		stages [stage].SetActive (false);
		tutorial.SetActive (false);
		stage++;
	}

	public void NextStage ()
	{
		if (stage < stages.Length) {
			stages [stage].SetActive (true);
			tutorial.SetActive (true);
		}
	}
}
