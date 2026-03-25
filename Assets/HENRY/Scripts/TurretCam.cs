using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCam : MonoBehaviour
{
    [SerializeField] GameObject neck;
    [SerializeField] float speed = 1f;

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
        Vector3 currentRot = neck.transform.localEulerAngles;

        initXRotation = (currentRot.x > 180) ? currentRot.x - 360 : currentRot.x;
        initYRotation = (currentRot.y > 180) ? currentRot.y - 360 : currentRot.y;

        xRotation = initXRotation;
        yRotation = initYRotation;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            yRotation += speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            yRotation -= speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.W))
        {
            xRotation -= speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            xRotation += speed * Time.deltaTime;
        }
        xRotation = Mathf.Clamp(xRotation, initXRotation - clampCeiling, initXRotation + clampFloor);
        yRotation = Mathf.Clamp(yRotation, initYRotation - clampLeftbound, initYRotation + clampRightbound);

        neck.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        
    }

}
