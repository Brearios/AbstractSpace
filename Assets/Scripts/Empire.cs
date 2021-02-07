using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empire : MonoBehaviour
{
    public Race race;
    public bool AtWarWithPlayer;
    public bool AlliedWithPlayer;
    public string Name;
    public string rulerName;
    public string discoveredBy;
    public string boastWord;
    public string governmentWord;
    public bool isPlayer;
    public enum sixDegrees { Player, Rival, Second, Third, Fourth, Fifth }
    // TODO Less calculation for those further "out", determine number of desired degrees, method of discovering/promoting known empries
    // TODO - Record who discovered the empire, with GetInstanceID();

    public ScriptableEmpire empireTemplate;

    // Removed until I see how the paragraphs play out
    // public string Adjective;    
    public float grossEmpireProduct;
    public float bonusResourcesFromEvents;
    public int exploredStars;
    public int discoveredPlanets;
    public int colonizedPlanets;
    public int colonyShips;
    public int fleetStrength;
    public int relationsTowardPlayer;
    public int diplomaticCapacity; // Represents diplomats, analysts, space anthropologists
    // 1-100, with 1 being war, 2-34 being hostile, 35-65 being peace, 66-99 being trade, and 100 being allies
    public float economyAllocation;
    public float explorationAllocation;
    public float colonizationAllocation;
    public float militaryAllocation;
    public float scienceAllocation;
    public float diplomacyAllocation;

    public float expectedEconomyBudget;
    public float expectedExplorationBudget;
    public float expectedColonizationBudget;
    public float expectedMilitaryBudget;
    public float expectedScienceBudget;
    public float expectedDiplomacyBudget;

    public SectorDetails economy;
    public SectorDetails exploration;
    public SectorDetails colonization;
    public SectorDetails military;
    public SectorDetails science;
    public SectorDetails diplomacy;

    // Potentially could add population numbers, to be factored into grossEmpireProduct, with a sector for spending that would increase growth rate
    public List<SectorDetails> empireSectors = new List<SectorDetails>();

    private void Start()
    {
            if (LogManager.Instance.logsEnabled)
            {
                if (LogManager.Instance.addSectorsToListLogs)
                {
                    Debug.Log($"Attempting to add 6 sectors to {Name}'s empireSectors list, in space year {GameManager.Instance.spaceYear}.");
                }
            }

            empireSectors.Add(economy);
            empireSectors.Add(exploration);
            empireSectors.Add(colonization);
            empireSectors.Add(military);
            empireSectors.Add(science);
            empireSectors.Add(diplomacy);

            if (LogManager.Instance.logsEnabled)
            {
                if (LogManager.Instance.addSectorsToListLogs)
                {
                    Debug.Log($"Added {empireSectors.Count} sectors to {Name}'s empireSectors list, in space year {GameManager.Instance.spaceYear}.");
                }
            }
        InitializeEmpireAddSectorsAndSetGEP(this);
        if (!isPlayer)
        {
            InitializeAlienEmpireDetails();
            CalculateProgressToSpaceyear();
            GameManager.Instance.knownEmpires.Add(this);
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.isRunning)
        {
            return;
        }

        CalculateProgress();

        UpdateEmpireAllocations();

        CalculateExpectedBudgets();
    }

    private void InitializeEmpireAddSectorsAndSetGEP(Empire empire)
    {
        empire.grossEmpireProduct = MagicNumbers.Instance.StartingGrossEmpireProduct;
        empire.exploredStars = 0;
        empire.discoveredPlanets = 0;
        empire.colonizedPlanets = MagicNumbers.Instance.StartingColonizedPlanets;
        empire.fleetStrength = 1;
        empire.diplomaticCapacity = 0;
        foreach (SectorDetails sector in empire.empireSectors)
        {
            sector.sectorName = sector.sectorValuesTemplate.sectorName;
            if (LogManager.Instance.logsEnabled)
            {
                if (LogManager.Instance.initializeFundingLogs)
                {
                    Debug.Log($"Initializing funding for {sector.sectorName}, in space year {GameManager.Instance.spaceYear}.");
                }
            }
            sector.fundingAllocation = sector.sectorValuesTemplate.fundingAllocation;
            sector.growthLevelsAchieved = sector.sectorValuesTemplate.growthLevelsAchieved;
            sector.currentInvestment = sector.sectorValuesTemplate.currentInvestment;
            sector.neededInvestment = sector.sectorValuesTemplate.neededInvestment;
            sector.sectorScienceMultiplier = sector.sectorValuesTemplate.sectorScienceMultiplier;
        }

        // These read from Scriptable Sectors at start
        //foreach (SectorDetails sector in empire.empireSectors)
        //{
        //    sector.growthLevelsAchieved = 0;
        //    sector.currentInvestment = 0;
        //    sector.neededInvestment = 10;
        //    sector.sectorScienceMultiplier = 0.0f;
        //}
    }


    void CalculateProgress()
    {
        if (LogManager.Instance.logsEnabled)
        {
            if (LogManager.Instance.calculateProgressLogs)
            {
                Debug.Log($"Calculating progress for {Name}, in space year {GameManager.Instance.spaceYear}.");
            }
        }
        foreach (SectorDetails currentSector in empireSectors)
        {
            float iteratedInvestment = ((grossEmpireProduct * currentSector.fundingAllocation) / (100 / MagicNumbers.Instance.allocationIterationAmount));
            iteratedInvestment += ((bonusResourcesFromEvents * currentSector.fundingAllocation) / (100 / MagicNumbers.Instance.allocationIterationAmount));
            iteratedInvestment += (iteratedInvestment * currentSector.sectorScienceMultiplier);
            currentSector.currentInvestment += iteratedInvestment;
            while (currentSector.currentInvestment > currentSector.neededInvestment)
            {
                UpgradeEmpire(currentSector);
                UpgradeSector(currentSector);
            }
        }

        // Remove Bonus Resources now spent
        bonusResourcesFromEvents = 0;
    }

    private void UpgradeEmpire(SectorDetails sector)
    {
        if (LogManager.Instance.logsEnabled)
        {
            if (LogManager.Instance.empireUpgradeLogs)
            {

                Debug.Log($"Upgrading {Name}'s {sector.sectorName} in space year {GameManager.Instance.spaceYear}.");
            }
        }
        switch (sector.sectorName)
        {
            case "economy":
                GrowGEP();
                break;

            case "exploration":
                ExploreStar();
                break;

            case "colonization":
                //TODO May want to eventually have individual planets with their own bonuses that roll here
                if (discoveredPlanets > 0)
                {
                    colonizedPlanets++;
                    discoveredPlanets--;
                }
                else
                {
                    colonyShips++;
                }

                break;

            case "military":
                if (LogManager.Instance.logsEnabled)
                {
                    if (LogManager.Instance.fleetUpgradeLogs)
                    {
                        Debug.Log($"Upgrading fleet - from {fleetStrength} to {(fleetStrength + (fleetStrength * MagicNumbers.Instance.fleetStrengthUpgradeMultiplier))}.");
                    }
                }

                fleetStrength++;
                //TODO Multiply by a value - say 1.08 - rounding up if less than 1?
                break;

            case "science":
                int selectedSector = UnityEngine.Random.Range(1, 7);
                switch (selectedSector)
                {
                    case 1:
                        economy.sectorScienceMultiplier += 0.1f;
                        break;
                    case 2:
                        exploration.sectorScienceMultiplier += 0.1f;
                        break;
                    case 3:
                        colonization.sectorScienceMultiplier += 0.1f;
                        break;
                    case 4:
                        military.sectorScienceMultiplier += 0.1f;
                        break;
                    case 5:
                        science.sectorScienceMultiplier += 0.1f;
                        break;
                    case 6:
                        diplomacy.sectorScienceMultiplier += 0.1f;
                        break;
                }
                break;

            case "diplomacy":
                diplomaticCapacity++;
                break;
        }
    }

    void ExploreStar()
    {
        exploredStars++;
        RollStarOutcome();
    }

    private void RollStarOutcome()
    {
        int random = UnityEngine.Random.Range(1, 100);
        if (random < 34)
        {
            if (colonyShips > 0)
            {
                colonyShips--;
                colonizedPlanets++;
            }
            else
            {
                discoveredPlanets++;
            }
        }
        else if (random < 67)
        {
            //TODO - figure out if I want alien empires meeting alien empires and having their own trade and war
            if (isPlayer)
            {
                DiscoverAlienEmpire(this);
            }
            else
            {
                FindBonusResources();
            }
        }
        else if (random < 100)
        {
            FindBonusResources();
        }
    }

    private void FindBonusResources()
    {
        float treasureAmount = (grossEmpireProduct * MagicNumbers.Instance.treasurePortionOfGEP);
        bonusResourcesFromEvents += treasureAmount;
    }

    void DiscoverAlienEmpire(Empire discoveredByEmpire)
    {
        // TODO - record who discovered empire
        
        GameObject tempEmpireObject = Instantiate(GameManager.Instance.alienEmpire);
        Empire discoveredEmpire;
        discoveredEmpire = tempEmpireObject.GetComponent<Empire>();
        discoveredEmpire.discoveredBy = discoveredByEmpire.Name;
    }





    private void InitializeAlienEmpireDetails()
    {
        (string raceName, string raceAdjective, string raceHomeworld) = SyncronizedRaceDetailsGrabber(RandomNamesAndElements.Instance.raceNameGenerationList, RandomNamesAndElements.Instance.raceAdjectiveGenerationList, RandomNamesAndElements.Instance.raceHomeworldGenerationList);
        race.raceName = raceName;
        race.raceAdjective = raceAdjective;
        race.raceHomeworld = raceHomeworld;
        rulerName = ListSingleObjectGrabber(RandomNamesAndElements.Instance.emperorNameGenerationList);
        GenerateEmpireName();
        GenerateBiologyValues();

        for (int i = 0; i < (100 / MagicNumbers.Instance.allocationIterationAmount); i++)
        {
            // Determine how empire will allocate it's economy
            // TODO - tie this to randomly generated priorities
            AllocateEconomy();
        }
    }

    private void GenerateBiologyValues()
    {
        if (LogManager.Instance.logsEnabled)
        {
            if (LogManager.Instance.generateBiologyLogs)
            {
                Debug.Log($"Attempting to generate biological values for {Name}, in space year {GameManager.Instance.spaceYear}.");
            }
        }
        race.locomation = ListSingleObjectGrabber(RandomNamesAndElements.Instance.locomationGenerationList);
        race.typeOfRace = ListSingleObjectGrabber(RandomNamesAndElements.Instance.typeOfRaceGenerationList);
        race.numberOfAppendages = ListSingleObjectGrabber(RandomNamesAndElements.Instance.numberOfAppendagesGenerationList);
        race.typesOfAppendages = ListSingleObjectGrabber(RandomNamesAndElements.Instance.typesOfAppendagesGenerationList);
        race.eyeDetails = ListSingleObjectGrabber(RandomNamesAndElements.Instance.eyeDetailsGenerationList);
        race.externalCovering = ListSingleObjectGrabber(RandomNamesAndElements.Instance.externalCoveringGenerationList);
        race.societalUnit = ListSingleObjectGrabber(RandomNamesAndElements.Instance.societalUnitGenerationList);
        race.governmentTypes = ListSingleObjectGrabber(RandomNamesAndElements.Instance.governmentTypesGenerationList);
    }

    string ListSingleObjectGrabber(List<string> listName)
    {
        int randIndex = UnityEngine.Random.Range(1, listName.Count);
        return listName[randIndex];
    }

    (string raceName, string raceAdjective, string raceHomeworld) SyncronizedRaceDetailsGrabber(List<string> nameList, List<string> adjectiveList, List<string> homeworldList)
    {
        int randIndex = UnityEngine.Random.Range(1, nameList.Count);
        string raceName = nameList[randIndex];
        string raceAdjective = adjectiveList[randIndex];
        string raceHomeworld = homeworldList[randIndex];

        return (raceName, raceAdjective, raceHomeworld);
    }

    private void AllocateEconomy()
    {
        // Choose a Sector
        // Add 10 to that sector
        int allocation = UnityEngine.Random.Range(0, 6);
        if (LogManager.Instance.logsEnabled)
        {
            if (LogManager.Instance.alienAllocationLogs)
            {
                Debug.Log($"Allocating {MagicNumbers.Instance.allocationIterationAmount} percent of {Name} economy to sector {empireSectors[allocation].sectorName}.");
            }
        }
        empireSectors[allocation].fundingAllocation += MagicNumbers.Instance.allocationIterationAmount;
    }

    private void UpgradeSector(SectorDetails sector)
    {
        sector.growthLevelsAchieved += 1;
        sector.currentInvestment -= sector.neededInvestment;
        sector.neededInvestment *= MagicNumbers.Instance.upgradeCostMultiplier;
        if (LogManager.Instance.logsEnabled)
        {
            if (LogManager.Instance.sectorUpgradeLogs)
            {
                Debug.Log($"The {Name} upgraded {sector.sectorName}, to {sector.growthLevelsAchieved} in space year {GameManager.Instance.spaceYear}.");
            }
        }
    }

    private void GrowGEP()
    {
        float economyIncrease = (1.0f + (colonizedPlanets * MagicNumbers.Instance.planetGrossEmpireProductContribution));
        {
            if (LogManager.Instance.logsEnabled)
            {
                if (LogManager.Instance.growGEPLogs)
                {
                    Debug.Log($"Economy of {Name} increasing by {economyIncrease}, from {grossEmpireProduct}, in space year {GameManager.Instance.spaceYear}.");
                }
            }
        }
        grossEmpireProduct += economyIncrease;
    }

    private void CalculateProgressToSpaceyear()
    {
        for (int i = 1; i < GameManager.Instance.spaceYear; i++)
        {
            CalculateProgress();
        }
    }

    private void GenerateEmpireName()
    {
        boastWord = ListSingleObjectGrabber(RandomNamesAndElements.Instance.boastWordGenerationList);
        governmentWord = ListSingleObjectGrabber(RandomNamesAndElements.Instance.governmentWordGenerationList);
        Name = $"{boastWord} {race.raceAdjective} {governmentWord}";
    }

    private void UpdateEmpireAllocations()
    {
        economyAllocation = economy.fundingAllocation;
        explorationAllocation = exploration.fundingAllocation;
        colonizationAllocation = colonization.fundingAllocation;
        militaryAllocation = military.fundingAllocation;
        scienceAllocation = science.fundingAllocation;
        diplomacyAllocation = diplomacy.fundingAllocation;
    }

    private void CalculateExpectedBudgets()
    {
        expectedEconomyBudget = ((grossEmpireProduct * economyAllocation) / MagicNumbers.Instance.allocationPercentage);
        expectedExplorationBudget = ((grossEmpireProduct * explorationAllocation) / MagicNumbers.Instance.allocationPercentage);
        expectedColonizationBudget = ((grossEmpireProduct * colonizationAllocation) / MagicNumbers.Instance.allocationPercentage);
        expectedMilitaryBudget = ((grossEmpireProduct * militaryAllocation) / MagicNumbers.Instance.allocationPercentage);
        expectedScienceBudget = ((grossEmpireProduct * scienceAllocation) / MagicNumbers.Instance.allocationPercentage);
        expectedDiplomacyBudget = ((grossEmpireProduct * diplomacyAllocation) / MagicNumbers.Instance.allocationPercentage);
    }


}
