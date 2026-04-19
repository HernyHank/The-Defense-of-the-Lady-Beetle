using UnityEngine;

public class FlightControl_HM : MonoBehaviour
{
    public EventController controller;
    public GameObject Joan;
    public GameObject JoanPivot;

    [Header("Movement Settings")]
    public float moveSpeed = 15f;
    public float tiltAmount = 15f;
    public float tiltSpeed = 5f;
    public float maxDistanceFromOrigin = 20f;

    private float horizontal;
    private float vertical;

    void Update()
    {
        // Reset inputs
        horizontal = 0f;
        vertical = 0f;

        bool shipIsMoving;

        if (controller.currentRoom == "PilotRoom" && controller.pilotMode == true)
        {
            shipIsMoving = false;
            Joan.transform.SetParent(this.transform, true);

            // ? Get joystick input from manager
            float xInput = JoystickManager.Instance.xInput;
            float yInput = JoystickManager.Instance.yInput;

            // Apply input
            vertical = yInput;
            horizontal = xInput;

            if (Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0)
            {
                shipIsMoving = true;
            }

            // Toggle player controller
            if (shipIsMoving)
                Joan.GetComponent<CharacterController>().enabled = false;
            else
                Joan.GetComponent<CharacterController>().enabled = true;

            // Movement
            Vector3 move = new Vector3(horizontal, vertical, 0f) * moveSpeed * Time.deltaTime;
            transform.position += move;

            /*if (transform.position.magnitude > maxDistanceFromOrigin)
            {
                transform.position = transform.position.normalized * maxDistanceFromOrigin;
            }*/

            // Tilt
            /*float targetTiltX = -vertical * tiltAmount;
            float targetTiltZ = -horizontal * tiltAmount;

            Quaternion targetRotation = Quaternion.Euler(targetTiltX, 0f, targetTiltZ);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * tiltSpeed);*/
        }
        else
        {
            Joan.transform.SetParent(JoanPivot.transform, true);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Vector3.zero, maxDistanceFromOrigin);
    }
}