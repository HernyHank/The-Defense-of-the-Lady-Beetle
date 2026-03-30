using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public string[] eventNames;
    public int currentEvent;
    public int previousEvent;

    public string currentRoom = "bedroom";

    public VRPlayerMovement script;
    //Phase 2
    public pottyScript pottyScript;

    //Phase 3
    public GameObject turretMonitor;

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

        if (Input.GetKeyDown(KeyCode.L))
        {
            isCompleted = true;
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
        if (currentRoom == "PilotRoom")
        {
            return true;
        }

        return false;
    }

    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//

    //HENRY"S ATTEMPT
    /*bool[] shipsAfoot = new bool[4];
    int shipCounter;
    bool hasStarted = false;
    private bool TheAmbush() 
    {
        int shipCounter = 0;
        int singleShipIndex = 0;

        if (shipCounter == 0 && !hasStarted)
        {
            singleShipIndex = Random.Range(0, 4);
            shipsAfoot[singleShipIndex] = true;
            PirateShip_HM pirateShipScript = GameObject.Find("CameraJoint (" + singleShipIndex + ")").GetComponentInChildren<PirateShip_HM>();
            pirateShipScript.SpawnPirateShip();
            hasStarted = true;
        }

        for (int i = 0; i < shipsAfoot.Length; i++)
        {
            if (shipsAfoot[i])
            {
                shipCounter++;
            }
        }

        if (shipCounter == 0)
        {
            return true;
        }

        return false;
    }*/
    /*    private IEnumerator PirateAttackRoutine(PirateShip_HM script, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            script.PirateShipAttack(Random.Range(1, 4));
        }*/
    bool[] shipsAfoot = new bool[4];
    int shipCounter;
    bool ambushStarted = false; // Add this to your class variables

    private bool TheAmbush()
    {
        // If we haven't started the ambush yet, kick it off!
        if (!ambushStarted)
        {
            ambushStarted = true;
            StartCoroutine(AmbushSequence());
        }

        // This event only "completes" when all ships are destroyed (or whatever your win condition is)
        if (ambushStarted && AllShipsDestroyed())
        {
            return true;
        }

        return false;
    }

    // A new helper function to check if the sea is clear
    private bool AllShipsDestroyed()
    {
        foreach (bool ship in shipsAfoot)
        {
            if (ship) return false;
        }
        return true;
    }

    private IEnumerator AmbushSequence()
    {
        // Wave 1: Spawn 2 ships with a delay between them
        SpawnSingleShip();
        yield return new WaitForSeconds(3f);

        SpawnSingleShip();
        yield return new WaitForSeconds(5f);

        SpawnSingleShip();
        yield return new WaitForSeconds(1f);

        // Wave 2... etc.
    }

    private void SpawnSingleShip()
    {
        List<int> emptySlots = new List<int>();
        for (int i = 0; i < shipsAfoot.Length; i++)
        {
            if (!shipsAfoot[i]) emptySlots.Add(i);
        }

        if (emptySlots.Count > 0)
        {
            int chosenSlot = emptySlots[Random.Range(0, emptySlots.Count)];
            shipsAfoot[chosenSlot] = true;

            GameObject cameraObj = GameObject.Find("CameraJoint (" + chosenSlot + ")");
            if(cameraObj = null)
            {
                Debug.Log("Couldn't find camera obj");
            }
            PirateShip_HM script = cameraObj.GetComponentInChildren<PirateShip_HM>();

            Transform child = turretMonitor.transform.Find("Button " + chosenSlot);
            ButtonPress buttonScript = child.GetComponentInChildren<ButtonPress>();

            script.SpawnPirateShip();
            StartCoroutine(PirateAttackRoutine(script, buttonScript, 5.0f, chosenSlot));
        }
    }
    private IEnumerator PirateAttackRoutine(PirateShip_HM script, ButtonPress buttonScript, float waitTime, int chosenSlot)
    {
        yield return new WaitForSeconds(waitTime);
        script.PirateShipAttack(chosenSlot);
        buttonScript.SetMaterial("Warning");
    }

    public void DestroyShip(int index)
    {
        shipsAfoot[index] = false;

        Transform child = turretMonitor.transform.Find("Button " + index);
        ButtonPress buttonScript = child.GetComponentInChildren<ButtonPress>();
        buttonScript.SetMaterial("Normal");
    }

    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//

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

    public void SetCurrentRoom(string room)
    {
        currentRoom = room;
        Debug.Log(currentRoom);
    }
}