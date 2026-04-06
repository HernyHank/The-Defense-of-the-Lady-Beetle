using UnityEngine;
using UnityEngine.InputSystem;

public class KB2040Joystick : MonoBehaviour
{
    void Update()
    {
        var gamepad = Gamepad.current;
        if (gamepad != null)
        {
            Vector2 stick = gamepad.leftStick.ReadValue();
            float throttle = gamepad.rightTrigger.ReadValue();
            bool btn1 = gamepad.buttonSouth.isPressed;
            bool btn2 = gamepad.buttonEast.isPressed;

            Debug.Log($"X/Y: {stick}, Throttle: {throttle}, Btn1: {btn1}, Btn2: {btn2}");
        }
    }
}