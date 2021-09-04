using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 씬이 다른데 같은 애니메이션에 사운드 이벤트를 넣을때 씁니다. 
/// </summary>
public class PlaySoundInAnimationEvent : MonoBehaviour
{
    [System.Serializable]
    public class SoundInfo
    {
        public AudioClip clip;
    }

    public AudioSource sfxSource;
    public bool isCutScene;
    public SoundInfo[] sounds;

    public void Awake()
    {
        sfxSource.mute = !SecurityPlayerPrefs.GetBool("inGame.SFX", true);
    }

    public void PlaySound(int index)
    {
        if (isCutScene) return;

        if (index >= 0 && index < sounds.Length)
        {
            sfxSource.PlayOneShot(sounds[index].clip, 1);
        }
        else
        {
            Debug.LogError("Index out of range");
        }
    }
}
