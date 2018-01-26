using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public AudioClip jumpClip;
    public AudioClip runClip;
    public AudioClip coinClip;
    public AudioClip dieClip;
    public AudioClip reachGoalClip;
    public AudioClip resetLevelClip;
    public AudioClip pauseClip;
    public AudioClip resumeClip;
    public AudioClip enableSoundsClip;

    void Update()
    {
        foreach (var src in GetComponents<AudioSource>())
        {
            if (!src.isPlaying)
            {
                Destroy(src);
            }
        }
    }

    public void PlayJump()
    {
        playClip(jumpClip);
    }

    public void PlayRun()
    {
        playClip(runClip);
    }

    public void PlayCoin()
    {
        playClip(coinClip);
    }

    public void PlayDie()
    {
        playClip(dieClip);
    }

    public void PlayReachGoal()
    {
        playClip(reachGoalClip);
    }

    public void PlayResetLevel()
    {
        playClip(resetLevelClip);
    }

    public void PlayPause()
    {
        playClip(pauseClip);
    }

    public void PlayResume()
    {
        playClip(resumeClip);
    }

    public void PlayEnableSounds()
    {
        playClip(enableSoundsClip);
    }

    private void playClip(AudioClip clip)
    {
        if (Persistence.SoundsEnabled)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.PlayOneShot(clip, 0.2f);
        }
    }
}
