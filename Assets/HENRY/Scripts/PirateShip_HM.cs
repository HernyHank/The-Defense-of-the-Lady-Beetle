using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateShip_HM : MonoBehaviour
{
    [Header("Animator References")]
    public Animator orbitAnimator;   // Animator on the empty orbit object
    public Animator shipAnimator;

/*    void Update()
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

        *//*        Debug.Log(shipAnimator.GetInteger("attackMode"));
        *//*
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


    }*/

    /*    bool[] shipsAfoot = new bool[4];

        public int SpawnPirateShip()
        {
            int cameraNum = Random.Range(0, 4);

            shipsAfoot[cameraNum] = true;
            GameObject specificCam = GameObject.Find("CameraJoint (" + cameraNum + ")");

            return cameraNum;

        }*/

    public void SpawnPirateShip()
    {
        {
            if (orbitAnimator != null)
            {
                orbitAnimator.SetTrigger("StartOrbit");
            } else
            {
                Debug.Log("Couldn't find orbit animator");
            }

            // Start attack animation on the ship
            if (shipAnimator != null)
            { 
                shipAnimator.SetTrigger("StartFly");
            } else
            {
                Debug.Log("couldn't find ship animator");
            }
        }
    }

    public void PirateShipAttack(int mode)
    {
        shipAnimator.SetInteger("attackMode", mode);

        if (orbitAnimator != null)
        {
            orbitAnimator.SetTrigger("StopOrbit");
        } else
        {
            Debug.Log("couldn't find orbit animator");
        }
        // Start attack animation on the ship
        if (shipAnimator != null)
        {
            shipAnimator.SetTrigger("Attack");
        } else
        {
            Debug.Log("couldn't find ship animator");
        }
    }

    public void PrepareNextAttack(float delay)
    {
        StartCoroutine(AttackAfterDelay(delay));
    }

    private IEnumerator AttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Call your existing attack logic
        PirateShipAttack(Random.Range(1, 4));
    }



}