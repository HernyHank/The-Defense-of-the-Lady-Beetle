using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateShip_DW : MonoBehaviour
{
    [Header("Animator References")]
    public Animator orbitAnimator;   // Animator on the empty orbit object
    public Animator shipAnimator;    // Animator on the spaceship

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("T is pressed");

            // Stop the orbit animation (return to origin)
            if (orbitAnimator != null)
                orbitAnimator.SetTrigger("StopOrbit");

            // Start attack animation on the ship
            if (shipAnimator != null)
                shipAnimator.SetTrigger("Attack");
        }
    }
}