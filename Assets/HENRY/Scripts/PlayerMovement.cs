using UnityEngine;
using Valve.VR; // Don't forget this!

public class VRPlayerMovement : MonoBehaviour
{
    public SteamVR_Input_Sources handType; // Set to "Left Hand" in Inspector
    public SteamVR_Action_Vector2 moveAction; // Select "Move" in Inspector
    public float speed = 2.0f;
    CharacterController controller;
    float verticalVelocity;
    public float gravity = -9.81f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {

        
        // 1. Get the Vector2 value (X and Y) from the joystick
        Vector2 joystickValue = moveAction.GetAxis(handType);
        Vector3 move = Vector3.zero;

        Debug.Log($"X: {joystickValue.x:F2} | Y: {joystickValue.y:F2}");

        if (joystickValue.magnitude > 0.1f) // Deadzone check
        {
            // 2. Translate joystick Y to Forward and X to Strafe
            Vector3 direction = new Vector3(joystickValue.x, 0, joystickValue.y);

            // 3. Move relative to where the player is looking
            // (Uses the Camera's Y rotation so 'Forward' is always where you look)
            Vector3 headRotation = new Vector3(0, GameObject.Find("VRCamera").transform.rotation.eulerAngles.y, 0);
            direction = Quaternion.Euler(headRotation) * direction;

            move = direction * speed;
        }

        // Gravity
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -2f; // keeps player grounded
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }
}