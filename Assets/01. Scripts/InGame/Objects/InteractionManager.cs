using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] InteractionObject[] objects = null;
    [SerializeField] GameObject player = null;
    [SerializeField] GameObject interactionBtn = null;
    [SerializeField] GameObject abilityBtn = null;
    [SerializeField] AudioSource audioSource = null;
    [SerializeField] AudioClip Audio_deniedAbility = null;

    private void Update()
    {
        ButtonChange();
    }

    void ButtonChange()
    {
        foreach (InteractionObject item in objects)
        {
            float distance = Mathf.Abs(item.transform.position.x - player.transform.position.x);
            if(distance < item.interactionDistance)
            {
                abilityBtn.SetActive(false);
                interactionBtn.SetActive(true);
                if (PlayerInput.Instance.KeyInteraction) {
                    if(!item.OnInteraction())
                    {
                        GameManager.Instance.SetAudio(audioSource, Audio_deniedAbility, 0.5f, false);
                        interactionBtn.GetComponent<Image>().DOComplete();
                        interactionBtn.GetComponent<Image>().color = Color.red;
                        interactionBtn.GetComponent<Image>().DOColor(Color.white, 1);
                    }
                   
                }
                return;
            }
        }

        abilityBtn.SetActive(true);
        interactionBtn.SetActive(false);
        interactionBtn.GetComponent<Image>().color = Color.white;
    }
}