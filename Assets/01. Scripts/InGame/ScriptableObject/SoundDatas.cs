using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SoundDatas", order = 1)]
public class SoundDatas : ScriptableObject
{
    [Header("BGM")]
    public AudioClip BGM_Boss;

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
}