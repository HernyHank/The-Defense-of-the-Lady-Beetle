using UnityEngine;
using Valve.VR; // Don't forget this!
using TMPro;
using Valve.VR.InteractionSystem;

public class VRPlayerMovement : MonoBehaviour
{
    public EventController eventController;

    public SteamVR_Input_Sources leftHand;
    public SteamVR_Input_Sources rightHand;
    public Hand activatedHand;// Set to "Left Hand" in Inspector
    public SteamVR_Action_Vector2 moveAction;
    public SteamVR_Action_Boolean bIsHeld;
    public SteamVR_Action_Boolean bIsDoublePressed;
    public SteamVR_Action_Boolean sprint;
    public float speed = 2.0f;
    CharacterController controller;
    float verticalVelocity;
    public float gravity = -1f;

    public GameObject floorColliders;

    public Animator animator;
    public TextMeshProUGUI UIText;

    public bool playerIsRotating = false;
    public bool isClimbing = false;
    public bool isPeeing = false;
    public bool shipIsShaking = false;
    public bool isOutsideShip = false;

    private int rotationMode = 0;

    public Transform pilotModeTransform;
    public Transform turretModeTransform;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInParent<Animator>();
    }

/*    private void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;
        if(tag == "PilotRoom" ||
            tag == "TurretRoom" ||
            tag == "Pantry" ||
            tag == "Airlock")
        {
            eventController.SetCurrentRoom(tag);
        }
    }*/

    private void OnTriggerStay(Collider other)
    {
        //makes sure that UI is turned off while rotating}
        if (playerIsRotating == false)
        {         

            //first Collider
            if (other.CompareTag("RotationToggler"))
            {
                Debug.Log("RotationToggler Entered");
                SetUIText("Hold B to rotate", true);

                //bHeld
                if (bIsHeld.GetState(rightHand) == true)
                {
                    animator.SetBool("isOutsideShip", !animator.GetBool("isOutsideShip"));
                    SetUIText("You shouldn't see this", false);

                    isOutsideShip = !isOutsideShip;
                    if (isOutsideShip)
                    {
                        floorColliders.SetActive(false);
                    } else
                    {
                        floorColliders.SetActive(true);
                    }
                    playerIsRotating = true;
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
        if (other.CompareTag("RotationToggler") || other.CompareTag("Rotation2ggler"))
        {
            SetUIText("You shouldn't see this", false);
        }

        // eventController.SetCurrentRoom("InBetweenRooms");
    }


    void Update()
    {

        if (!playerIsRotating && !isClimbing && !isPeeing && !shipIsShaking && !eventController.pilotMode && !eventController.turretMode)
        {
            MoveNormally(rotationMode);
        }

        if (isClimbing)
        {
            Climb();
        }

        //Debug.Log(touchCount);
    }

    void MoveNormally(int rotationMode)
    {

        // 1. Get the Vector2 value (X and Y) from the joystick
        Vector2 joystickValue = moveAction.GetAxis(leftHand);
        Vector3 move = Vector3.zero;

        float xValue = joystickValue.x;
        float zValue = joystickValue.y;

        /*straight forward: -0.5, -0.5, 0.5, -0.5
  straight left:     0.0, -0.0, 0.7, -0.7
  straight back:    -0.5,  0.5,-0.5,  0.5
  straight right:    0.7,  0.7, 0.0,  0.0
  */

       /* Quaternion quat = GameObject.Find("VRCamera").transform.rotation;
        float x = quat.x;
        float y = quat.y;
        float z = quat.z;
        float w = quat.w;
        if (x > 0 && y > 0.5 && -0.5 < z && z < 0.5 && -0.5 < w && w < 0.5)
        {
            Debug.Log("danger zone");
        }*/

        if (joystickValue.magnitude > 0.1f) // Deadzone check
        {
            float verticalVelocity = 0;
            Vector3 direction = Vector3.zero;
            Vector3 headRotation = Vector3.zero;
            if (rotationMode == 0)
            {
                if(bIsHeld.GetState(leftHand) == true)
            {
                speed *= 1.4f;
            }
                direction = new Vector3(joystickValue.x, 0, joystickValue.y);
                headRotation = new Vector3(0, GameObject.Find("VRCamera").transform.rotation.eulerAngles.y, 0);
            //Gravity
            if (controller.isGrounded)
            {
                if (verticalVelocity < 0)
                    verticalVelocity = -2; // keeps player grounded
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
            }

            }

            //shorthand quat exp. 
            /*straight forward: -0.5, -0.5, 0.5, -0.5
              straight left:     0.0, -0.0, 0.7, -0.7
              straight back:    -0.5,  0.5,-0.5,  0.5
              straight right:    0.7,  0.7, 0.0,  0.0
              */
            /*else if (rotationMode == 1)
            {
                direction = new Vector3(0, -joystickValue.x, joystickValue.y);
                headRotation = new Vector3(GameObject.Find("VRCamera").transform.rotation.eulerAngles.x, 0, 0);
                Quaternion quat = GameObject.Find("VRCamera").transform.rotation;
                float x = quat.x;
                float y = quat.y;
                float z = quat.z;
                float w = quat.w;
                if (x > 0 && y > 0.5 && -0.5 < z && z < 0.5 && -0.5 < w && w < 0.5)
                {
                    Debug.Log("danger zone");
                    headRotation.x = -headRotation.x;
                }
                
            }*/
            else if (rotationMode == 1)
            {
                // 1. Setup direction: mapping stick Left/Right to Global Vertical for your wall
                direction = new Vector3(0, -joystickValue.x, joystickValue.y);

                Transform camTransform = GameObject.Find("VRCamera").transform;
                float pitch = camTransform.rotation.eulerAngles.x;

                // 2. DETECT "DANGER ZONE"
                // We compare the Head's Forward to the Body's Forward. 
                // If Dot is negative, the head is looking "Backward" relative to the body.
                float lookAlignment = Vector3.Dot(camTransform.forward, transform.forward);

            if(Input.GetKeyDown(KeyCode.J))
                Debug.Log(camTransform.forward + " " + transform.forward + " " + lookAlignment);

                if (lookAlignment < 0)
                {
                    // We are in the "Back-facing" Euler flip zone.
                    // We invert the pitch to compensate for the Euler flip.
                    headRotation = new Vector3(-pitch, 0, 0);
                    direction = new Vector3(0, joystickValue.x, -joystickValue.y);

                    // Optional Debug to confirm it's working
                    // Debug.Log("Detecting Backwards Look: Compensating Pitch");
                }
                else
                {
                    // Normal front-facing range
                    headRotation = new Vector3(pitch, 0, 0);
                }
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
        // Defensive: if no hand is active bail out and clear climbing state
    if (activatedHand == null)
    {
        isClimbing = false;
        return;
    }

    // Use localPosition only if your hand and player share the same parent space.
    // If positions are in world space, use activatedHand.transform.position instead.
    Vector3 currentHandPos = activatedHand.transform.localPosition;

    // First frame of climbing: lastHandPosition should have been set in climbingToggle,
    // but protect against an uninitialized value here.
    if (lastHandPosition == Vector3.zero)
        lastHandPosition = currentHandPos;

    Vector3 handDelta = currentHandPos - lastHandPosition;

    // Avoid tiny floating noise moves
    if (handDelta.sqrMagnitude > 1e-8f)
        controller.Move(transform.rotation * -handDelta);

    lastHandPosition = currentHandPos;
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

       /* Debug.Log("false toggled")*/;
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

    public void SetUIText(string text, bool textIsOn)
    {
        UIText.SetText(text);
        UIText.gameObject.SetActive(textIsOn);
    }

    public bool GetBState()
    {
        return bIsHeld.state;
    }

    public bool GetBIsDoublePressedState()
    {
        return bIsDoublePressed.state;
    }

    public void SetJoanTransform(Vector3 newPosition, Vector3 newRotation, bool playerCanMove)
    {
        if (controller != null) controller.enabled = false;

        // 2. Set the position and rotation
        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(newRotation);

        // 3. Re-enable the controller
        if (controller != null) controller.enabled = true;

        isPeeing = !playerCanMove;
    }

    public void RealRoomModeBehavior(int mode)
    {

        if(mode == 0)
        {
            transform.position = pilotModeTransform.position;
            transform.rotation = pilotModeTransform.rotation;
        }

        if (mode == 1)
        {
            transform.position = turretModeTransform.position;
            transform.rotation = turretModeTransform.rotation;
        }
    }

    public void DisableController()
    {
        controller.enabled = false;
    }

    public void EnableController()
    {
        controller.enabled = true;
    }

    public void SetJoanTransform(int mode)
    {
        if(mode == 1)
        {
            Vector3 euler = transform.eulerAngles;
            euler.x = 0f;
            transform.eulerAngles = euler;
        }
    }
}




