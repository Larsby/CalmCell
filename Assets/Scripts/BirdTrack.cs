using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdTrack : MonoBehaviour {
	public string name;
	public int[] tracks;
	public int frequency;
	public int volumeDiff;
	AudioClip [] clips;
	public float randomRangeStart;
	public float randomRangeEnd;
	// Use this for initialization
	private AudioSource audioPlayer;
	void Start () {
		audioPlayer = null;
	}
	public void SetClips(AudioClip [] clips) {
		this.clips = clips;
	}

	public float Play(AudioSource player, float baseVolume) {
		float percent = baseVolume / 100;
		float minVolume =  baseVolume -(percent * volumeDiff);
		float maxVolume = baseVolume + (percent * volumeDiff);
		float volume = Random.Range (minVolume, maxVolume);
		int length = tracks.Length - 1;
		int randomIndex = Random.Range (0, length);
		audioPlayer = player;
		//	Debug.Log (name + "base volum e" +baseVolume+ "volume diff " + volumeDiff + " min vol  " + minVolume + " max volume" + maxVolume+ "frequency" + frequency) ;
		player.clip = clips[tracks[randomIndex]];
		player.volume = volume;
		player.pitch = Random.Range (0.95f, 1.05f);
		player.panStereo = Random.Range (-1.0f, 1.0f);
		player.Play ();
		float randomRange = Random.Range (randomRangeStart, randomRangeEnd);
		return randomRange +frequency + player.clip.length;

	}
	public void Play() {
		if(audioPlayer != null) {
			audioPlayer.Play ();
		}
	}

	public void StopPlay() {
		if (audioPlayer != null) {
			audioPlayer.Stop ();
		}
	}
}
