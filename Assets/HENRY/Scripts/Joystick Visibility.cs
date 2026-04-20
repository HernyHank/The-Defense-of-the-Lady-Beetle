using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickVisibility : MonoBehaviour
{
    [SerializeField] private Transform bigVertical;      // assign in Inspector
    [SerializeField] private Transform smallHorizontal;  // assign in Inspector

    void Update()
    {
        if (bigVertical == null || smallHorizontal == null) return;

        float xInput = JoystickManager.Instance.xInput;
        float yInput = JoystickManager.Instance.yInput;

        Vector3 eulerVersionOfJoystick = ConvertToEuler(xInput, yInput);

        bigVertical.localRotation = Quaternion.Euler(0f, 180f, eulerVersionOfJoystick.y);
        smallHorizontal.localRotation = Quaternion.Euler(-eulerVersionOfJoystick.x, 0f, 0f);
    }

    Vector3 ConvertToEuler(float x, float y)
    {
        float maxAngle = 30f;
        return new Vector3(x * maxAngle, y * maxAngle, 0f);
    }
}
