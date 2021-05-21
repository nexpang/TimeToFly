using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear : MonoBehaviour
{
    public static Clear Instance;
    [SerializeField]
    private CanvasGroup clearCG;
    [SerializeField]
    private Canvas clear;
    [SerializeField]
    private Camera main;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("¹«¾ßÈ£!");
    }
    private void Start()
    {
        clearCG = GetComponent<CanvasGroup>();
        clear = GetComponent<Canvas>();
        main = GameObject.Find("Main Camera").GetComponent<Camera>();
        if (!main) 
        {
            clear.worldCamera = main;
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) 
        {
            if (clearCG.alpha == 1) 
            {
                Time.timeScale = 1f;
                clearCG.alpha = 0f;
                clearCG.blocksRaycasts = false;
            }
            else
            {
                Time.timeScale = 0f;
                clearCG.alpha = 1f;
                clearCG.blocksRaycasts = true;
            }
            

        }

    }



}
