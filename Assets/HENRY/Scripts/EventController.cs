using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Valve.VR;

public class EventController : MonoBehaviour
{
    public string[] eventNames;
    public int currentEvent;
    public int previousEvent;

    public string currentRoom = "bedroom";

    [Header("Universals")]
    public VRPlayerMovement script;
    public Animator deathSequenceAnimator;

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
    public bool frontWingBlobbed = false;
    public bool backWingBlobbed = false;
    public bool powerBankBlobbed = false;
    public GameObject turretUIObject;
    public GameObject gooGun;
    public Rigidbody gooGunRB;

    [Header("Calibration")]
    public bool batteryShotIntoSpace = false;
    public bool goodBatteryInPlace = true;

    [Header("AsteroidField")]
    public int bossFightAsteroidCount = 1100;
    public float bossAsteroidTimer = 60f;

    [Header("BossFight")]
    public GameObject bossTorpedo;
    public bool torpedoHit = false;
    public bool torpedoIsLoaded = false;
    public GameObject bossShip;

    [Header("ShipHealth")]
    public int shipHealthMain = 100;
    public GameObject particleParent;


    void Start()
    {
        gooGunRB = gooGun.GetComponent<Rigidbody>();
        if(gooGunRB == null)
        {
            Debug.Log("Couldn't find rigidbody on goo gun");
        }


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
        //ContinuousRoomConditionals();
        //RoomConditionals();
        //HealthConditionals();
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

        if (Input.GetKeyDown(KeyCode.D))
        {
            DamageShip(10);
            Debug.Log("Ship took 10 damage. Remaining health: " + shipHealthMain);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            PlayerDeathSequence();
        }

        if (shipHealthMain <= 0)
        {
            PlayerDeathSequence();
        }
    }

    public void RegenShip(int regenAmount)
    {
        shipHealthMain += regenAmount;
        if(shipHealthMain > 100)
        {
            shipHealthMain = 100;
        }
        //Debug.Log("Ship regenerated " + regenAmount + " health. Current health: " + shipHealthMain);
    }

    public void DamageShip(int damageAmount)
    {
        shipHealthMain -= damageAmount;
        HealthConditionals();
        //Debug.Log("Ship took " + damageAmount + " damage. Remaining health: " + shipHealthMain);
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
        deathSequenceAnimator.SetTrigger("TriggerDeathSequence");
        Debug.Log("PlayerDeathSequence triggered");
        if (!isDeathTimerSet)
        {
            SetTimer(0.5f);
            isDeathTimerSet = true;
            StartCoroutine(DeathCorourtine());
            return;
        }

        /*if (IsTimerFinished())
        {
            // Diagnostics
            bool compositorAvailable = false;
            try
            {
                compositorAvailable = (OpenVR.Compositor != null);
            }
            catch
            {
                compositorAvailable = false;
            }
            Debug.Log($"PlayerDeathSequence: OpenVR.Compositor available = {compositorAvailable}");

            if (compositorAvailable)
            {
                // compositor fade -> guaranteed to affect headset
                SteamVR_Fade.View(Color.black, 1f);
            }
            else
            {
                // Fallback: overlay/renderer fade (requires SteamVR_Fade component on camera)
                Debug.LogWarning("OpenVR compositor not available. Using SteamVR_Fade.Start fallback. Ensure SteamVR_Fade component is present on the SteamVR camera.");
                SteamVR_Fade.Start(Color.black, 1f, true);
            }

            // Show death UI
            script.SetUIText("You have died", true);

            // other death handling...
            currentEvent++;
            currentEvent--;
        }*/
    }

    IEnumerator DeathCorourtine()
    {
               yield return new WaitUntil(IsTimerFinished);
        bool compositorAvailable = false;
        try
        {
            compositorAvailable = (OpenVR.Compositor != null);
        }
        catch
        {
            compositorAvailable = false;
        }
        Debug.Log($"PlayerDeathSequence: OpenVR.Compositor available = {compositorAvailable}");

        if (compositorAvailable)
        {
            // compositor fade -> guaranteed to affect headset
            SteamVR_Fade.View(Color.black, 1f);
        }
        else
        {
            // Fallback: overlay/renderer fade (requires SteamVR_Fade component on camera)
            Debug.LogWarning("OpenVR compositor not available. Using SteamVR_Fade.Start fallback. Ensure SteamVR_Fade component is present on the SteamVR camera.");
            SteamVR_Fade.Start(Color.black, 1f, true);
        }

        // Show death UI
        script.SetUIText("You have died", true);

        // other death handling...
        currentEvent++;
        currentEvent--;
    }

    public bool isGroup1Active = false;
    public bool isGroup2Active = false;
    public bool isGroup3Active = false; 
    public void HealthConditionals()
    {
        if(shipHealthMain <= 75 && !isGroup1Active)
        {
            particleParent.transform.Find("Group1").gameObject.SetActive(true);
            isGroup1Active = true;
        }

        if(shipHealthMain <= 50 && !isGroup2Active)
        {
            particleParent.transform.Find("Group2").gameObject.SetActive(true);
            isGroup2Active = true;
        }

        if(shipHealthMain <= 25 && !isGroup3Active)
        {
            particleParent.transform.Find("Group3").gameObject.SetActive(true);
            isGroup3Active = true;
        }
    }

    public void SetCurrentRoom(string room)
    {
        currentRoom = room;
        Debug.Log(currentRoom);
    }

    public bool pilotMode = false;
    public bool turretMode = false;

    bool modeFlashed = false;
    bool doubleTapFlashed = false;

    public bool outsideAirlockIsOpen = true;
    public void RoomConditionals()
    {
        if(currentRoom == "PilotRoom")
        {
            if (pilotMode == false)
            {
                script.SetUIText("Hold Right Circle to enter Pilot Mode", true);
                if (script.GetBState())
                {
                    script.RealRoomModeBehavior(0);
                    pilotMode = true;
                    script.SetUIText("You shouldn't see this", false);
                    script.DisableController();
                }
            } else if (pilotMode == true)
            {
                if (!doubleTapFlashed)
                {
                    script.SetUIText("Double Tap right circle to exit", true);
                    FlashMode(3f);
                    doubleTapFlashed = true;
                }

                if (script.GetBIsDoublePressedState())
                {
                    pilotMode = false;
                    doubleTapFlashed = false;
                    script.EnableController();
                }
            }
        }

        if(currentRoom == "TurretRoom")
        {
            if (turretMode == false)
            {
                script.SetUIText("Hold Left Circle to enter Turret Mode", true);
                if (script.GetBState())
                {
                    script.DisableController();
                    script.RealRoomModeBehavior(1);
                    turretMode = true;
                    script.SetUIText("You shouldn't see this", false);
                }
            } else if (turretMode == true)
            {
                if (!doubleTapFlashed)
                {
                    script.SetUIText("Double Tap right circle to exit", true);
                    FlashMode(3f);
                    doubleTapFlashed = true;
                }

                if (script.GetBIsDoublePressedState())
                {
                    script.SetJoanTransform(1);
                    script.EnableController();
                    turretMode = false;
                    doubleTapFlashed = false;
                }
            }
        }

        if (currentRoom != "Outside" && currentRoom != "Airlock")
        {
            gooGunRB.useGravity = true;
            gooGunRB.drag = 0.5f;
        }
        else
        {
            gooGunRB.useGravity = false;
            gooGunRB.drag = 300f;
        }

    }

/*    public void ContinuousRoomConditionals()
    {
        if (currentRoom != "Outside" && currentRoom != "Airlock")
        {
            gooGunRB.useGravity = true;
        }
        else
        {
            gooGunRB.useGravity = false;
        }
    }*/

    public void EventControllerSetText(string text, bool shouldShow)
    {
        script.SetUIText(text, shouldShow);
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
    private bool wakeUpTimerSet = false;
    private bool TheWakeUp() {

        if (Input.GetKeyDown(KeyCode.L))
        {
            return true;
        }

        if (!wakeUpTimerSet)
        {
            SetTimer(3f); // Set a timer for 5 seconds
            wakeUpTimerSet = true;
        }

        if (IsTimerFinished())
        {
            return true; // Timer finished, event is complete
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

        DestroyShipOpt();

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

    public void DestroyShipOpt()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            DestroyShipFromController(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DestroyShipFromController(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DestroyShipFromController(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            DestroyShipFromController(3);
        }
    }

    public void DestroyShipFromController(int index)
    {
        if(shipsAfoot[index])
        {

            GameObject cameraObj = GameObject.Find("CameraJoint (" + index + ")");
            PirateShip_HM script = cameraObj.GetComponentInChildren<PirateShip_HM>();
            PirateDestroy_HM pirateGoonScript = script.gameObject.GetComponentInChildren<PirateDestroy_HM>();
            pirateGoonScript.getParentAndSend();

            DestroyShip(index);
        }
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

        /*SpawnSingleShip();*/

        shipCountReachedTwo = true;
        // Wave 2... etc.
    }

    List<int> emptySlots = new List<int>();
    private void SpawnSingleShip()
    {
        // Build a fresh list of available slots each call to avoid accumulation bugs
        List<int> availableSlots = new List<int>();
        for (int i = 0; i < shipsAfoot.Length; i++)
        {
            if (!shipsAfoot[i]) availableSlots.Add(i);
        }

        if (availableSlots.Count == 0)
        {
            Debug.Log("SpawnSingleShip: no available slots to spawn.");
            return;
        }

        int chosenSlot = availableSlots[Random.Range(0, availableSlots.Count)];

        // Mark the slot occupied immediately
        shipsAfoot[chosenSlot] = true;

        //get the camera the ship is parented to
        GameObject cameraObj = GameObject.Find("CameraJoint (" + chosenSlot + ")");
        if (cameraObj == null)
        {
            Debug.LogWarning($"SpawnSingleShip: CameraJoint ({chosenSlot}) not found.");
            // rollback occupancy so future spawns can try this slot again
            shipsAfoot[chosenSlot] = false;
            return;
        }

        //get the orbitScript
        PirateShip_HM script = cameraObj.GetComponentInChildren<PirateShip_HM>();
        // Optionally enable pirate goon object if present
        PirateDestroy_HM pirateGoonScript = script.gameObject.GetComponentInChildren<PirateDestroy_HM>();
/*        if (pirateGoonScript != null)
        {
            pirateGoonScript.gameObject.SetActive(true);
        }*/

        if (script == null)
        {
            Debug.LogWarning($"SpawnSingleShip: PirateShip_HM not found under {cameraObj.name}.");
            shipsAfoot[chosenSlot] = false;
            return;
        }


        // Find turret button and its script (guarded)
        Transform child = turretMonitor.transform.Find("Button " + chosenSlot);
        ButtonPress buttonScript = null;
        if (child != null)
        {
            buttonScript = child.GetComponentInChildren<ButtonPress>();
            if (buttonScript == null)
                Debug.LogWarning($"SpawnSingleShip: ButtonPress not found for Button {chosenSlot}.");
        }
        else
        {
            Debug.LogWarning($"SpawnSingleShip: Button {chosenSlot} not found on turretMonitor.");
        }

        // Spawn and schedule attack
        script.SpawnPirateShip();
        StartCoroutine(PirateAttackRoutine(script, buttonScript, 5.0f, Random.Range(1, 4)));
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
        //Debug.Log("Ship" + index + "destroyed");

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

        DestroyShipOpt();

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
        yield return new WaitForSeconds(0f);
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

        if (frontWingBlobbed && backWingBlobbed && powerBankBlobbed)
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
        if (goodBatteryInPlace)
        {

            TurretMonitorController turretScript = turretMonitor.GetComponent<TurretMonitorController>();
            turretScript.enabled = true;

            turretUIObject.SetActive(false);
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

        if (torpedoIsLoaded)
        {
            if (JoystickManager.Instance.button1)
            {
                TorpedoHoming torpedoScript = bossTorpedo.GetComponent<TorpedoHoming>();
                torpedoScript.enabled = true;
            }
        }


        if (!bossFightStarted)
        {
            TurretMonitorController turretScript = turretMonitor.GetComponent<TurretMonitorController>();
            turretScript.enabled = true;
            turretCanShoot = true;
            turretUIObject.SetActive(false);
            bossShip.SetActive(true);
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
        yield return new WaitUntil(() => torpedoHit);

        bossFightComplete = true;
    }

    private bool Conclusion() 
    {
        script.SetUIText("You have won the game", true);




        return true;
    }

}