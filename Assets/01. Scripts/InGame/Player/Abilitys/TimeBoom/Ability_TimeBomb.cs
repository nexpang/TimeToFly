using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ability_TimeBomb : Ability, IAbility
{
    [Header("管径 紺 痕呪級")]
    [SerializeField] Animator abilityEffectAnim = null;
    [SerializeField] float abilityDefaultTime = 15;
    public float currentTime = 15;

    public bool isAnimationPlaying = false;
    private bool hasTimeBoom = false;
    public bool _hasTimeBoom { get { return hasTimeBoom; } set { hasTimeBoom = value; } }

    //[SerializeField] GameObject player = null;

    [SerializeField] GameObject chargingBar = null; //託臓郊たたたたたたたたたたたたたたたたた
    [SerializeField] Image i_chargingBar = null; //託臓郊たたたたたたたたたたたたたたたたた

    [SerializeField] int useTime = 10;
    [SerializeField] GameObject timeBoom = null;

    float boomUpForce = 2f;
    float boomFForce = 3f;
    [SerializeField] float throwForce = 5f;//1~3
    [SerializeField] float defaultAddForce = 0.1f;
    [SerializeField] float addForce = 0.1f;

    [SerializeField] GameObject player = null;

    CircleCollider2D circleCol = null;
    Rigidbody2D rigid = null;

    [SerializeField] float explosionTime = 10f;

    [Header("神巨神 適験")]
    [SerializeField] AudioClip Audio_futureEnter = null;

    bool isCharging = false;

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
            Debug.Log("蕉艦五戚芝 叔楳掻");
            return;
        }
        if (hasTimeBoom)
        {
            isCharging = true;
            throwForce = 1f;
            addForce = defaultAddForce;
        }
        else
        {
            if (abilityCurrentCoolDown > 0)
            {
                GameManager.Instance.SetAudio(audioSource, Audio_deniedAbility, 0.5f, false);
                abilityCooldownCircle.DOComplete();
                abilityCooldownCircle.color = Color.red;
                abilityCooldownCircle.DOColor(new Color(0, 0, 0, 0.75f), 0.5f);
                return;
            } // 悌展績戚 焼送 照菊陥.

            GameManager.Instance.timer -= useTime;

            GameManager.Instance.SetAudio(audioSource, Audio_futureEnter, 1, false);
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
        if (hasTimeBoom && isCharging)
        {
            if (!PlayerInput.Instance.KeyAbilityHold)
            {
                chargingBar.SetActive(false);
                //Debug.Log(PlayerInput.Instance.KeyAbilityHold);
                isCharging = false;
                ThrowBoom();
            }
            else
            {
                chargingBar.transform.position = player.transform.position - (Vector3.right * 1f);
                chargingBar.SetActive(true);
                Charging();
            }
        }
        //effect.GetComponent<ParticleSystemRenderer>().material.mainTexture = player.sprite.texture; 照喫
    }

    void Using()
    {
        isAnimationPlaying = true;
        Invoke("EndAnimation", 1.5f);
        hasTimeBoom = true;
        timeBoom.SetActive(true);
    }

    void Charging()
    {
        //Debug.Log("託臓掻");
        if (throwForce + (addForce * Time.deltaTime) > 3f || throwForce + (addForce * Time.deltaTime) < 1f)
            addForce *= -1f;
        throwForce += (addForce * Time.deltaTime);
        i_chargingBar.fillAmount = (throwForce-1f) / 2f;
    }

    void ThrowBoom()
    {
        timeBoom.transform.SetParent(null);
        circleCol.enabled = true;
        rigid = timeBoom.AddComponent<Rigidbody2D>();

        rigid.AddForce(Vector2.up * (boomUpForce * throwForce), ForceMode2D.Impulse);
        rigid.AddForce(Vector2.right * (boomFForce * throwForce) * PlayerInput.Instance.KeyHorizontalRaw, ForceMode2D.Impulse);

        hasTimeBoom = false;
        PlayerController.Instance._speed = 1f;
        //Time.timeScale = 1f;


        abilityCurrentCoolDown = abilityCooldown;
        abilityCurrentCoolDownTime = Time.time;
    }

    void EndAnimation()
    {
        isAnimationPlaying = false;
        Debug.Log("持失 魁害");
        Invoke("ExTime", explosionTime);
    }
}
