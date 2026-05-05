using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using TMPro;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Valve.VR;
using static Valve.VR.InteractionSystem.Sample.CustomSkeletonHelper;


public class EventController : MonoBehaviour
{
    public string[] eventNames;
    public int previousEvent;

    public string currentRoom = "bedroom";

    [Header("Universals")]
    public VRPlayerMovement script;
    public Animator deathSequenceAnimator;
    private bool pilotPurgatoryRunning = false;
    private bool turretPurgatoryRunning = false;
    private bool deathSequenceReset = false;

    [Header("Audio")]
    public AudioManager audioManager;
    public bool dialogueActive = false; 

    [Header("Damage Groups")]
    public GameObject group1;
    public GameObject group2;
    public GameObject group3;

    [Header("Pirate Ships")]
    public float pirateConversationDelay = 42f;

    [Header("Lights")]
    // Drag your "Light Parent" object here in the inspector
    public GameObject lightParent;
    private RedLightFlash_MS[] allLights;

    public int currentEvent;

    [Header("Routine")]
    public pottyScript pottyScript;

    [Header("The Calm")]
    public float calmConversationTime = 1f;

    [Header("Ambush")]
    public GameObject turretMonitor;

    [Header("Evasive")]
    public AsteroidFieldSpawner asteroidSpawnScript;
    public float evasiveTimer = 60f;
    public int evasiveAsteroidCount = 800;

    [Header("Breach")]
    public GameObject CameraParent;
    public float breachTimer = 80f;
    public bool frontWingBlobbed = false;
    public bool backWingBlobbed = false;
    public bool powerBankBlobbed = false;
    public bool historyLessonGiven = false;
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
    public float shipHealthMain = 100f;
    public GameObject particleParent;


    void Start()
    {
        /*audioManager.OnDialogueFinished += HandleDialogueEnd;*/
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
        if (lightParent != null)
        {
            allLights = lightParent.GetComponentsInChildren<RedLightFlash_MS>();
        }
        script.SetUIText(eventNames[currentEvent], true); // Show "The Wake-up" immediately
        StartCoroutine(FlashMode(4f));
    }

    int regenCounter = 0;
    public int regenIncrement = 10;
    public int regenFrequency = 3000; // Regenerate every 3000 frames (adjust as needed)
    private void Update()
    {
        // Only run if we haven't finished all events
        if (currentEvent < eventNames.Length)
        {
            EventCaller();
        }

        if(regenCounter % regenFrequency == 0)
        {
            RegenShip(regenIncrement);
        } 
        /*        if (script.GetBState())
                {
                    Debug.Log("B was held");
                }*/
        //ContinuousRoomConditionals();
        //RoomConditionals();
        //HealthConditionals();
        //Debug.Log(currentRoom);
    }

/*    public void ResetLevel()
    {
        // Gets the index of the currently active scene and reloads it
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }*/

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
            audioManager.StopDialogue();
            conversationHasBeenStarted = false;
            dialogueActive = false;
            audioClipList.Clear();
            //audioManager.StopMusic();
            ambushStarted = false;
            shipCountReachedTwo = false;
            ResetDialogue();
            DestroyAllShips();
            StopAllCoroutines();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            isCompleted = true;
            currentEvent++;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            isCompleted = true;
            currentEvent--;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            audioManager.StopEverything();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            DestroyAllShips();
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            bool p1isActive = particleParent .transform.Find("Group1").gameObject.activeSelf;
            bool p2isActive = particleParent .transform.
                Find("Group2").gameObject.activeSelf;
            bool p3isActive = particleParent .transform.Find("Group3").gameObject.activeSelf;

            particleParent.transform.Find("Group3").gameObject.SetActive(!p3isActive);
            particleParent.transform.Find("Group2").gameObject.SetActive(!p2isActive);
            particleParent.transform.Find("Group1").gameObject.SetActive(!p1isActive);
        }

        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            currentRoom = "PilotRoom";
        }

        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            currentRoom = "TurretRoom";
        }

        //INPUT key down keypad 8 clears asteroid field, on AsteroidField Script

        if (deathSequenceReset)
        {
            isCompleted = true;
            currentEvent--;
            deathSequenceReset = false;
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

/*        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("Death sequence triggered by key press");
            PlayerDeathSequence();
        }*/

    }

    public void RegenShip(float regenAmount)
    {
        shipHealthMain += regenAmount;
        if(shipHealthMain > 100)
        {
            shipHealthMain = 100;
        }
        HealthConditionals();
        //Debug.Log("Ship regenerated " + regenAmount + " health. Current health: " + shipHealthMain);
    }

    public void DamageShip(float damageAmount)
    {
        shipHealthMain -= damageAmount;
        HealthConditionals();
        Debug.Log("Ship took " + damageAmount + " damage. Remaining health: " + shipHealthMain);
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

    //bool isDeathTimerSet = false;

    public bool explosionsAreInactive = true;
    public bool playerDeathSequenceActive = false;
    public void PlayerDeathSequence()
    {
        if (!playerDeathSequenceActive)
        {
            playerDeathSequenceActive = true;
            deathSequenceAnimator.SetTrigger("TriggerDeathSequence");
            explosionsAreInactive = false;
            Debug.Log("PlayerDeathSequence triggered");
            //if (!isDeathTimerSet)
            //{ 
            //SetTimer(0.5f);
            //isDeathTimerSet = true;
            StartCoroutine(DeathCorourtine());
            return;
        }
        
    }
    IEnumerator DeathCorourtine()
    {
               //yield return new WaitUntil(IsTimerFinished);
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

        yield return new WaitForSeconds(3f);

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

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 4; i++)
        {
            DestroyShipFromController(i);
        }
        asteroidSpawnScript.ClearField();
        // Show death UI

        // other death handling...
        deathSequenceAnimator.SetTrigger("TriggerDeath_Inactive");

        //reset EVERYTHING
        pilotPurgatoryRunning = false;
        turretPurgatoryRunning = false;
        deathSequenceReset = false;
        frontWingBlobbed = false;
        backWingBlobbed = false;
        powerBankBlobbed = false;
        batteryShotIntoSpace = false;
        goodBatteryInPlace = true;
        torpedoHit = false;
        torpedoIsLoaded = false;
        pilotMode = false;
        turretMode = false;

        modeFlashed = false;
        doubleTapFlashed = false;

        outsideAirlockIsOpen = true;
        calmFieldSpawned = false;
        calmTimersBeenCalled = false;
        playerWalkedInOnce = false;

        ambushStarted = false; // Ad
        shipCountReachedTwo = false;

        steroidsSpawned = false;
        evasiveTimerSet = false;

        shipsHasGoneUpTo4 = false;

        breachTimersBeenSet = false;
        turretCanShoot = true;

        bossFightStarted = false;
        bossFightComplete = false;
        asteroidsShouldBeSpawned = true;
        StopAllCoroutines();
        RegenShip(1000);

        script.Respawn();

        yield return new WaitForSeconds(1f);

        yield return new WaitUntil(() => explosionsAreInactive);
        if (compositorAvailable)
        {
            // compositor fade -> guaranteed to affect headset
            SteamVR_Fade.View(Color.clear, 1f);
        }
        else
        {
            // Fallback: overlay/renderer fade (requires SteamVR_Fade component on camera)
            Debug.LogWarning("OpenVR compositor not available. Using SteamVR_Fade.Start fallback. Ensure SteamVR_Fade component is present on the SteamVR camera.");
            SteamVR_Fade.Start(Color.clear, 1f, true);
        }
        deathSequenceReset = true;
        playerDeathSequenceActive = false;
    }

    public void DestroyAllShips()
    {
        for (int i = 0; i < 4; i++)
        {
            DestroyShipFromController(i);
        }
    }

    public void HealthConditionals()
    {
        // 1. Healthy State
        if (shipHealthMain > 75)
        {
            SetVisuals(false, false, false);
            UpdateLights(isRed: false);
            return;
        }

        // 2. Minor Damage
        if (shipHealthMain >= 50)
        {
            SetVisuals(true, false, false);
            UpdateLights(isRed: false);
            return;
        }

        // 3. Major Damage
        if (shipHealthMain > 25)
        {
            SetVisuals(true, true, false);
            UpdateLights(isRed: false);
            return;
        }

        // 4. Critical State
        if (shipHealthMain > 0)
        {
            SetVisuals(true, true, true);
            UpdateLights(isRed: true);
            return;
        }

        if (shipHealthMain <= 0)
        {
            Debug.Log("Death sequence triggered by key press");
            PlayerDeathSequence();
        }
    }

    // Helper method to toggle particles/groups
    private void SetVisuals(bool g1, bool g2, bool g3)
    {
        if (group1) group1.SetActive(g1);
        if (group2) group2.SetActive(g2);
        if (group3) group3.SetActive(g3);
    }

    // Helper method to update all lights at once
    private void UpdateLights(bool isRed)
    {
        foreach (RedLightFlash_MS light in allLights)
        {
            if (isRed) light.SetRedFlashing();
            else light.SetWhiteSolid();
        }
    }



/*            if (shipHealthMain <= 75 && !isGroup1Active)
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
        }*/

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
        // Pilot room entry: start a one-shot wait that either activates pilot mode on button press
        if (currentRoom == "PilotRoom")
        {
            if (!pilotMode && !pilotPurgatoryRunning)
            {
                script.SetUIText("Hold Right Circle to enter Pilot Mode", true);
                StartCoroutine(FlashMode(3f));
                StartCoroutine(PilotTurretPurgatory("PilotRoom"));
            }
            else if (pilotMode)
            {
                if (!doubleTapFlashed)
                {
                    script.SetUIText("Double Tap right circle to exit", true);
                    StartCoroutine(FlashMode(3f));
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

        // Turret room entry: same pattern but for turret
        if (currentRoom == "TurretRoom")
        {
            if (!turretMode && !turretPurgatoryRunning)
            {
                script.SetUIText("Hold Right Circle to enter Turret Mode", true);
                StartCoroutine(FlashMode(3f));
                StartCoroutine(PilotTurretPurgatory("TurretRoom"));
            }
            else if (turretMode)
            {
                if (!doubleTapFlashed)
                {
                    script.SetUIText("Double Tap right circle to exit", true);
                    StartCoroutine(FlashMode(3f));
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
    }

    IEnumerator PilotTurretPurgatory(string room)
    {
        if (room == "PilotRoom")
            pilotPurgatoryRunning = true;
        else if (room == "TurretRoom")
            turretPurgatoryRunning = true;

        // Wait until B is pressed or player leaves the room
        yield return new WaitUntil(() => script.GetBState() || currentRoom != room);

        // If player is still in the room and pressed B, perform the appropriate enter behavior
        if (currentRoom == room && script.GetBState())
        {
            if (room == "PilotRoom")
            {
                script.RealRoomModeBehavior(0);
                pilotMode = true;
                script.SetUIText("You shouldn't see this", false);
                script.DisableController();
                StartCoroutine(PilotModeConditionals());
            }
            else if (room == "TurretRoom")
            {
                script.DisableController();
                script.RealRoomModeBehavior(1);
                turretMode = true;
                script.SetUIText("You shouldn't see this", false);
                StartCoroutine(TurretModeConditionals());
            }
        }

        if (room == "PilotRoom")
            pilotPurgatoryRunning = false;
        else if (room == "TurretRoom")
            turretPurgatoryRunning = false;
    }

    IEnumerator PilotModeConditionals()
    {
        if (!doubleTapFlashed)
        {
            script.SetUIText("Double Tap right circle to exit", true);
            StartCoroutine(FlashMode(3f));
            doubleTapFlashed = true;
        }

        yield return new WaitUntil(() => script.GetBIsDoublePressedState());

            pilotMode = false;
            doubleTapFlashed = false;
            script.EnableController();
    }

    IEnumerator TurretModeConditionals()
    {
        if (!doubleTapFlashed)
        {
            script.SetUIText("Double Tap right circle to exit", true);
            StartCoroutine(FlashMode(3f));
            doubleTapFlashed = true;
        }

        yield return new WaitUntil(() => script.GetBIsDoublePressedState());

            script.SetJoanTransform(1);
            script.EnableController();
            turretMode = false;
            doubleTapFlashed = false;

    }

    IEnumerator PilotTurretPurgatory()
    {
        // kept for compatibility if other code calls the parameterless overload
        yield break;
    }

    IEnumerator PilotTurretPurgatory(bool dummy) { yield break; }

    public void EventControllerSetText(string text, bool shouldShow)
    {
        script.SetUIText(text, shouldShow);
    }

    public bool initialConversationStarted = false;
    public int audioClipIndex = 0;
    public bool allClipsPlayed = false;
    public void PlayAudioClipList()
    {
        // Fixed: use logical check, guard bounds and nulls
        if (!dialogueActive && !allClipsPlayed)
        {
            if (audioClipIndex < 0) audioClipIndex = 0;

            if (audioClipIndex >= audioClipList.Count)
            {
                Debug.Log("All audio clips in the list have been played.");
                allClipsPlayed = true;
                dialogueActive = false;
                return;
            }

            Debug.Log("Attempting to play audio clip at index " + audioClipIndex);
            AudioClip clipToPlay = audioClipList[audioClipIndex];

            if (clipToPlay == null)
            {
                Debug.LogWarning($"PlayAudioClipList: clip at index {audioClipIndex} is null. Skipping.");
                audioClipIndex++;
                return;
            }

            audioManager.PlayDialogueSequence(clipToPlay, 0.8f);

            dialogueActive = true;
            if (dialogueActive)
            {
                audioClipIndex++;
            }


        }

        // check again after potential play
/*        if (audioClipIndex >= audioClipList.Count)
        {
            Debug.Log("All audio clips in the list have been played.");
            allClipsPlayed = true;
            dialogueActive = false;
            return;
        }*/
    }

    float clipListLength = 0;
    public float GetTotalClipListLength()
    {
        for (int i = 0; i < audioClipList.Count; i++)
        {
            if (audioClipList[i] == null)
            {
                Debug.Log("Failed to load audio clip at index " + i + " for Routine event.");
            }
            else
            {
                clipListLength += audioClipList[i].length;
            }
        }

        return clipListLength;
    }

    void checkAudioClipList()
    {
        for (int i = 0; i < audioClipList.Count; i++)
        {
            if (audioClipList[i] == null)
            {
                Debug.Log("Failed to load audio clip at index " + i + " for Routine event.");
            }
            else
            {
                Debug.Log("Successfully loaded audio clip: " + audioClipList[i].name);
            }
        }
    }

    private void ResetDialogue()
    {
        audioManager.StopDialogue();
        dialogueActive = false;
        conversationHasBeenStarted = false; // Reset for potential future use
        audioClipsFetched = false;
        audioClipList.Clear();
        allClipsPlayed = false;
        audioClipIndex = 0;
    }

    /*    void HandleDialogueEnd()
        {
            Debug.Log("The AudioManager told me the clip is done!");
        }*/

    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//
    // --- EVENT METHODS ---
    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//
    private bool conversationHasBeenStarted = false;
    private bool musicHasBeenInitiated = false;
    private bool TheWakeUp() {

        script.DisableController();
        if (Input.GetKeyDown(KeyCode.L))
        {
            return true;
        }

        if (!musicHasBeenInitiated)
        {
            AudioClip musicClip = audioManager.FetchClip("Music/LadyBeetleOpener");
            if (musicClip != null)
            {
                audioManager.PlayMusicClip(musicClip);
                musicHasBeenInitiated = true;
            }
            else
            {
                Debug.Log("Failed to load music clip for The Wake-Up event.");
            }
        }

        if (!conversationHasBeenStarted)
        {
            AudioClip audioClip = audioManager.FetchClip("Dialogue/1. Wakeup/WakeUp");
            if (audioClip != null)
            {
                audioManager.PlayDialogueSequence(audioClip, 0.8f);
            } else
            {
                Debug.Log("Failed to load audio clip for The Wake-Up event.");
            }
                conversationHasBeenStarted = true;
        }

        if (dialogueActive == false)
        {
            conversationHasBeenStarted = false;
            musicHasBeenInitiated = false;
            audioClipIndex = 0;// Reset for potential future use
            return true;
        }   

        return false;    
    }

    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//
    public List<AudioClip> audioClipList = new List<AudioClip>();
    bool audioClipsFetched = false;
    private bool Routine() 
    {
        script.EnableController();  
        if (audioClipsFetched == false)
        {
            audioClipList.Add(audioManager.FetchClip("Dialogue/2. Routine/Routine__Morning Whizzes_"));
            audioClipList.Add(audioManager.FetchClip("Dialogue/2. Routine/Routine__Broken door, missing Arms and Legs_"));
            for (int i = 0; i < audioClipList.Count; i++)
            {
                if (audioClipList[i] == null)
                {
                    Debug.Log("Failed to load audio clip at index " + i + " for Routine event.");
                }
                else
                {
                    Debug.Log("Successfully loaded audio clip: " + audioClipList[i].name);
                }
            }
            audioClipsFetched = true;
        }
        else
        {
            if(allClipsPlayed == false)
                PlayAudioClipList();
        }

        if (pottyScript != null)
        {
            if (pottyScript.eventComplete)
            {
                /*ResetDialogue();*/
                allClipsPlayed = false;
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
            asteroidSpawnScript.SpawnField(120, 30f);
            calmFieldSpawned = true;
        }

        if (currentRoom == "PilotRoom")
        {
            playerWalkedInOnce = true;
        } 
        
        if(playerWalkedInOnce)
        {
           /* SetTimer(5f);
            calmTimersBeenCalled = true;*/
            if (audioClipsFetched == false)
            {
                audioClipList.Add(audioManager.FetchClip("Dialogue/3. The Calm/The Calm__You_d think someone who trained in the Polywoggle Arts_"));
                for (int i = 0; i < audioClipList.Count; i++)
                {
                    if (audioClipList[i] == null)
                    {
                        Debug.Log("Failed to load audio clip at index " + i + " for Routine event.");
                    }
                    else
                    {
                        Debug.Log("Successfully loaded audio clip: " + audioClipList[i].name);
                    }
                }
                audioClipsFetched = true;
            }

            if (allClipsPlayed == false)
                   PlayAudioClipList();
            
            if(allClipsPlayed)
            {
                ResetDialogue();
                return true;
            }
        }

        /*if (calmTimersBeenCalled)
        {
            bool timerState = IsTimerFinished();
            if(timerState == true)
            {
                return true;
            }
        }*/


        return false;
    }

    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//
    //---------------------------------------------//

    bool[] shipsAfoot = new bool[4];
    bool ambushStarted = false; // Add this to your class variables
    bool shipCountReachedTwo = false;
    float dialogueTime;
    bool pirateMusicInitiated = false;

    private bool TheAmbush()
    {
        // If we haven't started the ambush yet, kick it off!
        if (!ambushStarted)
        {
            ambushStarted = true;
            StartCoroutine(AmbushSequence());
        }

        if (!pirateMusicInitiated)
        {
            AudioClip musicClip = audioManager.FetchClip("Music/Ambushes");
            if (musicClip != null)
            {
                audioManager.PlayMusicClip(musicClip);
                pirateMusicInitiated = true;
            }
            else
            {
                Debug.Log("Failed to load music clip for The Wake-Up event.");
            }
        }

        DestroyShipOpt();

        if (audioClipsFetched == false)
        {
            audioClipList.Add(audioManager.FetchClip("Dialogue/4. Ambush/Ambush__And we_re about to BLOW your(shoe)s off!!_"));
            audioClipList.Add(audioManager.FetchClip("Dialogue/4. Ambush/Ambush__Get Down that Ladder and Obliterate them_"));
            audioClipList.Add(audioManager.FetchClip("Dialogue/4. Ambush/Ambush__Get ready to be BLASTED JOOOAN_"));
            dialogueTime = audioManager.FetchClip("Dialogue/4. Ambush/Ambush__And we_re about to BLOW your(shoe)s off!!_").length;


            for (int i = 0; i < audioClipList.Count; i++)
            {
                if (audioClipList[i] == null)
                {
                    Debug.Log("Failed to load audio clip at index " + i + " for Routine event.");
                }
                else
                {
                    Debug.Log("Successfully loaded audio clip: " + audioClipList[i].name);
                }
            }
            audioClipsFetched = true;
        }

        if (allClipsPlayed == false)
            PlayAudioClipList();

        // This event only "completes" when all ships are destroyed (or whatever your win condition is)
        if (ambushStarted && AllShipsDestroyed() && shipCountReachedTwo && !playerDeathSequenceActive)
        {
            ambushStarted = false;
            shipCountReachedTwo = false;
            emptySlots.Clear();
           //audioManager.StopMusic();
            ResetDialogue();
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
        if (Input.GetKeyDown(KeyCode.Alpha4))
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
            pirateGoonScript.gameObject.SetActive(false);

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
        StartCoroutine(PirateAttackRoutine(script, buttonScript, pirateConversationDelay, Random.Range(1, 4)));
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
    bool evasiveMusicInitiated = false;
    bool evasiveCoroutineStarted = false;

    private bool Evasive() 
    {
        if(evasiveCoroutineStarted == false)
        {
            StartCoroutine(EvasiveCoroutine());
            evasiveCoroutineStarted = true;
        }
        /*        if (!evasiveMusicInitiated)
                {
                    AudioClip musicClip = audioManager.FetchClip("Music/Pilot Mode");
                    if (musicClip != null)
                    {
                        audioManager.PlayMusicClip(musicClip);
                        evasiveMusicInitiated = true;
                    }
                    else
                    {
                        Debug.Log("Failed to load music clip for The Wake-Up event.");
                    }
                }*/

        if (audioClipsFetched == false)
        {
            audioClipList.Add(audioManager.FetchClip("Dialogue/4. Ambush/Ambush__That_s the last of them_"));
            audioClipList.Add(audioManager.FetchClip("Dialogue/4. Ambush/Ambush__You got them just in the nick of time_"));
            //dialogueTime = audioManager.FetchClip("Dialogue/4. Ambush/Ambush__And we_re about to BLOW your(shoe)s off!!_").length;


            for (int i = 0; i < audioClipList.Count; i++)
            {
                if (audioClipList[i] == null)
                {
                    Debug.Log("Failed to load audio clip at index " + i + " for Routine event.");
                }
                else
                {
                    Debug.Log("Successfully loaded audio clip: " + audioClipList[i].name);
                }
            }
            audioClipsFetched = true;
        }

        if (allClipsPlayed == false)
            PlayAudioClipList();

        /*        if (!steroidsSpawned)
                {
                    asteroidSpawnScript.SpawnField(evasiveAsteroidCount, evasiveTimer);
                    steroidsSpawned = true;
                }*/

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
                ResetDialogue();
                return true;
            }
        }

        return false;
    }

    IEnumerator EvasiveCoroutine()
    {
        yield return new WaitForSeconds(7f);
        //load music
        AudioClip musicClip = audioManager.FetchClip("Music/Pilot Mode");
        if (musicClip != null)
        {
            audioManager.PlayMusicClip(musicClip);
            evasiveMusicInitiated = true;
        }
        else
        {
            Debug.Log("Failed to load music clip for The Wake-Up event.");
        }

        //Delay asteroid spawn
        yield return new WaitForSeconds(10f);
        if (!steroidsSpawned)
        {
            asteroidSpawnScript.SpawnField(evasiveAsteroidCount, evasiveTimer);
            steroidsSpawned = true;
        }
    }

    bool shipsHasGoneUpTo4 = false;

    AudioClip thatsWhatSheSaidClip;
    private bool Retaliation() 
    {
        pirateConversationDelay = 1f;

        DestroyShipOpt();

        if (audioClipsFetched == false)
        {
            audioClipList.Add(audioManager.FetchClip("Dialogue/5. Evasive/Evasive__We_re in the clear and I gotta go poop_"));
            audioClipList.Add(audioManager.FetchClip("Dialogue/5. Evasive/Evasive__I don_t have any bowels or an anus. You sold those for parts_"));
            audioClipList.Add(audioManager.FetchClip("Dialogue/6. Retaliation/Retaliation__Kackle pirates are back and we brought friends_"));
/*            audioClipList.Add(audioManager.FetchClip("Dialogue/6. Retaliation/Retaliation_KackleIntimidation05"));
            audioClipList.Add(audioManager.FetchClip("Dialogue/6. Retaliation/Retaliation_KackleIntimidation05"));*/

            //for after initial dialogue
            thatsWhatSheSaidClip = audioManager.FetchClip("Dialogue/6. Retaliation/Retaliation__That_s what she said_");
            //dialogueTime = audioManager.FetchClip("Dialogue/4. Ambush/Ambush__And we_re about to BLOW your(shoe)s off!!_").length;
            checkAudioClipList();
            audioClipsFetched = true;
        }

        if (allClipsPlayed == false)
            PlayAudioClipList();

        if (!ambushStarted)
        {
            ambushStarted = true;
            StartCoroutine(AmbushSequence2(GetTotalClipListLength()));
            Debug.Log("ClipListLength: " + GetTotalClipListLength());
        }

        // This event only "completes" when all ships are destroyed (or whatever your win condition is)
        if (ambushStarted && GetRemainingShips() == 1 && shipsHasGoneUpTo4)
        {
            ambushStarted = false;
            turretCanShoot = false;
            //ResetDialogue();
            return true;
        }

        return false;
    }

    IEnumerator AmbushSequence2(float clipListLength)
    {
        audioManager.StopMusic();
        pirateConversationDelay = 40f;
        Debug.Log("AmbushCoroutine is starting");
        yield return new WaitForSeconds(clipListLength - 25);
        audioManager.PlayMusicClip(audioManager.FetchClip("Music/Ambushes"));
        Debug.Log("Tried to play that's what she said");


        SpawnSingleShip();
        SpawnSingleShip();
        yield return new WaitForSeconds(3f);
        SpawnSingleShip();
        SpawnSingleShip();
        shipsHasGoneUpTo4 = true;

        yield return new WaitForSeconds(30f);

        audioManager.Play(thatsWhatSheSaidClip, true);

        /*        if (audioClipsFetched == false)
                {
                    audioClipList.Add(audioManager.FetchClip("Dialogue/6. Retaliation/Retaliation__That_s what she said_"));
                    checkAudioClipList();
                    audioClipsFetched = true;

                    if (allClipsPlayed == false)
                        PlayAudioClipList();
                }*/
    }

    bool breachTimersBeenSet = false;
    public bool turretCanShoot = true;
    bool breachCoroutineStarted = false;
    public bool helmetIsOn = false;
    AudioClip helmetClip;
    AudioClip gooGunPickup;
    AudioClip rotatorClip;
    AudioClip historyLesson;
    AudioClip playerIsRotatingClip;
    AudioClip blobMakesUncomfortable;
    AudioClip powerBankNeedsIt;
    private bool TheBreach() 
    {
        if (breachCoroutineStarted == false)
        {
            StartCoroutine(BreachCoroutine());
            breachCoroutineStarted = true;
        }

        if (audioClipsFetched == false)
        {
            audioClipList.Add(audioManager.FetchClip("Dialogue/6. Retaliation/Retaliation__THE WING HAS BEEN HIT_"));
            audioClipList.Add(audioManager.FetchClip("Dialogue/7. The Breach/Breach__Time to goo the wings_"));
            audioClipList.Add(audioManager.FetchClip("Dialogue/7. The Breach/Breach__Pirate Sherk Don_t Play Nice_"));
            audioClipList.Add(audioManager.FetchClip("Dialogue/7. The Breach/Breach__I love Spacewalks cause it_s when you Leave_"));

            //forLaterInCoroutine
            helmetClip = audioManager.FetchClip("Dialogue/7. The Breach/Breach__Gotta Put this helmet on_");
            gooGunPickup = audioManager.FetchClip("Dialogue/7. The Breach/Breach__Reverand_s Place_");
            historyLesson = audioManager.FetchClip("Dialogue/7. The Breach/Breach__This Goo makes me feel weird (Designed in 2120)_");
            rotatorClip = audioManager.FetchClip("Dialogue/7. The Breach/Breach__Time to Rotate_");
            playerIsRotatingClip = audioManager.FetchClip("Dialogue/7. The Breach/Breach_ROTATING");
            blobMakesUncomfortable = audioManager.FetchClip("Dialogue/7. The Breach/Breach__I will never get over how uncomfortable that makes me_");
            powerBankNeedsIt = audioManager.FetchClip("Dialogue/7. The Breach/Breach__Our Power bank got fried during the attack_");

            checkAudioClipList();
            audioClipsFetched = true;
        }

        if (allClipsPlayed == false)
            PlayAudioClipList();
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
            Debug.Log("Death sequence triggered cause of timer");
        }

        if (frontWingBlobbed && backWingBlobbed && powerBankBlobbed)
        {
            turretCanShoot = true;
            ResetDialogue();
            return true;
        }

        return false;
    }

    IEnumerator BreachCoroutine()
    {
        AudioClip musicClip = audioManager.FetchClip("Music/Pilot Mode");
        if (musicClip != null)
        {
            audioManager.PlayMusicClip(musicClip);
        }
        else
        {
            Debug.Log("Failed to load music clip for Breaches event.");
        }

        yield return new WaitUntil(() => currentRoom == "Airlock");
        audioManager.Play(helmetClip, true);

        yield return new WaitForSeconds(helmetClip.length + 0.25f);
        audioManager.Play(gooGunPickup, true);

        yield return new WaitForSeconds(gooGunPickup.length + 0.25f);
        audioManager.Play(historyLesson, true);

        yield return new WaitForSeconds(historyLesson.length + 0.25f);
        audioManager.Play(rotatorClip, true);

        historyLessonGiven = true;

        yield return new WaitUntil(() => script.playerIsRotating);
        audioManager.Play(playerIsRotatingClip, true);

        yield return new WaitUntil(() => frontWingBlobbed);
        audioManager.Play(blobMakesUncomfortable, true);

        yield return new WaitUntil(() => backWingBlobbed && frontWingBlobbed);
        audioManager.Play(powerBankNeedsIt, true);

        yield return new WaitUntil(() => script.playerIsRotating);
        audioManager.Play(playerIsRotatingClip, false);

        yield break;
    }

    private bool TheEVA() 
    {
        return true;
    }

    private bool TheRepair() 
    {
        return true;
    }

    bool calibrationCoroutineStarted = false;    

    AudioClip pirateBooty;
    private bool Calibration()
    { 
        if (goodBatteryInPlace)
        {

            TurretMonitorController turretScript = turretMonitor.GetComponent<TurretMonitorController>();
            turretScript.enabled = true;

            turretUIObject.SetActive(false);
        }

        if (calibrationCoroutineStarted == false)
        {
            StartCoroutine(CalibrationCoroutine());
            calibrationCoroutineStarted = true;
        }

        if (audioClipsFetched == false)
        {
            audioClipList.Add(audioManager.FetchClip("Dialogue/8. Calibration/Breach__All you have to do is get rid of the battery_"));
            audioClipList.Add(audioManager.FetchClip("Dialogue/8. CalibrationBreach__Make sure to put a new battery in the shield generator_"));


            //forLaterInCoroutine
            pirateBooty = audioManager.FetchClip("Dialogue/8. Calibration/Breach__Don_t Call us Pirate Booty_");


            checkAudioClipList();
            audioClipsFetched = true;
        }

        if (allClipsPlayed == false)
            PlayAudioClipList();


        if (batteryShotIntoSpace && goodBatteryInPlace && AllShipsDestroyed())
        {
            ResetDialogue();
            return true;

        }

        return false;
    }

    IEnumerator CalibrationCoroutine()
    {
        yield return new WaitUntil(() => goodBatteryInPlace);
        audioManager.Play(pirateBooty, true);

    }

    AudioClip cucarachaClip;
    bool steroidCoroutineInitialized = false;
    private bool AsteroidField() 
    {
        if (!steroidsSpawned)
        {
            asteroidSpawnScript.SpawnField(evasiveAsteroidCount, evasiveTimer);
            steroidsSpawned = true;
        }

        if (steroidCoroutineInitialized == false)
        {
            StartCoroutine(AsteroidCoroutine());    
            steroidCoroutineInitialized = true;
        }

        if (audioClipsFetched == false)
        {
            audioClipList.Add(audioManager.FetchClip("Dialogue/9. Asteroid Field/AsteroidField__The Asteroids are back and they brought their friends_"));

            //forLaterInCoroutine
            cucarachaClip = audioManager.FetchClip("Dialogue/9. Asteroid Field/AsteroidField__I can fly this thing better than the 3 cucarachas ever could_");


            checkAudioClipList();
            audioClipsFetched = true;
        }

        if (allClipsPlayed == false)
            PlayAudioClipList();

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
                ResetDialogue();
                return true;
            }
        }

        return false;
    }

    IEnumerator AsteroidCoroutine()
    {
        yield return new WaitForSeconds(10f);

        yield return new WaitUntil(() => currentRoom == "Pilot Room");
        audioManager.Play(cucarachaClip, true);

    }

    bool bossFightStarted = false;
    bool bossFightComplete = false;
    bool asteroidsShouldBeSpawned = true;
    bool bossMusicInit = false;
    AudioClip AAAAAAAAHHHHHHHHH;
    private bool BossFight() 
    {
        pirateConversationDelay = 50f;
        if (audioClipsFetched == false)
        {
            audioClipList.Add(audioManager.FetchClip("Dialogue/10. BOSSFIGHT/BOSSFIGHT__It_s me, the big Boss Man_"));
            audioClipList.Add(audioManager.FetchClip("Dialogue/10. BOSSFIGHT/BOSSFIGHT__Eat my blasts_"));

            //later
            AAAAAAAAHHHHHHHHH = audioManager.FetchClip("Dialogue/10. BOSSFIGHT/BOSSFIGHT__The Pollywoggle Clan will not forget this_");
            checkAudioClipList();   
            audioClipsFetched = true;
        }

        if (allClipsPlayed == false)
            PlayAudioClipList();


        if (!bossMusicInit)
        {
            AudioClip musicClip = audioManager.FetchClip("Music/BOSSFIGHT");
            if (musicClip != null)
            {
                audioManager.PlayMusicClip(musicClip);
                bossMusicInit = true;
            }
            else
            {
                Debug.Log("Failed to load music clip for The Wake-Up event.");
            }
        }
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
           /* ResetDialogue();
            if (audioClipsFetched == false)
            {
                audioClipList.Add(audioManager.FetchClip("BOSSFIGHT__The Pollywoggle Clan will not forget this_"));


                for (int i = 0; i < audioClipList.Count; i++)
                {
                    if (audioClipList[i] == null)
                    {
                        Debug.Log("Failed to load audio clip at index " + i + " for Routine event.");
                    }
                    else
                    {
                        Debug.Log("Successfully loaded audio clip: " + audioClipList[i].name);
                    }
                }
                audioClipsFetched = true;
            }

            if (allClipsPlayed == false)
                PlayAudioClipList();*/
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
        pirateConversationDelay = 40f;
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
        audioManager.Play(AAAAAAAAHHHHHHHHH, true);

        bossFightComplete = true;
    }

    private bool Conclusion() 
    {
        script.SetUIText("You have won the game", true);




        return true;
    }

}