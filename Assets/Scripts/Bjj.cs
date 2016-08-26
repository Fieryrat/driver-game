using UnityEngine;
using System.Collections;

public class Bjj : MonoBehaviour
{
	private AudioSource playerAudio;


	void Awake ()
	{
		playerAudio = gameObject.GetComponent<AudioSource> ();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Boot") {
			playerAudio.volume = Random.Range (0.25f, 0.35f);
			playerAudio.pitch = Random.Range (0.65f, 0.75f);
			playerAudio.Play ();
		}
	}
}
