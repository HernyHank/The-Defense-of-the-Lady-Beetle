using UnityEngine;
using Valve.VR; // Don't forget this!

public class VRPlayerMovement : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Input_Sources rightHand;// Set to "Left Hand" in Inspector
    public SteamVR_Action_Vector2 moveAction;
    public SteamVR_Action_Boolean bIsHeld;
    public float speed = 2.0f;
    CharacterController controller;
    float verticalVelocity;
    public float gravity = -9.81f;

    public Animator animator;

    public bool playerIsRotating = false;

    private int rotationMode = 0;




    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RotationToggler"))
        {
            Debug.Log("First one a go");
            animator.SetBool("isOutsideShip", true);
            playerIsRotating = true;
        }
        else
        if (other.CompareTag("Rotation2ggler"))
        {
            Debug.Log("2ggler Ibound");
            animator.SetBool("isOnWing", true);
            playerIsRotating = true;
        }
    }
    void Update()
    {

        //debug
        /*if (bIsHeld.GetStateDown(rightHand))
        {
            RotationState();
            playerIsRotating = true;
        }*/

        if (!playerIsRotating)
        {
            MoveNormally(rotationMode);
        }
    }

    void MoveNormally(int rotationMode)
    {

        
        // 1. Get the Vector2 value (X and Y) from the joystick
        Vector2 joystickValue = moveAction.GetAxis(handType);
        Vector3 move = Vector3.zero;

        
/*        else if (rotationMode == 2)
        {

        }*/

        /* float xValue = joystickValue.x;
         float zValue = joystickValue.y;

         *//*Debug.Log($"X: {joystickValue.x:F2} | Y: {joystickValue.y:F2}");*//*
         if (!isInsideShip)
         {

         }*/
        if (joystickValue.magnitude > 0.1f) // Deadzone check
        {

            Vector3 direction = Vector3.zero;
            Vector3 headRotation = Vector3.zero;
            if (rotationMode == 0)
            {
                direction = new Vector3(joystickValue.x, 0, joystickValue.y);
                headRotation = new Vector3(0, GameObject.Find("VRCamera").transform.rotation.eulerAngles.y, 0);
            }
            else if (rotationMode == 1)
            {
                direction = new Vector3(-joystickValue.x, -joystickValue.y, 0);
                headRotation = new Vector3(0, 0, GameObject.Find("VRCamera").transform.rotation.eulerAngles.y);
            }
            else if (rotationMode == 2)
            {
                direction = new Vector3(-joystickValue.x, 0, joystickValue.y);
                headRotation = new Vector3(0, GameObject.Find("VRCamera").transform.rotation.eulerAngles.y, 0);

            }

            direction = Quaternion.Euler(headRotation) * direction;
            // 2. Translate joystick Y to Forward and X to Strafe

            // 3. Move relative to where the player is looking
            // (Uses the Camera's Y rotation so 'Forward' is always where you look)
            move = direction * speed;
        }

        // Gravity
/*        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -2; // keeps player grounded
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }*/

        //move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }

    public void FinishRotation()
    {
        if (!animator.GetBool("isOutsideShip"))
        {
            rotationMode = 0;
        }
        else if (animator.GetBool("isOnWing"))
        {
            rotationMode = 2;
        }
        else
        {
            rotationMode = 1;
        }
        
        
        playerIsRotating = false;
       
        Debug.Log("false toggled");
    }


}