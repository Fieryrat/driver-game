using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour {

	[SerializeField] RectTransform Cover;
	
	public void Open () {
		LeanTween.move (Cover, new Vector2(0,100f), 1.5f).setOnComplete (AfterOpening);
	}

	private void AfterOpening (){
		Cover.anchoredPosition = Vector3.zero;
		gameObject.transform.parent.GetComponent<Prize>().FadeOut();
		gameObject.SetActive(false);
	}

}
