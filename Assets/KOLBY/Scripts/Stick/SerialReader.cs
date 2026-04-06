using System.IO.Ports;
using UnityEngine;

public class SerialReader : MonoBehaviour
{
    SerialPort serial = new SerialPort("COM5", 115200);

    void Start()
    {
        serial = new SerialPort("COM5", 115200);
        serial.ReadTimeout = 100;

        serial.Open();
        Debug.Log("Serial port opened");

        System.Threading.Thread.Sleep(3000); // give board time to reboot
    }

    void Update()
    {
        if (serial.IsOpen)
        {
            try
            {
                string data = serial.ReadLine();
                Debug.Log(data);
            }
            catch
            {
            }
        }
    }

    void OnApplicationQuit()
    {
        if (serial.IsOpen)
        {
            serial.Close();
        }
    }
}