using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    [Header("오브젝트 SFX")]
    private static AudioSource audioSource = null;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(AudioClip audio, float volume, bool immediate)
    {
        audioSource.volume = volume;

        if(immediate)audioSource.PlayOneShot(audio);
        else
        {
            audioSource.clip = audio;
            if(!audioSource.isPlaying) audioSource.Play();
        }
    }
}
