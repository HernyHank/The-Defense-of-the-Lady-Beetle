using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
public class ClimberObject : MonoBehaviour
{
    public SteamVR_Input_Sources Hand;
    public int TouchedCount;
    public bool grabbing;

    private void OnHandHoverBegin(Hand hand)
    {
        Debug.Log($"[VR] {hand.name} started hovering over {gameObject.name}");
        TouchedCount++;
    }

    private void OnHandHoverEnd(Hand hand)
    {
        Debug.Log($"[VR] {hand.name} stopped hovering over {gameObject.name}");
        TouchedCount--;
    }


}