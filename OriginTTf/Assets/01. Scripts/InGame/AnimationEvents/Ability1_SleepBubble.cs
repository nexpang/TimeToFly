using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability1_SleepBubble : MonoBehaviour
{
    private Animator animator = null;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void BubbleAwake() // 해당 함수는 Sleeping_SpeechBubble_Awake에 들어있음.
    {
        animator.applyRootMotion = false;
        transform.localPosition = new Vector3(1.51f, 1.19f, 0);
    }

    void BubbleIdle() // 해당 함수는 Sleeping_SpeechBubble_Idle에 들어있음.
    {
        animator.applyRootMotion = true;
    }
}
