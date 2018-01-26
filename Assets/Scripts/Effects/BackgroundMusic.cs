using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip defaultMusicClip;

    public float pausePitch = 0.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var level = FindObjectOfType<Level>();
	    var clip = (level && level.musicClip) ? level.musicClip : defaultMusicClip;

	    if (clip != audioSource.clip)
	    {
            audioSource.Stop();
	        audioSource.clip = clip;
            audioSource.Play();
	    }

	    var paused = GameManager.Instance.Paused || DragController.DragActive;

	    var pitch = paused ? pausePitch : 1;
	    audioSource.pitch = Mathf.Lerp(audioSource.pitch, pitch, Time.unscaledDeltaTime * 5);

	    audioSource.enabled = Persistence.SoundsEnabled;
	}
}
