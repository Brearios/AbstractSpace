using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isRunning;
    public Empire playerEmpire;
    public SectorValues currentSector;
    public List<Empire> knownEmpires;
    public int playerDefeatedEmpires;
    public int currentWars;
    public int spaceYear;
    public float gameSpeed;
    public float deltaTime;
    public float timeIncrement;

    //TODO - War button and screen

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
        playerEmpire.grossEmpireProduct = 10;
        playerEmpire.discoveredPlanets = 0;
        playerEmpire.colonizedPlanets = 1;
    }

    private void CustomizeEmpire()
    {
        // Set MagicNumbers Empire Variables via text boxes
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
        foreach (Empire currentEmpire in knownEmpires)
        {
            CalculateProgress(currentEmpire);
        }
    }



    void CalculateProgress(Empire empire)
    {
        foreach (SectorValues currentSector in empire.empireSectors)
            ProcessSectorFunding(empire, currentSector);

        //GrowEconomy(empire, empire.economy);
        //ExploreStars(empire, empire.exploration);
        //ColonizePlanets(empire, empire.colonization);
        //BuildMilitaryShips(empire, empire.military);
        //FundResearchGrants(empire, empire.science);
        //SendSpaceDiplomats(empire, empire.diplomacy);
    }

    private void ProcessSectorFunding(Empire empire, SectorValues currentSector)
    {
        float iteratedInvestment = ((empire.grossEmpireProduct * currentSector.fundingAllocation) / 10);
        iteratedInvestment += (iteratedInvestment * currentSector.sectorScienceMultiplier);
        currentSector.currentInvestment += iteratedInvestment;
        while (currentSector.currentInvestment > currentSector.neededInvestment)
        {
            UpgradeSector(currentSector);
            UpgradeEmpire(empire, currentSector.name);
        }
    }

    private void UpgradeSector(SectorValues sector)
    {
        sector.growthLevelsAchieved += 1;
        sector.currentInvestment -= sector.neededInvestment;
        sector.neededInvestment *= MagicNumbers.Instance.upgradeCostMultiplier;
    }

    private void UpgradeEmpire(Empire empire, string sectorName)
    {
        switch (sectorName)
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
                //
                break;

            case "diplomacy":
                //
                break;
        }
    }

    void DiscoverAlienRace()
    {
        //catchUpTurnCalculations();
        //add to knownEmpires();
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
        //TODO - add random chance for stat increase, new alien race, space pirates, discover planet, etc.
        empire.discoveredPlanets++;
    }
}
