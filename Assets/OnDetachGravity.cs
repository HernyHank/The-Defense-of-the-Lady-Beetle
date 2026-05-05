using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


public class OnDetachGravity : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    private Hand attachedHand;
    private Interactable interactable;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        interactable = GetComponent<Interactable>();
    }

    private void OnDetachedFromHand(Hand hand)
    {
        rb.useGravity = true;
        rb.isKinematic = false;
    }
}
