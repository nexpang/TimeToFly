using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance;

    private static AudioSource audioSource = null;

    public List<GameObject> createdObjectWhileFutureList = new List<GameObject>();

    [Header("Sprites")]
    public Sprite Spr_bearTrapDefault = null;
    public Sprite Spr_bearTrapTriggered = null;
    public Sprite Spr_questionBlockDefault = null;
    public Sprite Spr_questionBlockTriggered = null;

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
