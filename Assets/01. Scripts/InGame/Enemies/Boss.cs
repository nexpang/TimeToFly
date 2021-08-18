using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : MonoBehaviour
{
    public bool isX = true;
    [SerializeField] protected Vector2 startAndEnd;

    public abstract void BossStart();

    protected bool cameraStop = true;

    public Vector2 bossBarRectStartAndEnd;
    private void Start()
    {
        bossBarRectStartAndEnd = new Vector2(GameManager.Instance.bossBar.transform.position.x,
            GameManager.Instance.bossBar.transform.position.x
            + GameManager.Instance.bossBar.GetComponent<RectTransform>().sizeDelta.x);
    }

    public virtual void Update()
    {
        float barScale = GameManager.Instance.player.transform.position.x / startAndEnd.y;

        GameManager.Instance.bossBarFill.transform.localScale = new Vector2(barScale, 1);

        GameManager.Instance.bossBarChicken.anchoredPosition = new Vector2(
            (bossBarRectStartAndEnd.x + ((bossBarRectStartAndEnd.y - bossBarRectStartAndEnd.x) * barScale)
            - GameManager.Instance.bossBar.GetComponent<RectTransform>().sizeDelta.x / 2)
            , GameManager.Instance.bossBarChicken.anchoredPosition.y);
    }

    #region ANIMATION_EVENTS

    private void Event_CameraForce()
    {
        GameManager.Instance.CameraImpulse(0, 0.5f, 0, 1);
    }

    private void Event_CameraStop()
    {
        cameraStop = true;
    }

    private void Event_CameraResume()
    {
        cameraStop = false;
    }

    #endregion
}
