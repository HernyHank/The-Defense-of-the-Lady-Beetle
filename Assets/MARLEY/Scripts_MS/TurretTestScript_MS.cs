using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTestScript_MS : MonoBehaviour
{
    private bool isColliding = false;

    private void OnCollisionStay(Collision collision)
    {
        isColliding = true;
        Debug.Log("Colliding");
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
        if (Input.GetKey(KeyCode.P) && isColliding == true)
        {
            Debug.Log("Pirate ship is destroyed!");
        }
    }
}
