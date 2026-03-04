using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using TMPro;

public class PressBTurnCameraTHenMOve : MonoBehaviour
{
    // Start is called before the first frame update
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean bIsHeld;
    public Animator animator;


    // Update is called once per frame
    void Update()
    {
        if (bIsHeld.GetState(handType) == true)
        {
            animator.SetBool("bIsPressed", true);
        }
    }
}
