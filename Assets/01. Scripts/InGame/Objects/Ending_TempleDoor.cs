using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Ending_TempleDoor : MonoBehaviour
{
    public CanvasGroup mobileControllerGroup;
    private bool isTrigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(!isTrigger)
            {
                isTrigger = true;
                DOTween.To(() => GameManager.Instance.bgAudioSource.volume, value => GameManager.Instance.bgAudioSource.volume = value, 0, 2);
                GameManager.Instance.player.SetStun(10);
                mobileControllerGroup.interactable = false;
                mobileControllerGroup.blocksRaycasts = false;
                GameManager.Instance.FadeInOut(3, 2, 2, () =>
                {
                    SecurityPlayerPrefs.SetBool("inGame.ending", true);
                    PoolManager.ResetPool();
                    SceneManager.LoadScene("TextCutScenes");
                });
            }
        }
    }
}
