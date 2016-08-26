using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System;

public class PauseScripts : MonoBehaviour, IPointerDownHandler {

	[SerializeField] Text PauseLabel;
	[SerializeField] GameObject tapToContinue;
	[SerializeField] float wait = 3;
	private bool timerstart = false;
	private float timer = 0f;

	void IPointerDownHandler.OnPointerDown (PointerEventData eventData)
	{
		if (!timerstart) {
			timer = wait;
			timerstart = true;
			tapToContinue.SetActive (false);
		}
	}

	void Update() {
		if (timerstart) {
			timer -= Time.fixedDeltaTime;
			PauseLabel.text = (Math.Floor(timer) + 1).ToString();
			if (timer <= 0f) {
				timerstart = false;
				Singleton<GameController>.instance.PauseOver ();
			}
		}
	}

	void OnApplicationPause (bool pauseStatus)
	{
		if (pauseStatus) {
			timerstart = false;
			OnEnable ();
		}
	}


	void OnEnable (){
		PauseLabel.text = "pause";
		tapToContinue.SetActive (true);
	}

}
