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
    public SectorValues currentSector;
    public List<Empire> knownEmpires;
    public int playerDefeatedEmpires;
    public int currentWars;
    public int spaceYear;
    public float gameSpeed;
    public float deltaTime;
    public float timeIncrement;
    public int empireLabelInt;

    public GameObject notificationTextActivator;

    //TODO - War button and screen
    //TODO - connect gameSpeed to deltaTime

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
        empireLabelInt = 1;

        // TODO - Text box - press S to start, or C to customize your empire
        // TODO - Buttons for Start and Customize
        // CustomizeEmpire();
        // EstablishPlayerEmpire();

        knownEmpires.Add(playerEmpire);
        AllocateSpending();
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
        }


        TimeControls();



        ProcessKnownEmpries();

        IncrementYear();


    }

    private void SetPlayEmpireDefaults(Empire playerEmpire)
    {
        playerEmpire.Name = MagicNumbers.Instance.PlayerEmpireName;
        playerEmpire.Adjective = MagicNumbers.Instance.PlayerAdjective;
        playerEmpire.rulerName = MagicNumbers.Instance.PlayerRuler;
        playerEmpire.grossEmpireProduct = MagicNumbers.Instance.StartingGrossEmpireProduct;
        playerEmpire.discoveredPlanets = 0;
        playerEmpire.colonizedPlanets = 1;
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

    private void ProcessKnownEmpries()
    {
        Debug.Log($"Processing empires, in space year {spaceYear}.");   
        foreach (Empire currentEmpire in knownEmpires)
        {
            CalculateProgress(currentEmpire);
        }
    }



    void CalculateProgress(Empire empire)
    {
        foreach (SectorValues currentSector in empire.empireSectors)
            ProcessSectorFunding(empire, currentSector);
            Debug.Log($"Calculating progress for {empire}, in space year {spaceYear}.");

        //GrowEconomy(empire, empire.economy);
        //ExploreStars(empire, empire.exploration);
        //ColonizePlanets(empire, empire.colonization);
        //BuildMilitaryShips(empire, empire.military);
        //FundResearchGrants(empire, empire.science);
        //SendSpaceDiplomats(empire, empire.diplomacy);
        empire.bonusResourcesFromEvents = 0;
    }

    private void ProcessSectorFunding(Empire empire, SectorValues currentSector)
    {
        float iteratedInvestment = ((empire.grossEmpireProduct * currentSector.fundingAllocation) / 10);
        iteratedInvestment += ((empire.bonusResourcesFromEvents * currentSector.fundingAllocation) / 10);
        iteratedInvestment += (iteratedInvestment * currentSector.sectorScienceMultiplier);
        currentSector.currentInvestment += iteratedInvestment;
        while (currentSector.currentInvestment > currentSector.neededInvestment)
        {
            UpgradeEmpire(empire, currentSector);
            UpgradeSector(currentSector);
        }

    }

    private void UpgradeSector(SectorValues sector)
    {
        sector.growthLevelsAchieved += 1;
        sector.currentInvestment -= sector.neededInvestment;
        sector.neededInvestment *= MagicNumbers.Instance.upgradeCostMultiplier;
        Debug.Log($"Upgraded {sector}, in space year {spaceYear}.");
    }

    private void UpgradeEmpire(Empire empire, SectorValues sector)
    {
        Debug.Log($"Upgrading {empire}'s {sector.name} in space year {spaceYear}.");
        switch (sector.name)
        {
            case "economy":
                GrowGEP(empire);
                break;

            case "exploration":
                ExploreStar(empire);
                break;

            case "colonization":
                //TODO May want to eventually have individual planets with their own bonuses that roll here
                empire.colonizedPlanets++;
                break;

            case "military":
                empire.fleetStrength++;
                break;

            case "science":
                int selectedSector = UnityEngine.Random.Range(1, 6);
                switch (selectedSector)
                {
                    case 1:
                        empire.economy.sectorScienceMultiplier += 0.1f;
                        break;
                    case 2:
                        empire.exploration.sectorScienceMultiplier += 0.1f;
                        break;
                    case 3:
                        empire.colonization.sectorScienceMultiplier += 0.1f;
                        break;
                    case 4:
                        empire.military.sectorScienceMultiplier += 0.1f;
                        break;
                    case 5:
                        empire.science.sectorScienceMultiplier += 0.1f;
                        break;
                    case 6:
                        empire.diplomacy.sectorScienceMultiplier += 0.1f;
                        break;
                }
                break;

            case "diplomacy":
                empire.diplomaticCapacity++;
                break;
        }
    }

    void IncrementYear()
    {
        spaceYear++;
    }

    private void GrowGEP(Empire empire)
    {
        float economyIncrease = (1.0f + (empire.colonizedPlanets * MagicNumbers.Instance.planetGrossEmpireProductContribution));
        if (empire == playerEmpire)
        {
            Debug.Log($"Economy increasing by {economyIncrease}, from {playerEmpire.grossEmpireProduct}, in space year {spaceYear}.");
        }
        empire.grossEmpireProduct += economyIncrease;
    }

    void ExploreStar(Empire empire)
    {
        empire.exploredStars++;
        RollStarOutcome(empire);
    }

    private void RollStarOutcome(Empire empire)
    {
        int random = UnityEngine.Random.Range(1, 100);
        if (random < 34)
        {
            empire.discoveredPlanets++;
        }
        else if (random < 67)
        {
            DiscoverAlienEmpire();
        }
        else if (random < 100)
        {
            FindBonusResources(empire);
        }
    }

    private void FindBonusResources(Empire empire)
    {
        float treasureAmount = (empire.grossEmpireProduct * MagicNumbers.Instance.treasurePortionOfGEP);
        empire.bonusResourcesFromEvents += treasureAmount;
    }


    void DiscoverAlienEmpire()
    {
        // Give each empire a unique name - Unsure if this is necessary
        //GenerateEmpireDetails();
        //GenerateRaceDetails();
        Race discoveredEmpireRace = new Race();
        string empName = ListObjectGrabber(RandomElementsObject.Instance.raceNameGenerationList);
        // TODO - switch statement with empire names for race names
        string empAdjective = empName;
        // TODO - having the adjective be the name is probably terrible - will need to revisit
        string ruler = ListObjectGrabber(RandomElementsObject.Instance.emperorNameGenerationList);
        Empire discoveredEmpire = new Empire(discoveredEmpireRace, empName, empAdjective, ruler);
        for (int i = 0; i < (MagicNumbers.Instance.allocationIterationAmount / 100);  i++)
        {
            AllocateEconomy(discoveredEmpire);
        }
        //CatchUpTurnCalculations();
        knownEmpires.Add(discoveredEmpire);
        //add to knownEmpires();
    }

    private void AllocateEconomy(Empire empire)
    {
        // Choose a Sector
        // Add 10 to that sector
        empire.empireSectors[UnityEngine.Random.Range(1, 6)].fundingAllocation += MagicNumbers.Instance.allocationIterationAmount;
    }

    string ListObjectGrabber(List<string> listName)
    {
        int randIndex = UnityEngine.Random.Range(1, listName.Count);
        return listName[randIndex];
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
}
