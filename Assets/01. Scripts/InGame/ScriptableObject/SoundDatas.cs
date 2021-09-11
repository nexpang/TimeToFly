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
    public AudioClip Audio_Weasel_Growl;
    public AudioClip Audio_Eagle_Crying;
    public AudioClip Audio_Eagle_Die;
    public AudioClip Audio_Rock_Breaking;
    public AudioClip Audio_StarSpirit_Rusing;
    public AudioClip Audio_AppearBlock;
    public AudioClip Audio_BrickBreak;
    public AudioClip Audio_BlockItem;
    public AudioClip Audio_BlockHit;
    public AudioClip Audio_BossAppear;
    public AudioClip Audio_Bat;
    public AudioClip Audio_BossEagleShout;
    public AudioClip Audio_BossEagleAmbient;
    public AudioClip Audio_BossJokJeBiShout;
    public AudioClip Audio_BossJokJeBiSmallSmash;
    public AudioClip Audio_BossBatWing;
    public AudioClip Audio_BossBatBite;
    public AudioClip Audio_BossBatScream;
    public AudioClip Audio_BossBatSwing;
    public AudioClip Audio_GameClear;
}