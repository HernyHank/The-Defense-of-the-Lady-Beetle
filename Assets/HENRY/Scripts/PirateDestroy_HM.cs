using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateDestroy_HM : MonoBehaviour
{
    private int pirateShotLayer;
    public EventController controller;
    Animator orbitAnimator;
    Animator bodyAnimator;

    private void Awake()
    {
        //controller = GameObject.Find("EmptyEventController").GetComponent<EventController>();
        orbitAnimator = this.gameObject.transform.parent.GetComponent<Animator>();
        bodyAnimator = this.gameObject.GetComponent<Animator>();
    }

    void OnTriggerStay(Collider other)
    {
            if (Input.GetKey(KeyCode.K) && other.CompareTag("Gun") && controller.turretCanShoot)
            {
                Debug.Log("collPirate ship is destroyed!");
                getParentAndSend();
                orbitAnimator.Rebind();
            orbitAnimator.Update(0f);
            bodyAnimator.Rebind();
            bodyAnimator.Update(0f);
            }

        return;
    }

    public void getParentAndSend()
    {
        Transform currentParent = transform;
        for (int i = 0; i < 3; i++)
        {
            if (currentParent.parent != null)
            {
                currentParent = currentParent.parent;
            }
        }
        string targetName = currentParent.name;
        //Debug.Log(targetName);

        // Split by space and parentheses
        string[] parts = targetName.Split(new char[] { ' ', '(', ')' }, System.StringSplitOptions.RemoveEmptyEntries);
        int index = int.Parse(parts[1]);

        Debug.Log("sending destroy ship of index" + index);

        controller.DestroyShip(index);
    }

}
