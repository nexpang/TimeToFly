using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ablility_Teleport : Ability, IAbility
{
    [Header("�ɷ� �� ������")]
    [SerializeField] GameObject clockUI = null;
    [SerializeField] Image clockUIFill = null;
    [SerializeField] Animator abilityEffectAnim = null;
    [SerializeField] RectTransform clockUINeedle = null;
    [SerializeField] float abilityDefaultTime = 15;
    public float currentTime = 15;

    [SerializeField] Transform defaultParent = null;
    [SerializeField] Vector3 defaultjoyStickPos = Vector3.zero;
    [SerializeField] GameObject joystick = null;
    [SerializeField] GameObject joystickBack = null;
    private RectTransform joystickRect = null;
    private RectTransform joystickBackRect = null;
    private bool isUsing = false;
    [SerializeField] GameObject player = null;

    private float fSqr = 0f;

    new void Start()
    {
        base.Start();
        joystickRect = joystick.GetComponent<RectTransform>();
        joystickBackRect = joystickBack.GetComponent<RectTransform>();
    }

    public void OnAbility()
    {
        if (abilityCurrentCoolDown > 0)
        {
            abilityCooldownCircle.DOComplete();
            abilityCooldownCircle.color = Color.red;
            abilityCooldownCircle.DOColor(new Color(0, 0, 0, 0.75f), 0.5f);
            return;
        }// ��Ÿ���� ���� �ȵƴ�.

        //�ɷ� ���
        defaultjoyStickPos = joystick.transform.position;
        isUsing = true;
        joystickBack.SetActive(true);
        joystick.transform.SetParent(joystickBack.transform);
        joystickRect.transform.localPosition = Vector2.zero;
        abilityEffectAnim.SetTrigger("BlueT");
    }
    new void Update()
    {
        base.Update();
        clockUIFill.fillAmount = 1 - (currentTime / abilityDefaultTime);
        clockUINeedle.rotation = Quaternion.Euler(0, 0, -360 * (1 - (currentTime / abilityDefaultTime)));

        Using();
        //effect.GetComponent<ParticleSystemRenderer>().material.mainTexture = player.sprite.texture; �ȵ�

        if (PlayerController.Instance.playerState == PlayerState.DEAD)//���� �������¶��
        {
            StopCoroutine(Clock()); // �ð踦 ������Ų��.
        }
    }

    void Using()
    {

        if (isUsing)
        {
            if (!PlayerInput.Instance.KeyAbilityHold)
            {
                isUsing = false;
                //joystick.transform.position = defaultjoyStickPos;
                joystickRect.localPosition = Vector2.zero;

                joystick.transform.SetParent(defaultParent);
                joystickBack.SetActive(false);

                abilityCurrentCoolDown = abilityCooldown;
                abilityCurrentCoolDownTime = Time.time;

                Debug.Log("�ɷ� �ѽ�����");
                clockUI.SetActive(true);
                DOTween.To(() => clockUI.GetComponent<CanvasGroup>().alpha, value => clockUI.GetComponent<CanvasGroup>().alpha = value, 0.75f, 2f);
                GlitchEffect.Instance.colorIntensity = 0.100f;
                GlitchEffect.Instance.flipIntensity = 0.194f;
                GlitchEffect.Instance.intensity = 0.194f;
                StartCoroutine(Clock());
            }
            else
            {

                /* �̰� ������ ĵ���� ���� �ٲ�ߵ� �ٵ� �׷���ȵ�
                Vector2 touchPos = Input.mousePosition;
                Vector2 vec = new Vector2(touchPos.x - (joystickBackRect.localPosition.x + defaultParent.localPosition.x + Screen.width / 2), touchPos.y - (joystickBackRect.localPosition.y + defaultParent.localPosition.y + Screen.height / 2));
                Debug.Log(defaultParent.localPosition);

                float radius = joystickBackRect.rect.width * 0.5f;

                //vec = Vector2.ClampMagnitude(vec, radius);
                joystickRect.localPosition = vec;

                fSqr = (joystickBackRect.position - joystickBackRect.position).sqrMagnitude / (radius * radius);

                Vector2 vecNormal = vec.normalized;
                */

                /*
                Vector3 touchPos = Input.mousePosition;
                touchPos = new Vector3 (Camera.main.ScreenToWorldPoint(touchPos).x, Camera.main.ScreenToWorldPoint(touchPos).y, 0f);

                touchPos = Vector2.ClampMagnitude(touchPos, (joystickBack.GetComponent<RectTransform>().rect.width * 0.5f));

                joystick.transform.position = touchPos;
                */
            }
        }
    }

    public void ResetPlayer()
    {
        //�ɷ� �ߴ�
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
            if (PlayerController.Instance.playerState == PlayerState.DEAD)//���� �������¶��
            {
                break; // WHILE�� ������
            }
        }

        if (PlayerController.Instance.playerState != PlayerState.DEAD)
        {
            ResetPlayer();
        }
    }
}
