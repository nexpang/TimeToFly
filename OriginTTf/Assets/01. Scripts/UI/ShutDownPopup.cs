using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ShutDownPopup : MonoBehaviour
{
    [SerializeField] CanvasGroup popup;
 
    private bool popupIsOpen = false;
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

    void Update()
    {
        if(SceneManager.GetActiveScene().name != "InGame"&&SceneManager.GetActiveScene().name != "Loading" && !popupIsOpen)
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
        if(!popupIsOpen)
        {
            popup.alpha = 0;
            popup.blocksRaycasts = false;
            popup.interactable = false;
        }
        else
        {
            popup.alpha = 1;
            popup.blocksRaycasts = true;
            popup.interactable = true;
        }
    }
}
