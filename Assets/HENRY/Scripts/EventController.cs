using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public string[] eventNames;
    public int currentEvent;

    void Start()
    {
        eventNames = new string[]
        {
            "The Wake-up", "Routine", "The Calm", "The Ambush", "Evasive",
            "Retaliation", "The Breach", "The EVA", "The Repair",
            "Calibration", "Asteroid Field", "Boss Fight", "Conclusion"
        };

        currentEvent = 0;
        Debug.Log("Current Event: " + eventNames[currentEvent]);
    }

    private void Update()
    {
        // Only run if we haven't finished all events
        if (currentEvent < eventNames.Length)
        {
            EventCaller();
        }
    }

    private void EventCaller()
    {
        bool isCompleted = false;

        // The switch statement is a cleaner way to handle 13 different IDs
        switch (currentEvent)
        {
            case 0: isCompleted = TheWakeUp(); break;
            case 1: isCompleted = Routine(); break;
            case 2: isCompleted = TheCalm(); break;
            case 3: isCompleted = TheAmbush(); break;
            case 4: isCompleted = Evasive(); break;
            case 5: isCompleted = Retaliation(); break;
            case 6: isCompleted = TheBreach(); break;
            case 7: isCompleted = TheEVA(); break;
            case 8: isCompleted = TheRepair(); break;
            case 9: isCompleted = Calibration(); break;
            case 10: isCompleted = AsteroidField(); break;
            case 11: isCompleted = BossFight(); break;
            case 12: isCompleted = Conclusion(); break;
        }

        if (isCompleted)
        {
            currentEvent++;
            if (currentEvent < eventNames.Length)
            {
                Debug.Log("Now Starting: " + eventNames[currentEvent]);
            }
            else
            {
                Debug.Log("All Events Completed!");
            }
        }
    }

    // --- EVENT METHODS ---

    private bool TheWakeUp() { return false; }

    private bool Routine() { return false; }

    private bool TheCalm() { return false; }

    private bool TheAmbush() { return false; }

    private bool Evasive() { return false; }

    private bool Retaliation() { return false; }

    private bool TheBreach() { return false; }

    private bool TheEVA() { return false; }

    private bool TheRepair() { return false; }

    private bool Calibration() { return false; }

    private bool AsteroidField() { return false; }

    private bool BossFight() { return false; }

    private bool Conclusion() { return false; }
}