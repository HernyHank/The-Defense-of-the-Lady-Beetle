using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidSpin_MS : MonoBehaviour
{
    public GameObject NeckJoint;
    public GameObject Head;
    public Animator DroidHeadSpin_MS;
    void Start()
    {
        NeckJoint = GameObject.Find("NeckJoint");
        Head = GameObject.Find("Head");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            
            
        }
    }
}
