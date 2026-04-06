using UnityEngine;

public class FlightControl_HM : MonoBehaviour
{
    public EventController controller;
    public GameObject Joan;
    public GameObject JoanPivot;
    [Header("Movement Settings")]
    public float moveSpeed = 10f;   // Speed of the ship
    public float tiltAmount = 15f;  // Maximum tilt angle
    public float tiltSpeed = 5f;    // Tilt easing speed
    public float maxDistanceFromOrigin = 20f;

    private float horizontal;
    private float vertical;

    void Update()
    {
        // ===== READ INPUT =====
        // Reset inputs every frame so the ship stops when you let go
        horizontal = 0f;
        vertical = 0f;

        bool shipIsMoving;
        // ===== READ INPUT =====
        if (controller.currentRoom == "PilotRoom")
        {
            shipIsMoving = false;    
            Joan.transform.SetParent(this.transform, true);
/*            Debug.Log("Player in Pilot room");*/
            // Up/Down movement
            if (Input.GetKey(KeyCode.Keypad8))
            { 
                vertical = 1f;
                shipIsMoving = true;
            }
            if (Input.GetKey(KeyCode.Keypad5)) 
            { 
                vertical = -1f;
                shipIsMoving = true;

            }

            // Left/Right movement
            if (Input.GetKey(KeyCode.Keypad4)) 
            { 
                horizontal = -1f;
                shipIsMoving = true;

            }
            if (Input.GetKey(KeyCode.Keypad6))
            { 
                horizontal = 1f;
                shipIsMoving = true;

            }

            if (shipIsMoving)
            {
                Joan.GetComponent<CharacterController>().enabled = false;
            } else
            {
                Joan.GetComponent<CharacterController>().enabled = true;
            }

            // ===== MOVE SHIP =====
            // The rest of your logic remains the same!
            Vector3 move = new Vector3(horizontal, vertical, 0f) * moveSpeed * Time.deltaTime;
            transform.position += move;

            // ===== SMOOTH TILT =====
            float targetTiltX = -vertical * tiltAmount;
            float targetTiltZ = -horizontal * tiltAmount;

            Quaternion targetRotation = Quaternion.Euler(targetTiltX, 0f, targetTiltZ);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * tiltSpeed);
        } else
        {
            Joan.transform.SetParent(JoanPivot.transform, true);
        }

        //Debug.Log("Joan's Parent is " + Joan.transform.parent.name);

    }


        void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Vector3.zero, maxDistanceFromOrigin);
    }
}