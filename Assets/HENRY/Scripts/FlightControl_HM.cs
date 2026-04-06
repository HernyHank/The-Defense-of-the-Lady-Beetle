using UnityEngine;
using System.IO.Ports;

public class FlightControl_HM : MonoBehaviour
{
    public EventController controller;
    public GameObject Joan;
    public GameObject JoanPivot;

    [Header("Movement Settings")]
    public float moveSpeed = 15f;
    public float tiltAmount = 15f;
    public float tiltSpeed = 5f;
    public float maxDistanceFromOrigin = 20f;

    private float horizontal;
    private float vertical;

    // --- NEW ---
    SerialPort serial;
    int xRaw = 32768;
    int yRaw = 32768;

    float deadzone = 0.1f;

    void Start()
    {
        serial = new SerialPort("COM5", 115200);
        serial.DtrEnable = true;
        serial.RtsEnable = true;
        serial.ReadTimeout = 100;

        try
        {
            serial.Open();
            Debug.Log("Serial connected (Flight)");
            System.Threading.Thread.Sleep(3000);
        }
        catch
        {
            Debug.LogError("Serial failed (Flight)");
        }
    }

    void Update()
    {
        // --- NEW: Read joystick ---
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

        float xInput = (xRaw - 32768f) / 32768f;
        float yInput = (yRaw - 32768f) / 32768f;

        if (Mathf.Abs(xInput) < deadzone) xInput = 0;
        if (Mathf.Abs(yInput) < deadzone) yInput = 0;

        // Reset inputs
        horizontal = 0f;
        vertical = 0f;

        bool shipIsMoving;

        if (controller.currentRoom == "PilotRoom")
        {
            shipIsMoving = false;
            Joan.transform.SetParent(this.transform, true);

            // --- REPLACED INPUT ---
            vertical = -yInput;
            horizontal = xInput;

            if (Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0)
            {
                shipIsMoving = true;
            }

            if (shipIsMoving)
            {
                Joan.GetComponent<CharacterController>().enabled = false;
            }
            else
            {
                Joan.GetComponent<CharacterController>().enabled = true;
            }

            Vector3 move = new Vector3(horizontal, vertical, 0f) * moveSpeed * Time.deltaTime;
            transform.position += move;

            float targetTiltX = -vertical * tiltAmount;
            float targetTiltZ = -horizontal * tiltAmount;

            Quaternion targetRotation = Quaternion.Euler(targetTiltX, 0f, targetTiltZ);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * tiltSpeed);
        }
        else
        {
            Joan.transform.SetParent(JoanPivot.transform, true);
        }
    }

    void OnApplicationQuit()
    {
        if (serial != null && serial.IsOpen)
        {
            serial.Close();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Vector3.zero, maxDistanceFromOrigin);
    }
}