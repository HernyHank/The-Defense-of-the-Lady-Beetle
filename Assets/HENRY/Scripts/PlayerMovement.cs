using UnityEngine;
using Valve.VR; // Don't forget this!
using TMPro;
using Valve.VR.InteractionSystem;

public class VRPlayerMovement : MonoBehaviour
{
    public SteamVR_Input_Sources leftHand;
    public SteamVR_Input_Sources rightHand;
    public Hand activatedHand;// Set to "Left Hand" in Inspector
    public SteamVR_Action_Vector2 moveAction;
    public SteamVR_Action_Boolean bIsHeld;
    public float speed = 2.0f;
    CharacterController controller;
    float verticalVelocity;
    public float gravity = -1f;

    public Animator animator;
    public TextMeshProUGUI UIText;

    public bool playerIsRotating = false;
    public bool isClimbing = false;
    private int touchCount = 0;

    private int rotationMode = 0;




    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        //makes sure that UI is turned off while rotating}
        if (playerIsRotating == false)
        {         

            //first Collider
            if (other.CompareTag("RotationToggler"))
            {
                /*Debug.Log("First one a go");*/
                UIText.SetText("Hold B to rotate");
                UIText.gameObject.SetActive(true);

                //bHeld
                if (bIsHeld.GetState(rightHand) == true)
                {
                    animator.SetBool("isOutsideShip", !animator.GetBool("isOutsideShip"));
                    playerIsRotating = true;
                    UIText.gameObject.SetActive(false);
                }
            }
            //second Collider
            else
            if (other.CompareTag("Rotation2ggler"))
            {
                /*Debug.Log("2ggler Ibound");*/
                UIText.SetText("Hold B to rotate");
                UIText.gameObject.SetActive(true);

                if (bIsHeld.GetState(rightHand) == true)
                {
                    animator.SetBool("isOnWing", !animator.GetBool("isOnWing"));
                    playerIsRotating = true;
                    UIText.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UIText.gameObject.SetActive(false);
    }


    void Update()
    {

        if (!playerIsRotating && !isClimbing)
        {
            MoveNormally(rotationMode);
        }

        if (isClimbing)
        {
            Climb();
        }

        Debug.Log(touchCount);
    }

    void MoveNormally(int rotationMode)
    {

        // 1. Get the Vector2 value (X and Y) from the joystick
        Vector2 joystickValue = moveAction.GetAxis(leftHand);
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
            float verticalVelocity = 0;
            Vector3 direction = Vector3.zero;
            Vector3 headRotation = Vector3.zero;
            if (rotationMode == 0)
            {
                direction = new Vector3(joystickValue.x, 0, joystickValue.y);
                headRotation = new Vector3(0, GameObject.Find("VRCamera").transform.rotation.eulerAngles.y, 0);
                //Gravity
                /*                        if (controller.isGrounded)
                                        {
                                            if (verticalVelocity < 0)
                                                verticalVelocity = -2; // keeps player grounded
                                        }
                                        else
                                        {
                                            verticalVelocity += gravity * Time.deltaTime;
                                        }*/
                verticalVelocity += gravity * Time.deltaTime;

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
        if (rotationMode == 0)
        {
            if (controller.isGrounded)
            {
                if (verticalVelocity < 0)
                    verticalVelocity = -2; // keeps player grounded
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
            }

            move.y = verticalVelocity;
        }

        controller.Move(move * Time.deltaTime);
    }

    private Vector3 lastHandPosition;
    void Climb()
    {
        Vector3 handDelta = activatedHand.transform.localPosition - lastHandPosition;

        // Move the CharacterController in the opposite direction of the hand pull
        // (If you pull the hand DOWN, the body goes UP)
        controller.Move(transform.rotation * -handDelta);

        // Store the current position for the next frame
        lastHandPosition = activatedHand.transform.localPosition;

    }

    public void FinishRotation()
    {
        if (!animator.GetBool("isOutsideShip"))
        {
            rotationMode = 0;
            Debug.Log("RotationMode set");
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

    public void climbingToggle(Hand hand, bool grabType)
    {
        if (isClimbing == false && grabType == true)
        {
            activatedHand = hand;
            lastHandPosition = hand.transform.localPosition;
           // touchCount++;
            isClimbing = true;
        }
        else if (isClimbing == true && grabType == true)
        {
            activatedHand = hand;
            lastHandPosition = hand.transform.localPosition;
            //touchCount++;
        }
        else if (isClimbing == true && grabType == false)
        {
            activatedHand = null;
            //touchCount--;
            isClimbing = false;
        }

/*        if (touchCount > 0)
        {
            isClimbing = true;
        }
        else
        {
            isClimbing = false;
            Debug.Log("isClimbingToggled " + isClimbing.ToString());
        }*/
    }
}

