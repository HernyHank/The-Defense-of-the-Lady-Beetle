using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public string[] eventNames;
    public int currentEvent;
    public int previousEvent;

    public VRPlayerMovement script;

    public pottyScript pottyScript;

    void Start()
    {



        eventNames = new string[]
        {
            "The Wake-up", "Routine", "The Calm", "The Ambush", "Evasive",
            "Retaliation", "The Breach", "The EVA", "The Repair",
            "Calibration", "Asteroid Field", "Boss Fight", "Conclusion"
        };

        currentEvent = 0;
        previousEvent = 0;
        Debug.Log("Current Event: " + eventNames[currentEvent]);

        script.SetUIText(eventNames[currentEvent], true); // Show "The Wake-up" immediately
        StartCoroutine(FlashMode(4f));
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

        if (currentEvent != previousEvent)
        {
            isCompleted = true;
        }
        previousEvent = currentEvent;

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
            currentEvent++; // Move to the next event index (e.g., from 0 to 1)

            // Check if we still have events left
            if (currentEvent < eventNames.Length)
            {
                // 1. Get the name of the NEW event we just switched to
                string newEventName = eventNames[currentEvent];

                Debug.Log("Now Starting: " + newEventName);

                // 2. Update the VR UI
                script.SetUIText(newEventName, true);

                // 3. Start the timer to hide the text
                StartCoroutine(FlashMode(3f));
            }
            else
            {
                Debug.Log("All Events Completed!");
                script.SetUIText("Mission Complete", true);
            }
        }
    }

    public IEnumerator FlashMode(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);

        script.SetUIText("You shouldn't see this", false);
    }

/*    public void EventComplete()
    {
        currentEvent++;
    }*/

    // --- EVENT METHODS ---

    private bool TheWakeUp() {

        if (Input.GetKeyDown(KeyCode.L))
        {
            return true;
        }
        
        return false;    
    }

    private bool Routine() 
    {
        if(pottyScript != null)
        {
            if (pottyScript.eventComplete)
            {
                return true;
            }
        }
        else
        {
            Debug.Log("Cloudn't find da potty scrip");
        }
        return false;
    }

    private bool TheCalm() 
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            return true;
        }

        return false;
    }

    private bool TheAmbush() 
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            return true;
        }

        return false;
    }

    private bool Evasive() 
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            return true;
        }

        return false;
    }

    private bool Retaliation() 
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            return true;
        }

        return false;
    }

    private bool TheBreach() 
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            return true;
        }

        return false;
    }

    private bool TheEVA() 
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            return true;
        }

        return false;
    }

    private bool TheRepair() {
        if (Input.GetKeyDown(KeyCode.L))
        {
            return true;
        }

        return false;
    }

    private bool Calibration() 
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            return true;
        }

        return false;
    }

    private bool AsteroidField() 
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            return true;
        }

        return false;
    }

    private bool BossFight() 
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            return true;
        }

        return false;
    }

    private bool Conclusion() 
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            return true;
        }

        return false;
    }
}