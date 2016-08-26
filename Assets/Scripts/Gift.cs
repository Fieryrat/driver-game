using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Gift : MonoBehaviour {

	[SerializeField] GameObject rays = null;
	[SerializeField] GameObject coinsContainer = null;
	[SerializeField] Text coins = null;

	// Use this for initialization
	void OnEnable () {
		coins.text = Singleton<GiftController>.instance.gift.ToString ();
		coinsContainer.transform.localScale = Vector3.zero;
		LeanTween.scale( coinsContainer, Vector3.one, 1f).setDelay(0.3f).setEase(LeanTweenType.easeOutElastic);
		LeanTween.rotateAround(rays, Vector3.forward, 360f, 10f).setLoopClamp();
		Singleton<GiftController>.instance.GetGift ();
		if(Singleton<GiftController>.instance.Check ()) Singleton<GiftController>.instance.GetGift ();
	}
}
