using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ���� �ٸ��� ���� �ִϸ��̼ǿ� ���� �̺�Ʈ�� ������ ���ϴ�. 
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
