using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    private static AudioSource audioSource = null;

    [Header("SFX")]
    public AudioClip Audio_bearTrap;
    public AudioClip Audio_HeosuFall;
    public AudioClip Audio_Falling;
    public AudioClip Audio_Cat_Meow;
    public AudioClip Audio_Cat_Die;
    public AudioClip Audio_Cat_Purring;
    public AudioClip Audio_BrickBreak;
    public AudioClip Audio_BlockItem;
    public AudioClip Audio_BlockHit;

    private void Awake()
    {
        Instance = this;
    }

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
