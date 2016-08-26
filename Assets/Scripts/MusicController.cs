using UnityEngine;
using System.Collections;

public class MusicController : Singleton<MusicController>
{

	public float currentMusicLevel = 1f;
	private AudioSource source;
	private int currentGameTrackCount;
	//private AudioClip [] music;
	[SerializeField]
	AudioClip [] music;

	// Use this for initialization
	void Awake ()
	{
		Singleton<GameController>.instance.startGame += StartGame;
		Singleton<GameController>.instance.loadGame += LoadGame;
		Singleton<GameController>.instance.resetGame += StopGame;

		source = gameObject.GetComponent<AudioSource> ();
		//music = (AudioClip []) Resources.LoadAll ("Audio/Sound/");
	}

	void OnDestroy ()
	{
		Singleton<GameController>.instance.startGame -= StartGame;
		Singleton<GameController>.instance.loadGame -= LoadGame;
		Singleton<GameController>.instance.resetGame -= StopGame;
	}


	// Update is called once per frame
	private void StartGame ()
	{
		if (Singleton<TutorialController>.instance.tutorialComlete)
			StartCoroutine ("CleanUpSoundEnum");
	}
	private void StopGame ()
	{
		source.volume = currentMusicLevel /2f;
		Singleton<GameCenterController>.instance.CheckAchievement (Achievements.MusicLover, currentGameTrackCount);
	}

	private void LoadGame ()
	{
		StopCoroutine ("CleanUpSoundEnum");
		source.Stop ();
		source.volume = currentMusicLevel;
		currentGameTrackCount = 0;
	}

	private AudioClip ChangeTheRecord ()
	{
		AudioClip randomMusic;
		do {
			randomMusic = music [Random.Range (0, music.Length - 1)];
			//randomMusic = Resources.Load ("Audio/Music/music" + Random.Range (1, 3).ToString () + ".mp3") as AudioClip;
		} while (source.clip == randomMusic);
		return randomMusic;
	}


	IEnumerator CleanUpSoundEnum ()
	{
		do {
			if (!source.isPlaying && (source.time <= 0f)) {
				source.clip = ChangeTheRecord ();
				currentGameTrackCount++;
				source.Play ();
			} else {
				yield return new WaitForSeconds (0.2f);
			}
		} while (true);
	}


	public void SetSoundsLevel (SoundLevel iLevel)
	{
		switch (iLevel) {
		case SoundLevel.Mute:
			source.mute = true;
			source.volume = 0f;
			break;
		case SoundLevel.Low:
			source.mute = false;
			source.volume = 0.3f;
			break;
		case SoundLevel.Medium:
			source.mute = false;
			source.volume = 0.6f;
			break;
		case SoundLevel.High:
			source.mute = false;
			source.volume = 1f;
			break;
		}
		currentMusicLevel = source.volume;
	}

}

public enum SoundLevel
{
	Mute ,
	Low,
	Medium,
	High
}