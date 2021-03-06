using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public abstract class Boss : MonoBehaviour
{
    public bool isX = true;
    [SerializeField] protected Vector2 startAndEnd;
    public Transform spawnPoint;

    [System.Serializable]
    public struct BackgroundSpeed
    {
        public BackgroundMove backgroundMove;
        public float speed;
    }
    [SerializeField] protected BackgroundSpeed[] backgroundMoves; // 먼 것부터 집어넣어라.

    public abstract void BossStart();
    protected bool cameraStop = true;
    [System.NonSerialized] public Vector2 bossBarRectStartAndEnd = new Vector2(0, 1000);

    protected bool nextPatternCancel = false;

    public virtual void Start()
    {
        if(SceneController.targetMapId == 6 && SecurityPlayerPrefs.GetInt("inGame.bakSukEndingCount", 0) == 0 && SecurityPlayerPrefs.GetInt("inGame.otherEndingCount", 0) == 0)
        {
            GameManager.Instance.bossBarTuto.SetActive(true);
            GameManager.Instance.bossBarTuto.GetComponent<Text>().DOFade(0, 3).SetDelay(10);
        }
    }

    public virtual void Update()
    {
        float barScale = GameManager.Instance.player.transform.position.x / startAndEnd.y;

        GameManager.Instance.bossBarFill.transform.localScale = new Vector2(barScale, 1);

        GameManager.Instance.bossBarChicken.anchoredPosition = new Vector2(
            (bossBarRectStartAndEnd.x + ((bossBarRectStartAndEnd.y - bossBarRectStartAndEnd.x) * barScale)
            - GameManager.Instance.bossBar.GetComponent<RectTransform>().sizeDelta.x / 2)
            , GameManager.Instance.bossBarChicken.anchoredPosition.y);

        GameManager.Instance.bossBarChicken.GetComponent<Image>().sprite = GameManager.Instance.playerAnimObj.GetComponent<SpriteRenderer>().sprite;

        if (barScale >= 1)
        {
            Time.timeScale = 0;
            DOTween.To(() => GameManager.Instance.bgAudioSource.volume, value => GameManager.Instance.bgAudioSource.volume = value, 0, 1.5f).SetUpdate(true);
            GameManager.Instance.FadeInOut(1.5f, 0, 2, () =>
            {
                GameManager.Instance.RemoveRemainChicken(GameManager.Instance.player.abilityNumber);
                SceneController.targetMapId++;
                SecurityPlayerPrefs.SetInt("inGame.saveMapid", SceneController.targetMapId);
                PoolManager.ResetPool();
                SceneManager.LoadScene("CutScenes");
            });
        }
    }

    public void BossHitBoom()
    {
        nextPatternCancel = true;
    }

    #region ANIMATION_EVENTS

    public void Event_CameraForce()
    {
        GameManager.Instance.CameraImpulse(0, 0.5f, 0, 3);
        foreach(BackgroundSpeed background in backgroundMoves)
        {
            background.backgroundMove.transform.DOShakePosition(0.5f,10);
        }
    }

    public void Event_CameraBigForce()
    {
        GameManager.Instance.CameraImpulse(0, 1.25f, 0, 10);
        foreach (BackgroundSpeed background in backgroundMoves)
        {
            background.backgroundMove.transform.DOShakePosition(1.25f,30f);
        }
    }

    public void Event_CameraStop()
    {
        cameraStop = true;
        foreach (BackgroundSpeed background in backgroundMoves)
        {
            background.backgroundMove.SpeedChange(0);
        }
    }

    public void Event_CameraResume()
    {
        cameraStop = false;
        foreach (BackgroundSpeed background in backgroundMoves)
        {
            background.backgroundMove.SpeedChange(background.speed);
        }
    }

    protected void BossAppearSound() // 보스 등장 사운드
    {
        //ObjectManager.PlaySound(ObjectManager.Instance.soundData.Audio_, 1f, true);
    }

    #endregion
}
