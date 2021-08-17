using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : MonoBehaviour
{
    public bool isX = true;
    [SerializeField] protected Vector2 startAndEnd;

    public abstract void BossStart();

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
}
