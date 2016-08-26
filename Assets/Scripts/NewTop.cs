using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewTop : MonoBehaviour {

	[SerializeField] GameObject rays = null;
	[SerializeField] Text score = null;

	// Use this for initialization
	void OnEnable () {
		score.text = Singleton<GameController>.instance.score.ToString ();
		score.gameObject.transform.localScale = Vector3.zero;
		LeanTween.scale( score.gameObject, Vector3.one, 1f).setDelay(0.3f).setEase(LeanTweenType.easeOutElastic);
		LeanTween.rotateAround( rays, Vector3.forward, 360f, 10f).setLoopClamp();
	}
}
