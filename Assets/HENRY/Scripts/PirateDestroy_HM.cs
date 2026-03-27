using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateDestroy_HM : MonoBehaviour
{
    private bool isColliding = false;
    private int pirateShotLayer;



    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colliding");
    }
    void OnTriggerStay(Collider other)
    {

            isColliding = true;
            if (Input.GetKey(KeyCode.K) && other.CompareTag("Gun"))
            {
                Debug.Log("collPirate ship is destroyed!");
                this.gameObject.SetActive(false);
            }

       
    }
    //not specitying which object the ship is colliding with. Might cause problems later so keep that in mind.^^^vvv
    private void OnTriggerExit(Collider other)
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

        

    }
}
