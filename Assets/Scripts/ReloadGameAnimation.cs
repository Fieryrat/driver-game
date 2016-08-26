using UnityEngine;
using System.Collections;

public class ReloadGameAnimation : MonoBehaviour {

	private AudioSource noise = null;

	// Use this for initialization
	void Awake () {
		noise = gameObject.GetComponent<AudioSource> ();
		Singleton<GameController>.instance.loadGame += StartNoise;
	}
	void OnDestroy ()
	{
		Singleton<GameController>.instance.loadGame -= StartNoise;
	}

	public void StartNoise ()
	{
		noise.Play ();
	}

	public void StopNoise () {
		noise.Stop ();
	}
}
