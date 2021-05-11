using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeDACK : MonoBehaviour
{
    [SerializeField]
    private GameObject dackObj;

    [SerializeField]
    private List<GameObject> dackObjects = new List<GameObject>();

    private void Start()
    {
        for(int i =0;i<5;i++)
        {
            Instantiate(dackObj);
            dackObjects.Add(dackObj);
        }
    }



}
