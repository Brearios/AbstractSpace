using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isRunning;
    public Empire playerEmpire;
    public List<Empire> knownEmpires;
    public int spaceYear;
    public float gameSpeed;
    public float deltaTime;
    public float timeIncrement;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        gameSpeed = 1.0f;
        timeIncrement = .2f;
        spaceYear = 1;

        // EstablishPlayerEmpire();

        knownEmpires.Add(playerEmpire);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isRunning = !isRunning; // Toggle pause Bool when space is pressed
        }


        TimeControls();



        ProcessKnownEmpries();

        IncrementYear();

        void IncrementYear()
        {
            spaceYear++;
        }
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
            ProcessSectorGrowth(empire, currentSector);

            //GrowEconomy(empire, empire.economy);
            //ExploreStars(empire, empire.exploration);
            //ColonizePlanets(empire, empire.colonization);
            //BuildMilitaryShips(empire, empire.military);
            //FundResearchGrants(empire, empire.science);
            //SendSpaceDiplomats(empire, empire.diplomacy);
    }

    private void ProcessSectorGrowth(Empire empire, SectorValues currentSector)
    {
        currentSector.currentInvestment += ((empire.grossEmpireProduct * currentSector.fundingAllocation) / 10);
        if (currentSector.currentInvestment > currentSector.neededInvestment)
        {
            upgradeSector(currentSector);
            upgradeEmpire(currentSector);
        }
    }

    private void upgradeSector(SectorValues sector)
    {
        sector.growthLevelsAchieved += 1;
        sector.currentInvestment -= sector.neededInvestment;
        sector.neededInvestment *= MagicNumbers.Instance.upgradeCostMultiplier;
    }

    private void upgradeEmpire(SectorValues sector)
    {
        throw new NotImplementedException();
    }



    void DiscoverAlienRace()
    {
        //catchUpTurnCalculations();
        //add to knownEmpires();
    }
}
