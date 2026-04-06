using System.IO.Ports;
using UnityEngine;

public class SerialReader : MonoBehaviour
{
    SerialPort serial;

    void Start()
    {
        serial = new SerialPort("COM5", 115200);
        serial.DtrEnable = true;
        serial.RtsEnable = true;
        serial.NewLine = "\n";
        serial.ReadTimeout = 500;

        serial.Open();
        Debug.Log("Serial port opened");

        System.Threading.Thread.Sleep(3000);
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
                    Debug.Log(data);
                }
            }
            catch { }
        }
    }

    void OnApplicationQuit()
    {
        if (serial != null && serial.IsOpen)
        {
            serial.Close();
        }
    }
}