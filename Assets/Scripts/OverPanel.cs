using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OverPanel : MonoBehaviour
{

	void OnEnable ()
	{
		LeanTween.moveLocalX (gameObject, 0f, 0.6f).setEase (LeanTweenType.easeOutQuart).setOnComplete (() => gameObject.GetComponent<LayoutElement> ().ignoreLayout = false);
	}
}
