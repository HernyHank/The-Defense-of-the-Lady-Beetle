using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidSpin_HM : MonoBehaviour
{
    [Header("Animator References")]
    public Animator NeckJoint;
    public EventController eventController;


    void Start()
    {
        NeckJoint = GetComponent<Animator>();
            eventController = FindObjectOfType<EventController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (NeckJoint != null)
                NeckJoint.SetTrigger("HeadSpin");
            
        }

        if (eventController.dialogueActive)
        {
            if (NeckJoint != null)
                NeckJoint.SetTrigger("HeadSpin");
        }
    }
}
