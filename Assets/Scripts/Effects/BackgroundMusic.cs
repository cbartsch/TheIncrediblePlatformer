using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip defaultMusicClip;

    public float pausePitch = 0.5f;
    public float pitchSmoothTime = 3;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var level = FindObjectOfType<Level>();
	    var clip = (level && level.musicClip) ? level.musicClip : defaultMusicClip;

	    if (!audioSource.clip || (level && clip != audioSource.clip))
	    {
            audioSource.Stop();
	        audioSource.clip = clip;
            audioSource.Play();
	    }

	    var paused = GameManager.Instance.Paused || DragController.DragActive;

	    var targetPitch = paused ? pausePitch : 1;
	    var diff = targetPitch - audioSource.pitch;
	    var timeDiff = Mathf.Abs(1 - pausePitch) * Time.unscaledDeltaTime / pitchSmoothTime * Mathf.Sign(diff);
	    if (timeDiff < Mathf.Abs(diff))
	    {
	        audioSource.pitch += timeDiff;
	    }
	    else
	    {
	        audioSource.pitch = targetPitch;
	    }

	    audioSource.enabled = Persistence.MusicEnabled;
	}
}
