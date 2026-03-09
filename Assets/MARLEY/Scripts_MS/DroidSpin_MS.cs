using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidSpin_MS : MonoBehaviour
{
    [Header("Animator References")]
    public Animator NeckJoint;


    void Start()
    {
        NeckJoint = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (NeckJoint != null)
                NeckJoint.SetTrigger("HeadSpin");
            
        }
    }
}
