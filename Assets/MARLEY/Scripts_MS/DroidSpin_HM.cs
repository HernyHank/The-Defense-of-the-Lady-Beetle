using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidSpin_HM : MonoBehaviour
{
    [Header("Animator References")]
    public Animator NeckJoint;


    void Start()
    {
        NeckJoint = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (NeckJoint != null)
                NeckJoint.SetTrigger("HeadSpin");
            
        }
    }
}
