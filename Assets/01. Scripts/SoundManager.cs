using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private SoundManager[] soundManagers;

    private void Awake()
    {
        soundManagers = FindObjectsOfType<SoundManager>();

        if (soundManagers.Length >= 2)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
