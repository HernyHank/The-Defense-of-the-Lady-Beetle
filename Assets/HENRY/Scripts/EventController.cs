using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventController : MonoBehaviour
{
    public string[] eventNames;
    public int currentEvent;
    public int previousEvent;

    public string currentRoom = "bedroom";

    [Header("Universals")]
    public VRPlayerMovement script;
    public float beetleHealth = 100;

    [Header("Routine")]
    public pottyScript pottyScript;

    [Header("The Calm")]
    public float calmConversationTime = 1f;

    [Header("Ambush")]
    public GameObject turretMonitor;

    [Header("Evasive")]
    public AsteroidFieldSpawner asteroidSpawnScript;
    public float evasiveTimer = 40f;
    public int evasiveAsteroidCount = 800;

    [Header("Breach")]
    public GameObject CameraParent;
    public float breachTimer = 80f;
    public bool wingHoleBlobbed = false;
    public bool powerBankBlobbed = false;
    public GameObject turretUIObject;

    [Header("Calibration")]
    public bool batteryShotIntoSpace = false;
    public bool goodBatteryInPlace = true;

    [Header("AsteroidField")]
    public int bossFightAsteroidCount = 1100;
    public float bossAsteroidTimer = 20f;


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
        //Debug.Log(currentRoom);
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

        //Debug
        if (Input.GetKeyDown(KeyCode.L))
        {
            isCompleted = true;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            isCompleted = true;
            currentEvent++;
        }

        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            currentRoom = "PilotRoom";
        }

        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            currentRoom = "TurretRoom";
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

        if(beetleHealth <= 0)
        {
            PlayerDeathSequence();
        }
    }

    public IEnumerator FlashMode(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);

        script.SetUIText("You shouldn't see this", false);
    }

    //timer
    private float timerTarget = -1f;
    public void SetTimer(float duration)
    {
        // Current time + how long we want to wait
        timerTarget = Time.time + duration;
    }
    public bool IsTimerFinished()
    {
        // If no timer was set, it's technically "finished" or hasn't started
        if (timerTarget == -1f) return true;

        if (Time.time >= timerTarget)
        {
            timerTarget = -1f; // Reset so it doesn't stay true forever
            return true;
        }
        return false;
    }

    bool isDeathTimerSet = false;
    public void PlayerDeathSequence()
    {
        script.SetUIText("You have died", true);

        //TODO: Disable player Controller, Teleport them to respawn point Depending on level, enable playerController, setUItext = false
        if (!isDeathTimerSet) 
        {
            SetTimer(4f);
            isDeathTimerSet = true;
        } 
        else
        {
            if(IsTimerFinished())
            {
                //teleportPlayer
                script.SetUIText("You shouldn't see this", false);
                currentEvent++;
                currentEvent--;
            }       
        }       
    }

    public void SetCurrentRoom(string room)
    {
        currentRoom = room;
        Debug.Log(currentRoom);
    }

    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//
    // --- EVENT METHODS ---
    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//
    private bool TheWakeUp() {

        if (Input.GetKeyDown(KeyCode.L))
        {
            return true;
        }
        
        return false;    
    }

    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//

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

    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//

    bool calmFieldSpawned = false;
    bool calmTimersBeenCalled = false;
    bool playerWalkedInOnce = false;
    private bool TheCalm() 
    {
        if (!calmFieldSpawned)
        {
            asteroidSpawnScript.SpawnField(200, 20f);
            calmFieldSpawned = true;
        }

        if (currentRoom == "PilotRoom")
        {
            playerWalkedInOnce = true;
        } 
        
        if(!calmTimersBeenCalled && playerWalkedInOnce)
        {
            SetTimer(5f);
            calmTimersBeenCalled = true;
        }

        if (calmTimersBeenCalled)
        {
            bool timerState = IsTimerFinished();
            if(timerState == true)
            {
                return true;
            }
        }


        return false;
    }

    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//

    bool[] shipsAfoot = new bool[4];
    bool ambushStarted = false; // Add this to your class variables
    bool shipCountReachedTwo = false;

    private bool TheAmbush()
    {
        // If we haven't started the ambush yet, kick it off!
        if (!ambushStarted)
        {
            ambushStarted = true;
            StartCoroutine(AmbushSequence());
        }

        // This event only "completes" when all ships are destroyed (or whatever your win condition is)
        if (ambushStarted && AllShipsDestroyed() && shipCountReachedTwo)
        {
            ambushStarted = false;
            shipCountReachedTwo = false;
            emptySlots.Clear();
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
        Debug.Log("AllShipsDestroyed Called");
        return true;
    }

    private int GetRemainingShips()
    {
        int remainingShipsCounter = 0;
        foreach (bool ship in shipsAfoot)
        {
            if (ship)
                remainingShipsCounter++;
        }
        return remainingShipsCounter;
    }

    //TODO turn back on second ship
    private IEnumerator AmbushSequence()
    {
        // Wave 1: Spawn 2 ships with a delay between them
        SpawnSingleShip();
        yield return new WaitForSeconds(3f);

        SpawnSingleShip();

        shipCountReachedTwo = true;
        // Wave 2... etc.
    }

    List<int> emptySlots = new List<int>();
    private void SpawnSingleShip()
    {
        for (int i = 0; i < shipsAfoot.Length; i++)
        {
            if (!shipsAfoot[i]) emptySlots.Add(i);
        }

        if (emptySlots.Count > 0)
        {
            int chosenSlot = emptySlots[Random.Range(0, emptySlots.Count)];
           
            shipsAfoot[chosenSlot] = true;
            emptySlots.Remove(chosenSlot);

            //get the camera the ship is parented to
            GameObject cameraObj = GameObject.Find("CameraJoint (" + chosenSlot + ")");
            Debug.Log(cameraObj + " is active");
            if(cameraObj != null)
            {
               // Debug.Log("found " + cameraObj.name);
            } else
            {
                Debug.Log("Did not find camera join");
            }
            
            //get the orbitScript
            PirateShip_HM script = cameraObj.GetComponentInChildren<PirateShip_HM>();

            //Get the specific pirate ship object
            PirateDestroy_HM pirateGoonScript = script.gameObject.GetComponentInChildren<PirateDestroy_HM>();
/*            if (pirateGoonScript != null)
            {
                Debug.Log("Pirate goon script FOUND on: " + pirateGoonScript.gameObject.name);
                pirateGoonScript.gameObject.SetActive(true);
            } else
            {
                Debug.Log("Pirate goon script NOT found on " + cameraObj.name);
            }*/

            if (script != null) {
                //Debug.Log("found script" + script.name);
            } else
            {
                Debug.Log("found object but not script");
            }

            Transform child = turretMonitor.transform.Find("Button " + chosenSlot);
            if (child != null)
            {
               // Debug.Log("Found child: " + child.name);
            } else
            {
                Debug.Log("Uh oh null child");
            }
            ButtonPress buttonScript = child.GetComponentInChildren<ButtonPress>();

            script.SpawnPirateShip();
            StartCoroutine(PirateAttackRoutine(script, buttonScript, 5.0f, Random.Range(1, 4)));
        }
    }
    private IEnumerator PirateAttackRoutine(PirateShip_HM script, ButtonPress buttonScript, float waitTime, int attackMode)
    {
        yield return new WaitForSeconds(waitTime);
        script.PirateShipAttack(attackMode);
        buttonScript.SetMaterial("Warning");
    }

    public void DestroyShip(int index)
    {
        shipsAfoot[index] = false;
        Debug.Log("Ship" + index + "destroyed");

        Transform child = turretMonitor.transform.Find("Button " + index);
        ButtonPress buttonScript = child.GetComponentInChildren<ButtonPress>();
        buttonScript.SetMaterial("Normal");
    }

    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//

    public bool steroidsSpawned = false;
    bool evasiveTimerSet = false;

    private bool Evasive() 
    {
        if (!steroidsSpawned)
        {
            asteroidSpawnScript.SpawnField(evasiveAsteroidCount, evasiveTimer);
            steroidsSpawned = true;
        }

        if (!evasiveTimerSet)
        {
            SetTimer(evasiveTimer);
            evasiveTimerSet = true;
        } else if(evasiveTimerSet)
        {
            bool timerDone = IsTimerFinished();
            if (timerDone)
            {
                return true;
            }
        }
        
        return false;
    }

    bool shipsHasGoneUpTo4 = false;
    private bool Retaliation() 
    {
        if (!ambushStarted)
        {
            ambushStarted = true;
            StartCoroutine(AmbushSequence2());
        }

        // This event only "completes" when all ships are destroyed (or whatever your win condition is)
        if (ambushStarted && GetRemainingShips() == 1 && shipsHasGoneUpTo4)
        {
            ambushStarted = false;
            turretCanShoot = false;
            return true;
        }

        return false;
    }

    IEnumerator AmbushSequence2()
    {
        SpawnSingleShip();
        SpawnSingleShip();
        yield return new WaitForSeconds(5f);
        SpawnSingleShip();
        SpawnSingleShip();
        shipsHasGoneUpTo4 = true;
    }

    bool breachTimersBeenSet = false;
    public bool turretCanShoot = true;
    private bool TheBreach() 
    {
        /*TurretMonitorController turretScript = turretMonitor.GetComponent<TurretMonitorController>();
        turretScript.enabled = false;*/


        //TextMeshProUGUI turretUI = turretMonitor.GetComponentInChildren<TextMeshProUGUI>();
        turretUIObject.SetActive(true);
        turretCanShoot = false;

        //turretUI.enabled = true;
        //setcameraNotWorkingUI to true;
        if (!breachTimersBeenSet)
        {
            SetTimer(breachTimer);
        }

        if (IsTimerFinished())
        {
            PlayerDeathSequence();
        }

        if (wingHoleBlobbed && powerBankBlobbed)
        {
            turretCanShoot = true;
            return true;
        }

        return false;
    }

    private bool TheEVA() 
    {
        return true;
    }

    private bool TheRepair() 
    {
        return true;
    }

    private bool Calibration() 
    {
        turretCanShoot = true;
        turretUIObject.SetActive(false);
        if (goodBatteryInPlace)
        {

            TurretMonitorController turretScript = turretMonitor.GetComponent<TurretMonitorController>();
            turretScript.enabled = true;
            
            TextMeshProUGUI turretUI = turretMonitor.GetComponentInChildren<TextMeshProUGUI>();
            turretUI.enabled = false;
        }
        if(batteryShotIntoSpace && goodBatteryInPlace && AllShipsDestroyed())
        {
            return true;
        }

        return false;
    }

    private bool AsteroidField() 
    {
        if (!steroidsSpawned)
        {
            asteroidSpawnScript.SpawnField(evasiveAsteroidCount, evasiveTimer);
            steroidsSpawned = true;
        }

        if (!evasiveTimerSet)
        {
            SetTimer(evasiveTimer);
            evasiveTimerSet = true;
        }
        else if (evasiveTimerSet)
        {
            bool timerDone = IsTimerFinished();
            if (timerDone)
            {
                return true;
            }
        }

        return false;
    }

    bool bossFightStarted = false;
    bool bossFightComplete = false;
    bool asteroidsShouldBeSpawned = true;
    private bool BossFight() 
    {
        if (asteroidsShouldBeSpawned)
        {
            StartCoroutine(BossFightAsteroidSpawner());
            asteroidsShouldBeSpawned = false;
        }


        if (!bossFightStarted)
        {
            StartCoroutine(RealDealBossFight());
            bossFightStarted = true;
        }

        if (bossFightComplete)
        {
            return true;
        }

        return false;
    }

    IEnumerator BossFightAsteroidSpawner()
    {
        asteroidSpawnScript.SpawnField(bossFightAsteroidCount, bossAsteroidTimer * 2);
        yield return new WaitForSeconds(bossAsteroidTimer);
        asteroidSpawnScript.SpawnField(bossFightAsteroidCount, bossAsteroidTimer * 2);
        yield return new WaitForSeconds(bossAsteroidTimer);
        asteroidSpawnScript.SpawnField(bossFightAsteroidCount, bossAsteroidTimer * 2);
        yield return new WaitForSeconds(bossAsteroidTimer);
        asteroidSpawnScript.SpawnField(bossFightAsteroidCount, bossAsteroidTimer * 2);
        yield return new WaitForSeconds(bossAsteroidTimer);
        asteroidSpawnScript.SpawnField(bossFightAsteroidCount, bossAsteroidTimer * 2);
        yield return new WaitForSeconds(bossAsteroidTimer);
    }

    IEnumerator RealDealBossFight()
    {
        yield return new WaitForSeconds(5f);
        SpawnSingleShip();
        SpawnSingleShip();
        SpawnSingleShip();
        SpawnSingleShip();
        yield return new WaitUntil(() => GetRemainingShips() == 2);
        SpawnSingleShip();
        SpawnSingleShip();
        yield return new WaitUntil(() => GetRemainingShips() == 2);
        SpawnSingleShip();
        SpawnSingleShip();
        yield return new WaitUntil(() => AllShipsDestroyed());

        bossFightComplete = true;
    }

    private bool Conclusion() 
    {
        script.SetUIText("You have won the game", true);




        return true;
    }

}