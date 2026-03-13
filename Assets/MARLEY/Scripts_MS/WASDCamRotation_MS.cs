using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDCamRotation_MS : MonoBehaviour
{
    [SerializeField] GameObject neck;
    [SerializeField] float speed = 1f;

    float xRotation = 0f;
    float yRotation = 0f;

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
        xRotation = Mathf.Clamp(xRotation, -45f, 45f);
        yRotation = Mathf.Clamp(yRotation, -45f, 45f);

        neck.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        
    }

}
