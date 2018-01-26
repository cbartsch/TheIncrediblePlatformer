using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AudioClip jumpClip;
    public AudioClip runClip;
    public AudioClip coinClip;

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

    private void playClip(AudioClip clip)
    {
        if (Persistence.SoundsEnabled)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.PlayOneShot(clip, 0.1f);
        }
    }
}
