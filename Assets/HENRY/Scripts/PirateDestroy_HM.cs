using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateDestroy_HM : MonoBehaviour
{
    private int pirateShotLayer;



    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Colliding");
    }
    void OnTriggerStay(Collider other)
    {
            if (Input.GetKey(KeyCode.K) && other.CompareTag("Gun"))
            {
                Debug.Log("collPirate ship is destroyed!");
                this.gameObject.SetActive(false);
            }

       
    }
    //not specitying which object the ship is colliding with. Might cause problems later so keep that in mind.^^^vvv
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("No longer colliding");
    }

}
