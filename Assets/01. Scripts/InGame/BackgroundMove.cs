using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMove : MonoBehaviour
{
    private Image BGImg = null;
    private Vector2 offset = Vector2.zero; // == new Vector2(0f, 0f)

    [SerializeField]
    private float speed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        BGImg = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        offset.x += speed * Time.deltaTime * (PlayerInput.KeyHorizontal + 0.3f);
        BGImg.material.SetTextureOffset("_MainTex", offset);
    }
}
