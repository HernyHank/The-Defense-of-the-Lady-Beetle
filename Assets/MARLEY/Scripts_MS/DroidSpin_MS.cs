using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidSpin_MS : MonoBehaviour
{

    public Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("DroidHeadSpin_MS"))
            {
                animator.Play("DroidHeadSpin_MS", 0, 0f);
            }
            
        }
    }
}
