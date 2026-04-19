using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCam : MonoBehaviour
{
    public EventController Controller;
    [SerializeField] GameObject neck;
    [SerializeField] float speed = 2f;

    float initXRotation;
    float initYRotation;
    float xRotation;
    float yRotation;

    public float clampCeiling = 45;
    public float clampFloor = 45;
    public float clampLeftbound = 45;
    public float clampRightbound = 45;

    private void Start()
    {
        Controller = GameObject.Find("EmptyEventController").GetComponent<EventController>();
        Vector3 currentRot = neck.transform.localEulerAngles;

        initXRotation = (currentRot.x > 180) ? currentRot.x - 360 : currentRot.x;
        initYRotation = (currentRot.y > 180) ? currentRot.y - 360 : currentRot.y;

        xRotation = initXRotation;
        yRotation = initYRotation;
    }

    void Update()
    {
        if (Controller.currentRoom == "TurretRoom" && Controller.turretCanShoot)
        {
            // Get joystick input from manager
            float xInput = JoystickManager.Instance.xInput;
            float yInput = JoystickManager.Instance.yInput;

            //DEBUG KEYBOARD INPUT
/*            float xInput = Input.GetAxisRaw("Horizontal");
            float yInput = Input.GetAxisRaw("Vertical");*/

            // Apply rotation
            yRotation += xInput * speed * Time.deltaTime;
            xRotation -= yInput * speed * Time.deltaTime;

            // Clamp rotation
            xRotation = Mathf.Clamp(xRotation, initXRotation - clampCeiling, initXRotation + clampFloor);
            yRotation = Mathf.Clamp(yRotation, initYRotation - clampLeftbound, initYRotation + clampRightbound);

            // Apply to object
            neck.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }
}
