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

    // track whether Joan is parented to this ship to avoid repeated SetParent calls
    private bool joanParentedToShip = false;

    void Update()
    {
        // Reset inputs
        horizontal = 0f;
        vertical = 0f;

        bool shipIsMoving;

        if (controller == null) return;

        bool inPilotMode = controller.currentRoom == "PilotRoom" && controller.pilotMode == true;

        // Parent/unparent Joan only when the mode actually changes
        if (inPilotMode && !joanParentedToShip)
        {
            var cc = Joan.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false; // disable before reparenting to avoid physics reconciliation jumps
            Joan.transform.SetParent(this.transform, true);
            joanParentedToShip = true;
        }
        else if (!inPilotMode && joanParentedToShip)
        {
            Joan.transform.SetParent(JoanPivot.transform, true);
            var cc = Joan.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = true;
            joanParentedToShip = false;
        }

        if (inPilotMode)
        {
            shipIsMoving = false;

            // Get joystick input from manager
            float xInput = JoystickManager.Instance.xInput;
            float yInput = JoystickManager.Instance.yInput;

            // Apply input (NO deadzone here by request)
            vertical = yInput;
            horizontal = xInput;

            if (Mathf.Abs(horizontal) > 0f || Mathf.Abs(vertical) > 0f)
            {
                shipIsMoving = true;
            }

            // Toggle player controller (keep it disabled while ship is moving)
            var playerCC = Joan.GetComponent<CharacterController>();
            if (playerCC != null)
            {
                playerCC.enabled = !shipIsMoving;
            }

            // Movement: compute candidate position and clamp the candidate to avoid snapping the current position
            Vector3 move = new Vector3(0f, vertical, -horizontal) * moveSpeed * Time.deltaTime;
            Vector3 newPos = transform.position + move;
            if (newPos.magnitude > maxDistanceFromOrigin)
            {
                newPos = newPos.normalized * maxDistanceFromOrigin;
            }
            transform.position = newPos;

            // Tilt (use localRotation and Slerp for smoother, local-space rotation)
            float targetTiltX = -horizontal * tiltAmount;
            float targetTiltZ = vertical * tiltAmount;

            Quaternion targetRotation = Quaternion.Euler(targetTiltX, 0f, targetTiltZ);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * tiltSpeed);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Vector3.zero, maxDistanceFromOrigin);
    }
}