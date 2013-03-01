using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAudio : MonoBehaviour 
{
    public List<AudioClip> clips = new List<AudioClip>();
    AudioSource audioSource;
    int curClip;

	// Use this for initialization
	void Start () 
    {
        audioSource = GetComponentInChildren<AudioSource>();
	}

    void SwipeSwordSound()
    {
        if (!audioSource.isPlaying)
        {
            if (curClip == null || curClip == clips.Count - 1)
                curClip = 0;
            else
                curClip += 1;
            audioSource.PlayOneShot(clips[curClip]);
        }
    }
}
