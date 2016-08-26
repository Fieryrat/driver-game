using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopButton : MonoBehaviour {

	public GameObject Play;
	public GameObject Buy;
	public GameObject Lock;
	public string currentState { get; private set;}



	public void ChangeButtonState (string state) {
		currentState = state;
		switch (state) {
		case "buy":
			Play.SetActive (false);
			Lock.SetActive (false);
			Buy.SetActive (true);
			gameObject.GetComponent<Image> ().color = GameConstants.BUTTONCOLOR;
			gameObject.GetComponent<Shadow> ().effectColor = GameConstants.BUTTONSHADOWLCOLOR;
			gameObject.GetComponent<Button> ().interactable = true;
			break;
		case "play":
			Play.SetActive (true);
			Lock.SetActive (false);
			Buy.SetActive (false);
			gameObject.GetComponent<Image> ().color = GameConstants.BUTTONCOLOR;
			gameObject.GetComponent<Shadow> ().effectColor = GameConstants.BUTTONSHADOWLCOLOR;
			gameObject.GetComponent<Button> ().interactable = true;
			break;
		case "lock":
			Play.SetActive (false);
			Lock.SetActive (true);
			Buy.SetActive (false);
			gameObject.GetComponent<Image> ().color = Color.gray;
			gameObject.GetComponent<Shadow> ().effectColor = new Color (0f, 0f, 0f, 0.5f);
			gameObject.GetComponent<Button> ().interactable = false;
			break;
		}
	}
}
