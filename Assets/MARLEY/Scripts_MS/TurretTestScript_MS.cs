using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTestScript_MS : MonoBehaviour
{
    private bool isColliding = false;
    private int pirateShotLayer;


/*private void Start()
    {
        pirateShotLayer = LayerMask.NameToLayer("PirateShot");
    }*/

    void OnCollisionStay(Collision collision)
    {
        //int otherLayer = collision.gameObject.layer;

       // if (otherLayer == pirateShotLayer)
        //{

            isColliding = true;
            Debug.Log("Colliding");
            if (Input.GetKey(KeyCode.K) && isColliding == true)
            {
                Debug.Log("Pirate ship is destroyed!");
            }


        //}
       
    }
    //not specitying which object the ship is colliding with. Might cause problems later so keep that in mind.^^^vvv
    private void OnCollisionExit(Collision collision)
    {
        isColliding = false;
        Debug.Log("No longer colliding");
    }

    public bool IsColliding()
    {
        return isColliding;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Pressed K. isColliding = " + isColliding);
        }

        if (Input.GetKeyDown(KeyCode.K) && isColliding)
        {
            Debug.Log("Pirate ship is destroyed!");
        }

        /*if (Input.GetKey(KeyCode.P) && isColliding)
        {
            Debug.Log("Pirate ship is destroyed!");
        }*/
    }
}
