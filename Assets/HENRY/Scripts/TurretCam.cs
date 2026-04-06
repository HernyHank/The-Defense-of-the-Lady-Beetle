using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

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

    // --- NEW: Serial + joystick ---
    SerialPort serial;
    int xRaw = 32768;
    int yRaw = 32768;

    float deadzone = 0.15f;

    private void Start()
    {
        Controller = GameObject.Find("EmptyEventController").GetComponent<EventController>();
        Vector3 currentRot = neck.transform.localEulerAngles;

        initXRotation = (currentRot.x > 180) ? currentRot.x - 360 : currentRot.x;
        initYRotation = (currentRot.y > 180) ? currentRot.y - 360 : currentRot.y;

        xRotation = initXRotation;
        yRotation = initYRotation;

        // --- NEW: open serial ---
        serial = new SerialPort("COM5", 115200);
        serial.DtrEnable = true;
        serial.RtsEnable = true;
        serial.ReadTimeout = 100;

        try
        {
            serial.Open();
            Debug.Log("Serial connected");
            System.Threading.Thread.Sleep(3000);
        }
        catch
        {
            Debug.LogError("Serial failed");
        }
    }

    void Update()
    {
        // --- NEW: read joystick ---
        if (serial != null && serial.IsOpen)
        {
            try
            {
                while (serial.BytesToRead > 0)
                {
                    string data = serial.ReadLine();
                    string[] parts = data.Split(',');

                    if (parts.Length >= 2)
                    {
                        xRaw = int.Parse(parts[0]);
                        yRaw = int.Parse(parts[1]);
                    }
                }
            }
            catch { }
        }

        // Convert to -1 to 1
        float xInput = (xRaw - 32768f) / 32768f;
        float yInput = (yRaw - 32768f) / 32768f;

        // Deadzone
        if (Mathf.Abs(xInput) < deadzone) xInput = 0;
        if (Mathf.Abs(yInput) < deadzone) yInput = 0;

        // --- ORIGINAL LOGIC (lightly replaced) ---
        if (Controller.currentRoom == "TurretRoom" && Controller.turretCanShoot == true)
        {
            // Replace keypad with joystick
            yRotation += xInput * speed * Time.deltaTime;
            xRotation -= yInput * speed * Time.deltaTime;

            xRotation = Mathf.Clamp(xRotation, initXRotation - clampCeiling, initXRotation + clampFloor);
            yRotation = Mathf.Clamp(yRotation, initYRotation - clampLeftbound, initYRotation + clampRightbound);

            neck.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }

    private void OnApplicationQuit()
    {
        if (serial != null && serial.IsOpen)
        {
            serial.Close();
        }
    }
}
