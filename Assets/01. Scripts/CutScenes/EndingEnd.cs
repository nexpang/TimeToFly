using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingEnd : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.LoadScene("ChickenSelectScene");
    }
}
