using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointFlag : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if(SceneController.isSavePointChecked)
        {
            animator.Play("SavePoint_Off_to_On");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SceneController.isSavePointChecked) return;

        if (collision.CompareTag("Player") && GameManager.Instance.player.playerState == PlayerState.NORMAL)
        {
            if (GameManager.Instance.player.abilitys[(int)Chickens.BROWN].gameObject.activeSelf)
            {
                if (GameManager.Instance.player.abilitys[(int)Chickens.BROWN].isAbilityEnable)
                {
                    return;
                }
            }

            ObjectManager.PlaySound(GameManager.Instance.player.Audio_playerHeartEat, 1, true);
            ObjectManager.PlaySound(GameManager.Instance.player.Audio_playerTimeEat, 1, true);
            GameManager.Instance.savePointEffect.SetActive(true);
            GameManager.Instance.savePointEffectTxt.SetActive(true);

            SceneController.isSavePointChecked = true;
            SceneController.savePointPos = transform.position;
            animator.Play("SavePoint_Off_to_On");
        }
    }
}
