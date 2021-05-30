using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool playerLoss;
    public bool isRunning;
    public bool allocating; // Tracks whether Player is on the Allocation Screen
    public bool notificationsOn;
    public bool playerDiplomacyTowardExterminators;
    public bool playerDiplomacyTowardXenophobes;
    public bool playerDiplomacyTowardModerates;
    public Empire playerEmpire;
    public Empire alienTemplateEmpire;
    public SectorDetails currentSector;
    public List<Empire> allEmpires;
    // public List<Empire> knownEmpires; Moved to the empire script as encounteredEmpires
    public List<Empire> newEmpiresToAdd;
    public List<Empire> empiresAtWarWithPlayer;
    public int enemyFleetStrength;
    public int playerDefeatedEmpires;
    public int currentWars;
    public int spaceYear;
    public float gameSpeed;
    public float deltaTime;
    public float timeIncrement;
    public GameObject alienEmpire;
    public string currentStoredInput;
    public string inputFieldTitle;

    public GameObject notificationTextActivator;
    public string currentNotification;
    public List<String> notificationsToDisplay;
    public bool allocationsAvailable;
    public int numberOfAllocationsAvailable;

    //TODO - War button and screen
    //TODO - connect gameSpeed to deltaTime
    //TODO - generate race details

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SetPlayEmpireDefaults(playerEmpire);
    }



    // Start is called before the first frame update
    void Start()
    {
        gameSpeed = 1.0f;
        timeIncrement = .2f;
        spaceYear = 1;

        // TODO - Text box - press S to start, or C to customize your empire
        // TODO - Buttons for Start and Customize
        // CustomizeEmpire();
        // EstablishPlayerEmpire();
        
        
        // Uncomment once UI is complete/working
        // AllocateSpending();
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isRunning = !isRunning; // Toggle pause Bool when space is pressed
        }

        if (!isRunning)
        {
            return;
        }

        TimeControls();

        // REMOVE - No longer need foreach due to move of CalculateProgress to Empire.cs
        // ProcessKnownEmpries();

        // REMOVE - No longer interacting with lists, from within lists
        //AddEmpiresAndResetList();

        ShowNotifications();

        ClearNotifications();

        CalculateEnemyFleetStrength();        

        IncrementYear();

        
    }

    private void ClearNotifications()
    {
        notificationsToDisplay.Clear();
    }

    private void ShowNotifications()
    {
        if (notificationsOn)
        {
            PanelActivator.Instance.notificationTextActivator.SetActive(true);
            foreach (string notification in notificationsToDisplay)
            {
                currentNotification = notification;

                isRunning = false;
                //while (!isRunning)
                //{
                //    return;
                //}
            }
            if (isRunning)
            {
                PanelActivator.Instance.notificationTextActivator.SetActive(false);
            }
        }

    }

    private void SetPlayEmpireDefaults(Empire playerEmpire)
    {
        playerEmpire.Name = MagicNumbers.Instance.PlayerEmpireName;
        
        // Excluded for now, until I see if I need them
        // playerEmpire.Adjective = MagicNumbers.Instance.PlayerAdjective;

        playerEmpire.rulerName = MagicNumbers.Instance.PlayerRuler;
        playerEmpire.grossEmpireProduct = MagicNumbers.Instance.StartingGrossEmpireProduct;
        playerEmpire.discoveredPlanets = 0;
        playerEmpire.colonizedPlanets = 1;
        playerEmpire.colonyShips = 0;
    }

    private void CustomizeEmpire()
    {
        PanelActivator.Instance.CustomizeEmpire();
        // Set MagicNumbers Empire Variables via text boxes
        // TODO - implement customization with text boxes
    }

    private void TimeControls()
    {
        deltaTime = (gameSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (gameSpeed > 0)
            {
                gameSpeed *= 2;
                //gameSpeed += timeIncrement;
            }
            else if (gameSpeed >= 0)
            {
                gameSpeed += 0.1f;
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (gameSpeed <= timeIncrement)
            {
                gameSpeed = 0f;
            }

            else
            {
                gameSpeed /= 2;
                //gameSpeed /= timeIncrement;
            }
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            gameSpeed = 1.0f;
        }
    }

    void CalculateEnemyFleetStrength()
    {
        int enemyFleetStrengthCalculator;
        enemyFleetStrengthCalculator = 0;
        foreach (Empire enemyEmpire in GameManager.Instance.playerEmpire.empiresAtWarWithThisEmpire)
        {
            enemyFleetStrengthCalculator += enemyEmpire.fleetStrength;
        }
        enemyFleetStrength = enemyFleetStrengthCalculator;
    }

    void IncrementYear()
    {
        spaceYear++;
    }
}
