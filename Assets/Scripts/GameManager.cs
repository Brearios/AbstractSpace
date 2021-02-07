using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isRunning;
    public bool allocating;
    public Empire playerEmpire;
    public Empire alienTemplateEmpire;
    public SectorDetails currentSector;
    public List<Empire> knownEmpires;
    public List<Empire> newEmpiresToAdd;
    public int playerDefeatedEmpires;
    public int currentWars;
    public int spaceYear;
    public float gameSpeed;
    public float deltaTime;
    public float timeIncrement;
    public GameObject alienEmpire;

    public GameObject notificationTextActivator;

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

        if (Input.GetKeyDown(KeyCode.A))
        {
            AllocateSpending();
            //TODO - switch activations
        }


        TimeControls();



        // REMOVE - No longer need foreach due to move of CalculateProgress to Empire.cs
        // ProcessKnownEmpries();

        AddEmpiresAndResetList();

        IncrementYear();


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
        // Set MagicNumbers Empire Variables via text boxes
        // TODO - implement customization with text boxes
        throw new NotImplementedException();
    }

    private void AllocateSpending()
    {
        isRunning = false;
        // Activate Buttons
        // Deactivate Extra Menus
        isRunning = true;
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

    // REMOVE - moved to Empire.cs
    // Moved CalculateProgress to Empire.cs, no longer necessary
    //private void ProcessKnownEmpries()
    //{
    //    if (LogManager.Instance.logsEnabled)
    //        {
    //        if (LogManager.Instance.calculateProgressLogs)
    //        {
    //            Debug.Log($"Processing empires, in space year {spaceYear}.");
    //        }
    //    }
    //    foreach (Empire currentEmpire in knownEmpires)
    //    {
    //        CalculateProgress(currentEmpire);
    //    }
    //}

    // REMOVE - moved to Empire.cs
    // Moved to CalculateProgress in Empire.cs, no longer necessary
    //private void ProcessSectorFunding(Empire empire, SectorDetails currentSector)
    //{
    //    float iteratedInvestment = ((empire.grossEmpireProduct * currentSector.fundingAllocation) / (100 / MagicNumbers.Instance.allocationIterationAmount));
    //    iteratedInvestment += ((empire.bonusResourcesFromEvents * currentSector.fundingAllocation) / (100 / MagicNumbers.Instance.allocationIterationAmount));
    //    iteratedInvestment += (iteratedInvestment * currentSector.sectorScienceMultiplier);
    //    currentSector.currentInvestment += iteratedInvestment;
    //    while (currentSector.currentInvestment > currentSector.neededInvestment)
    //    {
    //        UpgradeEmpire(empire, currentSector);
    //        UpgradeSector(currentSector);
    //    }

    //}

    // REMOVE - moved to Empire.cs
    //private void UpgradeSector(SectorDetails sector)
    //{
    //    sector.growthLevelsAchieved += 1;
    //    sector.currentInvestment -= sector.neededInvestment;
    //    sector.neededInvestment *= MagicNumbers.Instance.upgradeCostMultiplier;
    //    Debug.Log($"Upgraded {sector.sectorName}, to {sector.growthLevelsAchieved} in space year {spaceYear}.");
    //}

    // REMOVE - moved to Empire.cs
    //private void UpgradeEmpire(Empire empire, SectorDetails sector)
    //{
    //    if (LogManager.Instance.logsEnabled)
    //    {
    //        if (LogManager.Instance.empireUpgradeLogs)
    //        {

    //            Debug.Log($"Upgrading {empire.Name}'s {sector.sectorName} in space year {spaceYear}.");
    //        }
    //    }
    //    switch (sector.sectorName)
    //    {
    //        case "economy":
    //            GrowGEP(empire);
    //            break;

    //        case "exploration":
    //            ExploreStar(empire);
    //            break;

    //        case "colonization":
    //            //TODO May want to eventually have individual planets with their own bonuses that roll here
    //            if (empire.discoveredPlanets > 0)
    //            {
    //                empire.colonizedPlanets++;
    //                empire.discoveredPlanets--;
    //            }
    //            else
    //            {
    //                empire.colonyShips++;
    //            }

    //            break;

    //        case "military":
    //            if (LogManager.Instance.logsEnabled)
    //            {
    //                if (LogManager.Instance.fleetUpgradeLogs)
    //                {
    //                    Debug.Log($"Upgrading fleet - from {empire.fleetStrength} to {(empire.fleetStrength + (empire.fleetStrength * MagicNumbers.Instance.fleetStrengthUpgradeMultiplier))}.");
    //                }
    //            }

    //            empire.fleetStrength++;
    //            //TODO Multiply by a value - say 1.08 - rounding up if less than 1?
    //            break;

    //        case "science":
    //            int selectedSector = UnityEngine.Random.Range(1, 7);
    //            switch (selectedSector)
    //            {
    //                case 1:
    //                    empire.economy.sectorScienceMultiplier += 0.1f;
    //                    break;
    //                case 2:
    //                    empire.exploration.sectorScienceMultiplier += 0.1f;
    //                    break;
    //                case 3:
    //                    empire.colonization.sectorScienceMultiplier += 0.1f;
    //                    break;
    //                case 4:
    //                    empire.military.sectorScienceMultiplier += 0.1f;
    //                    break;
    //                case 5:
    //                    empire.science.sectorScienceMultiplier += 0.1f;
    //                    break;
    //                case 6:
    //                    empire.diplomacy.sectorScienceMultiplier += 0.1f;
    //                    break;
    //            }
    //            break;

    //        case "diplomacy":
    //            empire.diplomaticCapacity++;
    //            break;
    //    }
    //}

    void IncrementYear()
    {
        spaceYear++;
    }

    // REMOVE - moved to Empire.cs
    //void GrowGEP(Empire empire)
    //{
    //    float economyIncrease = (1.0f + (empire.colonizedPlanets * MagicNumbers.Instance.planetGrossEmpireProductContribution));
    //    {
    //        if (LogManager.Instance.logsEnabled)
    //        {
    //            if (LogManager.Instance.growGEPLogs)
    //            {
    //                Debug.Log($"Economy increasing by {economyIncrease}, from {playerEmpire.grossEmpireProduct}, in space year {spaceYear}.");
    //            }
    //        }
    //    }
    //    empire.grossEmpireProduct += economyIncrease;
    //}

    // REMOVE - moved to Empire.cs
    //void ExploreStar(Empire empire)
    //{
    //    empire.exploredStars++;
    //    RollStarOutcome(empire);
    //}

    // REMOVE - moved to Empire.cs
    //private void RollStarOutcome(Empire empire)
    //{
    //    int random = UnityEngine.Random.Range(1, 100);
    //    if (random < 34)
    //    {
    //        if (empire.colonyShips > 0)
    //        {
    //            empire.colonyShips--;
    //            empire.colonizedPlanets++;
    //        }
    //        else
    //        {
    //            empire.discoveredPlanets++;
    //        }
    //    }
    //    else if (random < 67)
    //    {
    //        //TODO - figure out if I want alien empires meeting alien empires and having their own trade and war
    //        if (empire.isPlayer)
    //        {
    //            DiscoverAlienEmpire();
    //        }
    //        else
    //        {
    //            FindBonusResources(empire);
    //        }
    //    }
    //    else if (random < 100)
    //    {
    //        FindBonusResources(empire);
    //    }
    //}

    // REMOVE - moved to Empire.cs
    //private void FindBonusResources(Empire empire)
    //{
    //    float treasureAmount = (empire.grossEmpireProduct * MagicNumbers.Instance.treasurePortionOfGEP);
    //    empire.bonusResourcesFromEvents += treasureAmount;
    //}

    // REMOVE - moved to Empire.cs
    //void DiscoverAlienEmpire()
    //{
    //    // Give each empire a unique name - Unsure if this is necessary
        
    //    //GenerateEmpireDetails();
    //    //GenerateRaceDetails();

    //    // RTEMOVE
    //    // Race discoveredEmpireRace = ScriptableObject.CreateInstance<Race>();

    //    // ODO - move this below Empire, and don't use new Race
    //    (string raceName, string raceAdjective, string raceHomeworld) = SyncronizedRaceDetailsGrabber(RandomNamesAndElements.Instance.raceNameGenerationList, RandomNamesAndElements.Instance.raceAdjectiveGenerationList, RandomNamesAndElements.Instance.raceHomeworldGenerationList);
    //    Race discoveredEmpireRace = new Race();
    //    discoveredEmpireRace.raceName = raceName;
    //    discoveredEmpireRace.raceAdjective = raceAdjective;
    //    discoveredEmpireRace.raceHomeworld = raceHomeworld;

    //    GameObject tempEmpireObject = Instantiate(alienEmpire);
    //    Empire discoveredEmpire;
    //    discoveredEmpire = tempEmpireObject.GetComponent<Empire>();
    //    discoveredEmpire.race = discoveredEmpireRace;
    //    discoveredEmpire.Name = ListSingleObjectGrabber(RandomNamesAndElements.Instance.raceNameGenerationList);
    //    // TODO - overload ListObjectGrabber to get name, adjective, and homeworld from same instance

    //    // Removed until I figure out if paragraphs need seperate adjectives
    //    // string empAdjective = empName;
    //    // TODO - having the adjective be the name is probably terrible - will need to revisit

    //    discoveredEmpire.rulerName = ListSingleObjectGrabber(RandomNamesAndElements.Instance.emperorNameGenerationList);

    //    // REMOVE
    //    // Empire discoveredEmpire = ScriptableObject.CreateInstance<Empire>();


    //    CreateAndListAlienSectors(discoveredEmpire);

    //    for (int i = 0; i < (100 / MagicNumbers.Instance.allocationIterationAmount);  i++)
    //    {
    //        // Determine how empire will allocate it's economy
    //        // TODO - tie this to priorities
    //        AllocateEconomy(discoveredEmpire);
    //    }
    //    for (int i = 1; i < spaceYear; i++)
    //    {
    //        CalculateProgress(discoveredEmpire);
    //    }
    //    //CatchUpTurnCalculations();
    //    newEmpiresToAdd.Add(discoveredEmpire);
    //    //add to knownEmpires();
    //}

    // REMOVE - should be handled by Start in Empire.cs
    //private void CreateAndListAlienSectors(Empire discoveredEmpire)
    //{

    //    if (!discoveredEmpire.isPlayer)
    //    {
    //        discoveredEmpire.empireSectors.Add(discoveredEmpire.economy);
    //        discoveredEmpire.empireSectors.Add(discoveredEmpire.exploration);
    //        discoveredEmpire.empireSectors.Add(discoveredEmpire.colonization);
    //        discoveredEmpire.empireSectors.Add(discoveredEmpire.military);
    //        discoveredEmpire.empireSectors.Add(discoveredEmpire.science);
    //        discoveredEmpire.empireSectors.Add(discoveredEmpire.diplomacy);
    //    }
    //}

    // REMOVE - moved to Empire.cs
    private void AllocateEconomy(Empire empire)
    {
        // Choose a Sector
        // Add 10 to that sector
        int allocation = UnityEngine.Random.Range(0, 6);
        if (LogManager.Instance.logsEnabled)
        {
            if (LogManager.Instance.alienAllocationLogs)
            {
                Debug.Log($"Allocating {MagicNumbers.Instance.allocationIterationAmount} percent of {empire.Name} economy to sector {empire.empireSectors[allocation].sectorName}.");
            }
        }
        empire.empireSectors[allocation].fundingAllocation += MagicNumbers.Instance.allocationIterationAmount;
    }


    // REMOVE - moved to Empire.cs
    string ListSingleObjectGrabber(List<string> listName)
    {
        int randIndex = UnityEngine.Random.Range(1, listName.Count);
        return listName[randIndex];
    }

    // REMOVE - moved to Empire.cs
    (string raceName, string raceAdjective, string raceHomeworld) SyncronizedRaceDetailsGrabber(List<string> nameList, List<string> adjectiveList, List<string> homeworldList)
    {
        int randIndex = UnityEngine.Random.Range(1, nameList.Count);
        string raceName = nameList[randIndex];
        string raceAdjective = adjectiveList[randIndex];
        string raceHomeworld = homeworldList[randIndex];

        return (raceName, raceAdjective, raceHomeworld);
    }

    void TextBoxNotificationActivator()
    {    }

    void NotificationTextPopulator()
    {
        isRunning = false;
        notificationTextActivator.SetActive(false);
        // TODO this whole thing
        // fillText;
        // displayText;   
    }


    // Remove - added to known empires in Empire.Start
    void AddEmpiresAndResetList()
    {
        foreach (Empire currentEmpire in newEmpiresToAdd)
        {
            knownEmpires.Add(currentEmpire);
        }
        newEmpiresToAdd.Clear();
    }
}
