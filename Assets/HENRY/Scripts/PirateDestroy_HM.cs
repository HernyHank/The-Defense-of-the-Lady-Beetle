using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateDestroy_HM : MonoBehaviour
{
    private int pirateShotLayer;
    public EventController controller;



    void OnTriggerStay(Collider other)
    {
            if (Input.GetKey(KeyCode.K) && other.CompareTag("Gun"))
            {
                Debug.Log("collPirate ship is destroyed!");
            getParentAndSend();
                this.gameObject.SetActive(false);
            }

       
    }
    //not specitying which object the ship is colliding with. Might cause problems later so keep that in mind.^^^vvv

    public void getParentAndSend()
    {
        Transform currentParent = transform;
        for (int i = 0; i < 4; i++)
        {
            if (currentParent.parent != null)
            {
                currentParent = currentParent.parent;
            }
        }
        string targetName = currentParent.name;

        // Split by space and parentheses
        string[] parts = targetName.Split(new char[] { ' ', '(', ')' }, System.StringSplitOptions.RemoveEmptyEntries);
        int index = int.Parse(parts[1]);

        controller.DestroyShip(index);
    }

}
