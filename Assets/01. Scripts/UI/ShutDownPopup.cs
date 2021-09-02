using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ShutDownPopup : MonoBehaviour
{
    [SerializeField] CanvasGroup popup;
    private bool popupIsOpen = false;
    Sequence mySequence;
    private void Awake()
    {
        ShutDownPopup[] obj = FindObjectsOfType<ShutDownPopup>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        mySequence = DOTween.Sequence();
        if (SceneManager.GetActiveScene().name == "InGame")
        {
            popup.alpha = 0;
            popup.blocksRaycasts = false;
            popup.interactable = false;
        }
        else
        {
            popup.alpha = 0;
            popup.blocksRaycasts = false;
            popup.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name != "InGame")
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                PopupOpen();
            }
        }
    }
    public void GameShutDown()
    {
        Application.Quit();
    }

    public void PopupOpen()
    {
        popupIsOpen = !popupIsOpen;
        mySequence.Kill();
        if(popupIsOpen)
        {
            mySequence.Append(DOTween.To(() => popup.alpha, x => popup.alpha = x, 0f, 1f));
            popup.blocksRaycasts = false;
            popup.interactable = false;
        }
        else
        {
            mySequence.Append(DOTween.To(() => popup.alpha, x => popup.alpha = x, 1f, 1f));
            popup.blocksRaycasts = true;
            popup.interactable = true;
        }
    }
}
