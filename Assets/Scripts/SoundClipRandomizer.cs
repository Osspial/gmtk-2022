using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundClipRandomizer : MonoBehaviour
{
    public AudioClip[] clips;

    // Start is called before the first frame update
    void Start()
    {
        RandomizeSound();
        var source = GetComponent<AudioSource>();
        if (source.playOnAwake) source.Play();
    }

    public void RandomizeSound()
    {
        var i = Random.Range(0,clips.Length);
        var source = GetComponent<AudioSource>();
        source.clip = clips[i];
    }
}
