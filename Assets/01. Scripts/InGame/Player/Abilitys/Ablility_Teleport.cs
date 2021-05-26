using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ablility_Teleport : Ability, IAbility
{
    [Header("능력 별 변수들")]
    [SerializeField] GameObject clockUI = null;
    [SerializeField] Image clockUIFill = null;
    [SerializeField] Animator abilityEffectAnim = null;
    [SerializeField] RectTransform clockUINeedle = null;
    [SerializeField] float abilityDefaultTime = 15;
    public float currentTime = 15;

    [SerializeField] Transform defaultParent = null;
    [SerializeField] GameObject joystick = null;
    [SerializeField] GameObject joystickBack = null;
    private RectTransform joystickRect = null;
    private bool isUsing = false;
    [SerializeField] GameObject player = null;
    [SerializeField] Transform playerPos = null;
    [SerializeField] GameObject teleportPosObj = null;
    private Transform teleportPosObjTrans = null;
    [SerializeField] Sprite joystickSpr = null;

    [SerializeField] float timeSlow = 4f;
    [SerializeField] float minusTimeForS = 10f;

    private float fSqr = 0f;
    [SerializeField]
    private float radius = 1f;

    [SerializeField, Range(1f, 10f)]
    private float teleportPower = 10f;
    private Vector2 teleportPos;

    new void Start()
    {
        base.Start();
        joystickRect = joystick.GetComponent<RectTransform>();
        teleportPosObjTrans = teleportPosObj.GetComponent<Transform>();
    }

    public void OnAbility()
    {
        if (abilityCurrentCoolDown > 0)
        {
            abilityCooldownCircle.DOComplete();
            abilityCooldownCircle.color = Color.red;
            abilityCooldownCircle.DOColor(new Color(0, 0, 0, 0.75f), 0.5f);
            return;
        }// 쿨타임이 아직 안됐다.

        //능력 사용
        isUsing = true;
        joystickBack.SetActive(true);
        teleportPosObj.SetActive(true);
        joystick.transform.SetParent(joystickBack.transform);
        joystickRect.transform.localPosition = Vector2.zero;
        abilityBtn.sprite = joystickSpr;
        abilityEffectAnim.SetTrigger("BlueT");
        Time.timeScale = 1f / timeSlow;
        GameManager.Instance.TimerScale = 1f / (minusTimeForS * timeSlow);
        PlayerController.Instance._speed = 0f;
    }
    new void Update()
    {
        base.Update();
        clockUIFill.fillAmount = 1 - (currentTime / abilityDefaultTime);
        clockUINeedle.rotation = Quaternion.Euler(0, 0, -360 * (1 - (currentTime / abilityDefaultTime)));

        Using();
        //effect.GetComponent<ParticleSystemRenderer>().material.mainTexture = player.sprite.texture; 안됨

        if (PlayerController.Instance.playerState == PlayerState.DEAD)//만약 죽은상태라면
        {
            StopCoroutine(Clock()); // 시계를 정지시킨다.
        }
    }

    void Using()
    {

        if (isUsing)
        {
            if (!PlayerInput.Instance.KeyAbilityHold)
            {
                Time.timeScale = 1f;
                GameManager.Instance.TimerScale = 1f;
                PlayerController.Instance._speed = 1f;
                isUsing = false;
                //joystick.transform.position = defaultjoyStickPos;
                joystickRect.localPosition = Vector2.zero;
                abilityBtn.sprite = abilityBtnSpr;

                joystick.transform.SetParent(defaultParent);
                joystickBack.SetActive(false);

                playerPos.position = teleportPos;
                teleportPosObjTrans.position = Vector3.zero;
                teleportPosObj.SetActive(false);

                abilityCurrentCoolDown = abilityCooldown;
                abilityCurrentCoolDownTime = Time.time;

                Debug.Log("능력 뿌슝빠슝");
                clockUI.SetActive(true);
                DOTween.To(() => clockUI.GetComponent<CanvasGroup>().alpha, value => clockUI.GetComponent<CanvasGroup>().alpha = value, 0.75f, 2f);
                GlitchEffect.Instance.colorIntensity = 0.100f;
                GlitchEffect.Instance.flipIntensity = 0.194f;
                GlitchEffect.Instance.intensity = 0.194f;
                StartCoroutine(Clock());
            }
            else
            {
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                touchPos.z = 0f;
                Vector3 firstJPos = joystickBack.transform.position;
                firstJPos.z = 0f;
                Vector3 vec = (touchPos - firstJPos).normalized;
                float dist = Vector2.Distance(touchPos, joystickBack.transform.position);
                Debug.Log(dist);

                if(dist < radius)
                {
                    fSqr = dist;
                }
                else
                {
                    fSqr = radius;
                }
                joystick.transform.position = joystickBack.transform.position + vec * fSqr;

                teleportPos = playerPos.position + vec * (fSqr * teleportPower);

                teleportPosObjTrans.position = teleportPos;
                /* 이거 쓸려면 캔버스 설정 바꿔야됨 근데 그러면안됨
                Vector2 touchPos = Input.mousePosition;
                Vector2 vec = new Vector2(touchPos.x - (joystickBackRect.localPosition.x + defaultParent.localPosition.x + Screen.width / 2), touchPos.y - (joystickBackRect.localPosition.y + defaultParent.localPosition.y + Screen.height / 2));
                Debug.Log(defaultParent.localPosition);

                float radius = joystickBackRect.rect.width * 0.5f;

                //vec = Vector2.ClampMagnitude(vec, radius);
                joystickRect.localPosition = vec;

                fSqr = (joystickBackRect.position - joystickBackRect.position).sqrMagnitude / (radius * radius);

                Vector2 vecNormal = vec.normalized;
                */
            }
        }
    }

    public void ResetPlayer()
    {
        //능력 중단
        player.GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
        Time.timeScale = 1f;

        PlayerController.Instance._speed = 1;

        currentTime = abilityDefaultTime;

        DOTween.To(() => clockUI.GetComponent<CanvasGroup>().alpha, value => clockUI.GetComponent<CanvasGroup>().alpha = value, 0f, 2f).OnComplete(() => clockUI.SetActive(false));

        GlitchEffect.Instance.colorIntensity = 0;
        GlitchEffect.Instance.flipIntensity = 0;
        GlitchEffect.Instance.intensity = 0;

        abilityEffectAnim.SetTrigger("OrangeT");
    }

    IEnumerator Clock()
    {
        yield return new WaitForSeconds(1);

        while (currentTime > 0)
        {
            currentTime--;
            yield return new WaitForSeconds(1);
            if (PlayerController.Instance.playerState == PlayerState.DEAD)//만약 죽은상태라면
            {
                break; // WHILE문 나가기
            }
        }

        if (PlayerController.Instance.playerState != PlayerState.DEAD)
        {
            ResetPlayer();
        }
    }
}
