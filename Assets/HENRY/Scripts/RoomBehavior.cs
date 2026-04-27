using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public EventController controller;
    void Awake()
    {
        //Debug.Log("Rooms is awake");
        controller = GetComponentInParent<EventController>();
        /*if(controller == null)
        {
            Debug.Log("Bruh");
        } else
        {
            Debug.Log("Controller found");
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            controller.SetCurrentRoom(this.gameObject.name);
            Debug.Log("Player enterd" + this.gameObject.name);
            controller.RoomConditionals();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (controller.currentRoom != "Outside")
            {
                controller.SetCurrentRoom("InBetweenRooms");
            }            
            Debug.Log("Player exited");
            controller.EventControllerSetText("you shouldn't see this", false);
            controller.RoomConditionals();
        }
    }
}
