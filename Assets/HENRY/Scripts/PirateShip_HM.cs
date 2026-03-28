using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateShip_HM : MonoBehaviour
{
    [Header("Animator References")]
    public Animator orbitAnimator;   // Animator on the empty orbit object
    public Animator shipAnimator;
    private int attackMode = 1;// Animator on the spaceship

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            attackMode = 1;
            shipAnimator.SetInteger("attackMode", attackMode);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            attackMode = 2;
            shipAnimator.SetInteger("attackMode", attackMode);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            attackMode = 3;
            shipAnimator.SetInteger("attackMode", attackMode);
        }

        /*        Debug.Log(shipAnimator.GetInteger("attackMode"));
        */
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P is pressed");

            // Stop the orbit animation (return to origin)
            if (orbitAnimator != null)
                orbitAnimator.SetTrigger("StartOrbit");

            // Start attack animation on the ship
            if (shipAnimator != null)
                shipAnimator.SetTrigger("StartFly");
        }


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