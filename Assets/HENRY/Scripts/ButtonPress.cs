using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ButtonPress : MonoBehaviour
{

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug
/*        if(Input.GetKeyDown(KeyCode.P))
        {
            animator.SetBool("buttonIsPressed", true);
        }*/


    }

    private void OnHandHoverBegin(Hand hand)
    {
        Debug.Log($"[VR] {hand.name} started hovering over {gameObject.name}");
        animator.SetBool("buttonIsPressed", true);
    }

    // Triggered by Hand.cs Line 195
    private void OnHandHoverEnd(Hand hand)
    {
        Debug.Log($"[VR] {hand.name} stopped hovering.");
    }

    // Triggered by Hand.cs Line 419
    private void OnAttachedToHand(Hand hand)
    {
        Debug.Log($"[VR] Attached to {hand.name}!");
    }

    // Triggered by Hand.cs Line 523
    private void OnDetachedFromHand(Hand hand)
    {
        Debug.Log("[VR] Detached from hand.");
    }

}
