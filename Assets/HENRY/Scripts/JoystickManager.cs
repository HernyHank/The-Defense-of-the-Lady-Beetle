using UnityEngine;
using System.IO.Ports;

public class JoystickManager : MonoBehaviour
{
    public static JoystickManager Instance;

    SerialPort serial;

    public float xInput { get; private set; }
    public float yInput { get; private set; }
    public bool button1 { get; private set; }
    public bool button2 { get; private set; }

    float deadzone = 0.15f;

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        serial = new SerialPort("COM5", 115200);
        serial.DtrEnable = true;
        serial.RtsEnable = true;
        serial.ReadTimeout = 50;

        try
        {
            serial.Open();
            Debug.Log("Joystick connected");
            System.Threading.Thread.Sleep(2000);
        }
        catch
        {
            Debug.LogError("Joystick failed to connect");
        }
    }

    void Update()
    {
        if (serial != null && serial.IsOpen)
        {
            try
            {
                while (serial.BytesToRead > 0)
                {
                    string data = serial.ReadLine();
                    string[] parts = data.Split(',');

                    // Expect: x,y,b1,b2
                    if (parts.Length >= 4)
                    {
                        int xRaw = int.Parse(parts[0]);
                        int yRaw = int.Parse(parts[1]);

                        button1 = parts[2] == "1";
                        button2 = parts[3] == "1";

                        // Convert to -1 to 1
                        xInput = (xRaw - 32768f) / 32768f;
                        yInput = (yRaw - 32768f) / 32768f;

                        // Deadzone
                        if (Mathf.Abs(xInput) < deadzone) xInput = 0;
                        if (Mathf.Abs(yInput) < deadzone) yInput = 0;
                    }
                }
            }
            catch { }
        }
    }

    private void OnApplicationQuit()
    {
        if (serial != null && serial.IsOpen)
            serial.Close();
    }
    void OnGUI()
    {
        GUI.Label(new Rect(10, 60, 300, 20), $"X: {xInput:F2}");
        GUI.Label(new Rect(10, 75, 300, 20), $"Y: {yInput:F2}");
        GUI.Label(new Rect(10, 90, 300, 20), $"Button1: {button1}");
        GUI.Label(new Rect(10, 105, 300, 20), $"Button2: {button2}");
    }
}
