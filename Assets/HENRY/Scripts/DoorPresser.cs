using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPresser : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    public GameObject obj;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            animator.SetBool("doorIsOpen", true);
        }
    }
}
