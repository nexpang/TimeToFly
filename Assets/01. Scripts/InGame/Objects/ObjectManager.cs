using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance;

    private AudioSource audioSource = null;

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
    public AudioClip Audio_Eagle_Crying;
    public AudioClip Audio_Rock_Breaking;
    public AudioClip Audio_StarSpirit_Rusing;
    public AudioClip Audio_BrickBreak;
    public AudioClip Audio_BlockItem;
    public AudioClip Audio_BlockHit;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(AudioClip audio, float volume, bool immediate)
    {
        Instance.audioSource.volume = volume;

        if(immediate) Instance.audioSource.PlayOneShot(audio);
        else
        {
            Instance.audioSource.clip = audio;
            if(!Instance.audioSource.isPlaying) Instance.audioSource.Play();
        }
    }
}
