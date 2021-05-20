using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ability_TimeBomb : Ability, IAbility
{
    [Header("능력 별 변수들")]
    [SerializeField] Animator abilityEffectAnim = null;
    [SerializeField] float abilityDefaultTime = 15;
    public float currentTime = 15;

    public bool isAnimationPlaying = false;
    private bool hasTimeBoom = false;
    public bool _hasTimeBoom { get { return hasTimeBoom; } set { hasTimeBoom = value; } }

    //[SerializeField] GameObject player = null;
    [SerializeField] GameObject timeBoom = null;
    [SerializeField] float boomUpForce = 1f;
    [SerializeField] float boomFForce = 1f;
    CircleCollider2D circleCol = null;
    Rigidbody2D rigid = null;


    [SerializeField] float explosionTime = 10f;

    new void Start()
    {
        base.Start();
        circleCol = timeBoom.GetComponent<CircleCollider2D>();

        currentTime = abilityDefaultTime;
    }
    public void OnAbility()
    {
        if (isAnimationPlaying)
        {
            Debug.Log("애니메이션 실행중");
            return;
        }
        if (hasTimeBoom)
        {
            timeBoom.transform.SetParent(null);
            circleCol.enabled = true;
            rigid = timeBoom.AddComponent<Rigidbody2D>();

            rigid.AddForce(Vector2.up * boomUpForce, ForceMode2D.Impulse);
            rigid.AddForce(Vector2.right * boomFForce * PlayerInput.Instance.KeyHorizontalRaw, ForceMode2D.Impulse);

            hasTimeBoom = false;
            PlayerController.Instance._speed = 1f;
            //Time.timeScale = 1f;


            abilityCurrentCoolDown = abilityCooldown;
            abilityCurrentCoolDownTime = Time.time;
        }
        else
        {
            if (abilityCurrentCoolDown > 0) return; // 쿨타임이 아직 안됐다.
            timeBoom.transform.localPosition = new Vector3(0f, 1.1f, 0f);
            timeBoom.transform.localRotation = Quaternion.Euler(Vector3.zero);
            PlayerController.Instance._speed = 0f;
            //Time.timeScale = 0.6f;
            Using();

            abilityEffectAnim.SetTrigger("BlueT");
        }
    }
    void ExTime()
    {
        if (hasTimeBoom)
        {
            timeBoom.GetComponent<TimeBoom>().isAlreadyExplosion = true;
            timeBoom.transform.SetParent(null);
            circleCol.enabled = true;
            //rigid = timeBoom.AddComponent<Rigidbody2D>();

            hasTimeBoom = false;
            PlayerController.Instance._speed = 1f;

            abilityCurrentCoolDown = abilityCooldown;
            abilityCurrentCoolDownTime = Time.time;
        }
    }
    new void Update()
    {
        base.Update();
        //effect.GetComponent<ParticleSystemRenderer>().material.mainTexture = player.sprite.texture; 안됨
    }

    void Using()
    {
        isAnimationPlaying = true;
        Invoke("EndAnimation", 1.5f);
        hasTimeBoom = true;
        timeBoom.SetActive(true);
    }

    void EndAnimation()
    {
        isAnimationPlaying = false;
        Debug.Log("생성 끝남");
        Invoke("ExTime", explosionTime);
    }
}
