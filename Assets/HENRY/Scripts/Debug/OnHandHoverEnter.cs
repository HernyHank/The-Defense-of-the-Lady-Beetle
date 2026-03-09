using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class OnHandHoverEnter : MonoBehaviour
{
    public SteamVR_Input_Sources Hand;
    public int TouchedCount;
    public bool grabbing;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            TouchedCount++;
            Debug.Log("You've found a climbale");
        }
        Debug.Log("You've found an object");
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            TouchedCount--;
        }
    }

}