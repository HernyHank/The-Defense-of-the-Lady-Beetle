using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDCamRotation_MS : MonoBehaviour
{
    [SerializeField] GameObject neck;
    [SerializeField] float speed = 20f;

    Quaternion startingRotation;

    private void Awake()
    {
        startingRotation = neck.transform.rotation;
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            neck.transform.Rotate(Vector3.up *speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            neck.transform.Rotate(-Vector3.up * speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.W))
        {
            neck.transform.Rotate(-Vector3.right *speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            neck.transform.Rotate(Vector3.right * speed * Time.deltaTime);
        }
        //ClampVerticalRotation();
        
    }

/*public void ClampVerticalRotation()
    {
        var euler = neck.transform.eulerAngles;
        if (euler.x > 180)
        {
            euler.x -= 360;
        }

        if (euler.x < -180)
        {
            euler.x += 360;
        }

        euler.x = Mathf.Clamp(euler.x, -90, 90);
            transform.neck.eulerAngles = euler;
        
            
    }*/
}
