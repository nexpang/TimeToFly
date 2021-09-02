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
    public SoundDatas soundData;

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
