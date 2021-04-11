using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO - add randomness in funding allocation approach - work in random size chunks, not single segments - possibly randomly determine approach
// TODO - add xenophobe/xenophile modifier - changing relations decay rate and costs

// TODO - build in properties for Human Space Alliance if (isPlayer)
// TODO - wars are unlikely to ever end if they're 1v1


public class Empire : MonoBehaviour
{
    public Race race;
    public bool AtWarWithDiscoveredBy;
    public bool AlliedWithPlayer;
    public bool isDefeated;
    public bool keepSpendingDiplomacy;
    public bool warInitiated;
    public bool tradingWithDiscoveredBy;
    public bool alliedWithDiscoveredBy;
    public string Name;
    public string Abbreviation;
    public string rulerName;
    public Empire discoveredBy;
    public string discoveredByName;
    public string boastWord;
    public string governmentWord;
    public string orientationString;
    public bool isPlayer;
    public bool defeatAnnounced;
    public int degreesFromPlayer;

    // 0 degrees - player
    // 1 degree - opponents - does everything players do
    // 2 degrees - enemies of opponents - pays the costs opponents pay, details are less necessary
    // 3 degrees - just get discovered and targeted, no diplomacy, exploration, or wars of their own

    public int empiresCurrentWars;
    // public enum SixDegrees { Player, Rival, Second, Third, Fourth, Fifth } Replaced with an int - player is 0, less code/functionality as int increases.
    public enum DiplomaticOrientation { Extermination, Xenophobic, Moderate, Xenophilic }
    // TODO Less calculation for those further "out", determine number of desired degrees, method of discovering/promoting known empries

    public ScriptableEmpire empireTemplate;

    public string currentStatus; // Allied, War, Defeated, Peace
    public string madlib;
    public string defeatedBy;

    public DiplomaticOrientation orientation;
    public DiplomaticOrientation diplomacyOrientation;

    public float grossEmpireProduct;
    public float bonusResourcesFromEventsAndTrade;
    public int exploredStars;
    public int discoveredPlanets;
    public int colonizedPlanets;
    public int colonyShips;
    public int militaryCapacity;
    public int fleetStrength;
    public int relationsTowardDiscoveredBy; // adapt to reducing relations with all known Empires
    // 1-100, with 1 being war, 2-34 being hostile, 35-65 being peace, 66-99 being trade, and 100 being allies

    public int yearlyDiplomaticCapacity; 
    public int totalDiplomaticCapacity; // Represents funding for diplomats, analysts, space anthropologists
    public int warDamageThisYear;

    public float economyAllocationAmount;
    public float explorationAllocationAmount;
    public float colonizationAllocationAmount;
    public float militaryAllocationAmount;
    public float scienceAllocationAmount;
    public float diplomacyAllocationAmount;

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
    public List<Empire> encounteredEmpires = new List<Empire>();
    public List<Empire> empiresAtWarWithThisEmpire = new List<Empire>();
    public List<Empire> tradePartnerEmpires = new List<Empire>();
    public List<Empire> alliedEmpires = new List<Empire>();
    private void Start()
    {
        economy.sectorName = "economy";
        exploration.sectorName = "exploration";
        colonization.sectorName = "colonization";
        military.sectorName = "military";
        science.sectorName = "science";
        diplomacy.sectorName = "diplomacy";
        isDefeated = false;

        if (isPlayer)
        {
            AtWarWithDiscoveredBy = false;
            AlliedWithPlayer = false;
            diplomacyOrientation = DiplomaticOrientation.Xenophilic;
            discoveredByName = "The Player.";
        }

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

        foreach (SectorDetails sector in empireSectors)
        {
            if (LogManager.Instance.logsEnabled)
            {
                if (LogManager.Instance.initializeFundingLogs)
                {
                    Debug.Log($"Initializing funding for {sector.sectorName}, in space year {GameManager.Instance.spaceYear}.");
                }
            }

            sector.growthLevelsAchieved = 0;
            sector.fundingAllocation = 0.0f;
            sector.currentInvestment = 0;
            sector.neededInvestment = MagicNumbers.Instance.initialUpgradeCost;
            sector.sectorScienceMultiplier = 0.0f;
        }

        // Temp allocations until allocating is set up
        if (isPlayer)
        {
            economy.fundingAllocation = 4.0f;
            exploration.fundingAllocation = 2.0f;
            colonization.fundingAllocation = 1.0f;
            military.fundingAllocation = 1.0f;
            science.fundingAllocation = 1.0f;
            diplomacy.fundingAllocation = 1.0f;
            race.raceName = "Humans";
            race.raceAdjective = "Human";
            race.raceHomeworld = "Sol";
            race.locomation = "bipedal";
            race.typeOfRace = "mammalian";
            race.numberOfAppendages = "two";
            race.typesOfAppendages = "hands";
            race.eyeDetails = "two eyes";
            race.externalCovering = "skin";
            race.societalUnit = "in cities";
            race.governmentType = "a democracy";
            degreesFromPlayer = 0;
            GameManager.Instance.allEmpires.Add(this);
        }

        InitializeEmpireAddSectorsAndSetGEP(this);
        if (!isPlayer)
        {
            InitializeAlienEmpireDetails(this);

            // TODO - can I do something with this to put the processing time behind the discovery notification?
            // Show notification
            // Loop to Spaceyear
            // !isRunning
            CalculateProgressToSpaceyear();
            // Handled on Discovery
            // GameManager.Instance.allEmpires.Add(this); 
            relationsTowardDiscoveredBy = MagicNumbers.Instance.startingRelations;
            encounteredEmpires.Add(discoveredBy);
            discoveredByName = discoveredBy.Name;
        }

        // Invicible Sakkran League, or ISL. The ISL is made up of ...
        // Abbreviation = $"{boastWord.Substring(0, 1)}{race.raceAdjective.Substring(0, 1)}{governmentWord.Substring(0, 1)}";

        // TODO - exclude humans from randomness
        // TODO - avoid previously generated races OR better, preserve their biological settings but not others.

        madlib = $"The {governmentWord} is made up of {race.raceName.ToLower()}, who originated on the planet {race.raceHomeworld}. Most {race.raceName.ToLower()} live {race.societalUnit}. \n \n" +
            $"{race.raceName} are {race.locomation}, and {race.typeOfRace} in appearance. {race.raceName} have {race.numberOfAppendages} {race.typesOfAppendages}. \n " +
            $"They see via {race.eyeDetails}, and have bodies covered by {race.externalCovering}. \n \n " +
            $" The {governmentWord} is {race.governmentType}, and their approach to other races is {orientationString.ToLower()}.";


        string compiledString = $"In {GameManager.Instance.spaceYear} ESE, your explorers made contact with aliens known as the {Name}. \n \n " +
            $"{madlib}\n \n " +
            $"Press Space to continue.";

        if ((!isPlayer) && (discoveredBy.isPlayer))
        {
            AddNotificationToList(compiledString);
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.isRunning)
        {
            return;
        }

        CheckForDefeat();

        if (isDefeated && defeatAnnounced)
        {
            return;
        }

        LoseFleetsFromLastYear();

        BenefitFromTrade();

        BenefitFromAlliances();

        CalculateProgress(true);

        UpdateEmpireAllocations();

        CalculateExpectedBudgets();

        ManageEmpireRelations();

        if (!isDefeated && warInitiated)
        {
            FightWars();
        }

        BuildShips();
    }

    private void InitializeEmpireAddSectorsAndSetGEP(Empire empire)
    {
        empire.grossEmpireProduct = MagicNumbers.Instance.StartingGrossEmpireProduct;
        empire.exploredStars = 0;
        empire.discoveredPlanets = 0;
        empire.colonizedPlanets = MagicNumbers.Instance.StartingColonizedPlanets;
        empire.militaryCapacity = MagicNumbers.Instance.StartingFleetStrength;
        empire.yearlyDiplomaticCapacity = 0;
    }

    void LoseFleetsFromLastYear()
    {
        fleetStrength -= warDamageThisYear;
        if ((fleetStrength <= 0) && (warInitiated))
        {
            defeatedBy = empiresAtWarWithThisEmpire[0].Name;
            isDefeated = true;
        }
        warDamageThisYear = 0;
    }

    void BenefitFromTrade()
    {
        foreach (Empire tradingPartner in tradePartnerEmpires)
        {
            float tradeBonus = (tradingPartner.grossEmpireProduct * MagicNumbers.Instance.tradeBonusMultiplier);
            bonusResourcesFromEventsAndTrade += tradeBonus;
            if (LogManager.Instance.tradeBenefitLogs)
            {
                Debug.Log($"Adding trade bonus for {Name}, of {tradeBonus}, from trade with {tradingPartner.Name} in space year {GameManager.Instance.spaceYear}.");
            }
        }
    }

    void BenefitFromAlliances()
    {
        foreach (Empire alliedEmpire in alliedEmpires)
        {
            // Current settings should grant 5% of what the empire you're allied would gain in a year - likely rounds down to zero. =\
            float allyFleetBonus = ((alliedEmpire.militaryCapacity * MagicNumbers.Instance.fleetStrengthMaximumAsMultipleOfMilitaryCapacity) * MagicNumbers.Instance.allyFleetBonusMultiplier);
            int allyFleetInt = (int)Math.Round(allyFleetBonus); // Rounds to nearest int
            fleetStrength += allyFleetInt; 
            if (LogManager.Instance.allyFleetBonusLogs)
            {
                Debug.Log($"Adding fleet bonus for {Name}, of {allyFleetInt}, due to being allied with {alliedEmpire.Name} in space year {GameManager.Instance.spaceYear}.");
            }
        }
    }


    void CalculateProgress(bool log)
    {
        if (LogManager.Instance.logsEnabled)
        {
            if (log)
            {
                if (LogManager.Instance.calculateProgressLogs)
                {
                    Debug.Log($"Calculating progress for {Name}, in space year {GameManager.Instance.spaceYear}.");
                }
            }
        }
        foreach (SectorDetails currentSector in empireSectors)
        {
            float iteratedInvestment = ((grossEmpireProduct * currentSector.fundingAllocation) / (100 / MagicNumbers.Instance.allocationIterationAmount));
            iteratedInvestment += ((bonusResourcesFromEventsAndTrade * currentSector.fundingAllocation) / (100 / MagicNumbers.Instance.allocationIterationAmount));
            iteratedInvestment += (iteratedInvestment * currentSector.sectorScienceMultiplier);
            currentSector.currentInvestment += iteratedInvestment;
            while (currentSector.currentInvestment > currentSector.neededInvestment)
            {
                UpgradeEmpire(currentSector);
                UpgradeSector(currentSector);
            }
        }

        // Remove Bonus Resources now spent
        bonusResourcesFromEventsAndTrade = 0;
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
                        Debug.Log($"Upgrading fleet - from {militaryCapacity} to {(militaryCapacity + 1)}.");
                    }
                }

                militaryCapacity++;
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
                yearlyDiplomaticCapacity++;
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

            // If Aliens discover aliens, especially in higher spaceyears, those aliens will discover aliens as they CalculateProgress.
            // This would only be advisable if it only applied to rivals, possibly with a method for promoting empires discovered by Rivals to rivals themselves
            //if (isPlayer)
            //{
            //    DiscoverAlienEmpire(this);
            //}
            if (degreesFromPlayer <= 1)
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
        bonusResourcesFromEventsAndTrade += treasureAmount;
        if (isPlayer)
        {
            string activity = ListSingleObjectGrabber(RandomNamesAndElements.Instance.explorationActivity);
            string finder = ListSingleObjectGrabber(RandomNamesAndElements.Instance.explorationActor);
            string finding = ListSingleObjectGrabber(RandomNamesAndElements.Instance.explorationFinding);
            string compiledString = $"While {activity}, {race.raceAdjective.ToLower()} {finder} found {finding} worth {treasureAmount.ToString("0.00")} quadracreds. \n" +
            "The full amount will be added to next year's economic allocations. \n \n" +
            "Press space to continue.";
            AddNotificationToList(compiledString);
        }
    }

    void DiscoverAlienEmpire(Empire discoveredByEmpire)
    {
        // TODO - record who discovered empire

        GameObject tempEmpireObject = Instantiate(GameManager.Instance.alienEmpire);
        Empire discoveredEmpire;
        discoveredEmpire = tempEmpireObject.GetComponent<Empire>();
        discoveredEmpire.discoveredBy = discoveredByEmpire;
        discoveredEmpire.degreesFromPlayer = (discoveredByEmpire.degreesFromPlayer +1);
        discoveredByEmpire.encounteredEmpires.Add(discoveredEmpire);
        GameManager.Instance.allEmpires.Add(discoveredEmpire);
        if (LogManager.Instance.logsEnabled)
        {
            if (LogManager.Instance.empireDiscovered)
            {
                Debug.Log($"An alien empire was discovered by {discoveredByEmpire} in {GameManager.Instance.spaceYear}.");
            }
        }
    }

    private void InitializeAlienEmpireDetails(Empire empire)
    {
        (string raceName, string raceAdjective, string raceHomeworld) = SyncronizedRaceDetailsGrabber(RandomNamesAndElements.Instance.raceNameGenerationList, RandomNamesAndElements.Instance.raceAdjectiveGenerationList, RandomNamesAndElements.Instance.raceHomeworldGenerationList);
        race.raceName = raceName;
        race.raceAdjective = raceAdjective;
        race.raceHomeworld = raceHomeworld;
        rulerName = ListSingleObjectGrabber(RandomNamesAndElements.Instance.emperorNameGenerationList);
        GenerateEmpireName(empire);
        GenerateBiologyValues(empire);

        // Generate a random number for the empire's approach to aliens 0 - exterminate, 1 - xenophobe, 2 - normal, 3 - xenophile
        int diploRand = UnityEngine.Random.Range(0, 4);
        orientation = (DiplomaticOrientation)diploRand;
        diplomacyOrientation = orientation;
        orientationString = orientation.ToString();

        // REMOVE - handled in Start script via foreach loop
        // SetSectorAllocationsToZero(empire);

        // Setting 1 allocation to each sector, so empires cannot get stuck with one not developing
        foreach (SectorDetails sector in empireSectors)
        {
            sector.fundingAllocation = 1;
        }

        for (int i = 6; i < (100 / MagicNumbers.Instance.allocationIterationAmount); i++)
        {
            // Determine how empire will allocate it's economy
            // TODO - tie this to randomly generated priorities
            if (LogManager.Instance.logsEnabled)
            {
                if (LogManager.Instance.trackAlienAllocationLoop)
                {
                    Debug.Log($"Allocating 1 iteration amount for {Name}. Allocation {i + 1} of {100 / MagicNumbers.Instance.allocationIterationAmount}. Iteration amount is set to {MagicNumbers.Instance.allocationIterationAmount}");
                }
            }
            AllocateEconomy(empire);
        }
    }

    private void SetSectorAllocationsToZero(Empire empire)
    {
        foreach (SectorDetails sectorToBeZeroed in empire.empireSectors)
        {
            sectorToBeZeroed.fundingAllocation = 0;
        }

    }

    private void GenerateBiologyValues(Empire empire)
    {
        if (LogManager.Instance.logsEnabled)
        {
            if (LogManager.Instance.generateBiologyLogs)
            {
                Debug.Log($"Attempting to generate biological values for {Name}, in space year {GameManager.Instance.spaceYear}.");
            }
        }
        empire.race.locomation = ListSingleObjectGrabber(RandomNamesAndElements.Instance.locomationGenerationList);
        empire.race.typeOfRace = ListSingleObjectGrabber(RandomNamesAndElements.Instance.typeOfRaceGenerationList);
        empire.race.numberOfAppendages = ListSingleObjectGrabber(RandomNamesAndElements.Instance.numberOfAppendagesGenerationList);
        empire.race.typesOfAppendages = ListSingleObjectGrabber(RandomNamesAndElements.Instance.typesOfAppendagesGenerationList);
        empire.race.eyeDetails = ListSingleObjectGrabber(RandomNamesAndElements.Instance.eyeDetailsGenerationList);
        empire.race.externalCovering = ListSingleObjectGrabber(RandomNamesAndElements.Instance.externalCoveringGenerationList);
        empire.race.societalUnit = ListSingleObjectGrabber(RandomNamesAndElements.Instance.societalUnitGenerationList);
        empire.race.governmentType = ListSingleObjectGrabber(RandomNamesAndElements.Instance.governmentTypesGenerationList);
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

    private void AllocateEconomy(Empire allocationEmpire)
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
        allocationEmpire.empireSectors[allocation].fundingAllocation += 1;
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
            if (LogManager.Instance.logsEnabled)
            {
                if (LogManager.Instance.alienEmpireCatchUpLogs)
                {
                    Debug.Log($"Calculating catch-up progress for {Name}, for past year {i + 1} in space year {GameManager.Instance.spaceYear}.");
                }
            }
            CalculateProgress(false);
        }
    }

    private void GenerateEmpireName(Empire empire)
    {
        empire.boastWord = ListSingleObjectGrabber(RandomNamesAndElements.Instance.boastWordGenerationList);
        empire.governmentWord = ListSingleObjectGrabber(RandomNamesAndElements.Instance.governmentWordGenerationList);
        empire.Name = $"{boastWord} {race.raceAdjective} {governmentWord}";
    }

    private void UpdateEmpireAllocations()
    {
        economyAllocationAmount = economy.fundingAllocation;
        explorationAllocationAmount = exploration.fundingAllocation;
        colonizationAllocationAmount = colonization.fundingAllocation;
        militaryAllocationAmount = military.fundingAllocation;
        scienceAllocationAmount = science.fundingAllocation;
        diplomacyAllocationAmount = diplomacy.fundingAllocation;
    }

    private void CalculateExpectedBudgets()
    {
        expectedEconomyBudget = ((grossEmpireProduct * economyAllocationAmount) / MagicNumbers.Instance.allocationPercentage);
        expectedExplorationBudget = ((grossEmpireProduct * explorationAllocationAmount) / MagicNumbers.Instance.allocationPercentage);
        expectedColonizationBudget = ((grossEmpireProduct * colonizationAllocationAmount) / MagicNumbers.Instance.allocationPercentage);
        expectedMilitaryBudget = ((grossEmpireProduct * militaryAllocationAmount) / MagicNumbers.Instance.allocationPercentage);
        expectedScienceBudget = ((grossEmpireProduct * scienceAllocationAmount) / MagicNumbers.Instance.allocationPercentage);
        expectedDiplomacyBudget = ((grossEmpireProduct * diplomacyAllocationAmount) / MagicNumbers.Instance.allocationPercentage);
    }

    void AddNotificationToList(string notificationText)
    {
        // Add to Notification List
        GameManager.Instance.notificationsToDisplay.Add(notificationText);
        // Display Notification is Handled by GameManager
    }

    void ManageEmpireRelations()
    {
        if (isPlayer)
        {
            DistributeDiplomacyPoints();
            // Species design - choose a xeno-relations level at a species design cost
            // Implement diplomacy damage based on that level to encounteredEmpires
        }
        if (!isPlayer && degreesFromPlayer == 1)
        {
            DistributeDiplomacyPoints();
            foreach (Empire reduceRelationsEmpire in encounteredEmpires)
            {
                ReduceRelationsByDiplomaticOrientation(reduceRelationsEmpire);
            }
            // Xenophobia Multiplier Goes Here
            if (((relationsTowardDiscoveredBy < 10) && (militaryCapacity > 0) && (fleetStrength > 5)) && !AtWarWithDiscoveredBy)
            {
                AtWarWithDiscoveredBy = true;
                warInitiated = true;
                discoveredBy.warInitiated = true;
                if (discoveredBy == GameManager.Instance.playerEmpire)
                {
                    GameManager.Instance.currentWars++;
                    discoveredBy.empiresAtWarWithThisEmpire.Add(this);
                    empiresAtWarWithThisEmpire.Add(discoveredBy);
                    string warNotification = $"The {Name} has declared war. Analysts estimate their fleet strength at {fleetStrength}. \n " +
                   $"Our fleet strength is {GameManager.Instance.playerEmpire.fleetStrength}. If we can bring down their fleets, we will triumph. \n" +
                   $"But if they bring our fleet strength to zero, there will be no future for the {GameManager.Instance.playerEmpire.Name}.";

                    // TODO - outcomes beyond extermination
                    // Gain colonies, perhaps on a reabsorb/rebuild timer
                    // Gain colonizable worlds
                    AddNotificationToList(warNotification);
                }
                else
                {
                    discoveredBy.empiresAtWarWithThisEmpire.Add(this);
                    empiresAtWarWithThisEmpire.Add(discoveredBy);
                    string nonPlayerWarNotifcation = $"The {Name} has declared war on {discoveredBy.Name}. Analysts estimate their fleet strength at {fleetStrength}. \n " +
                    $"The {discoveredBy.Name} has a fleet strength of {discoveredBy.fleetStrength}. The battles of the days to come will determine who continues to \n +" +
                    $"explore the stars, and which empire will be left to the pages of history.";
                    AddNotificationToList(nonPlayerWarNotifcation);
                }
                

                // TODO - what does the player need to know? Make it sound cool.
                // This is garbage and needs work
               
            }
            // Build a way to stop wars
            if ((relationsTowardDiscoveredBy >= MagicNumbers.Instance.tradeThreshold) && (!tradingWithDiscoveredBy))
            {
                tradePartnerEmpires.Add(discoveredBy);
                discoveredBy.tradePartnerEmpires.Add(this);
                if (LogManager.Instance.empireRelationsLogs)
                {
                    Debug.Log($"The {Name} and the {discoveredBy.Name} became trading partners in {GameManager.Instance.spaceYear}.");
                }
                tradingWithDiscoveredBy = true;
            }

            if ((relationsTowardDiscoveredBy >= MagicNumbers.Instance.allianceThreshold) && (!alliedWithDiscoveredBy))
            {
                alliedEmpires.Add(discoveredBy);
                discoveredBy.alliedEmpires.Add(this);
                if (LogManager.Instance.empireRelationsLogs)
                {
                    Debug.Log($"The {Name} and the {discoveredBy.Name} became allies in {GameManager.Instance.spaceYear}.");
                }
                alliedWithDiscoveredBy = true;
            }
        }
    }

    void ReduceRelationsByDiplomaticOrientation(Empire discoveredBy)
    {
        switch ((int)orientation)
        {
            // Exterminator
            case 0:
                relationsTowardDiscoveredBy -= MagicNumbers.Instance.exterminatorRelationsReduction;
                break;

            // Xenophobe
            case 1:
                relationsTowardDiscoveredBy -= MagicNumbers.Instance.xenophobicRelationsReduction;
                break;

            // Normal
            case 2:
                relationsTowardDiscoveredBy -= MagicNumbers.Instance.moderateRelationsReduction;
                break;

            // Xenophile
            case 3:
                relationsTowardDiscoveredBy -= MagicNumbers.Instance.xenophilicRelationsReduction;
                break;
        }
    }

    void DistributeDiplomacyPoints()
    {
        totalDiplomaticCapacity += yearlyDiplomaticCapacity;

        // If all empires can be looped through without keepSpendingDiplomacy being toggled, we're done after running the loop once. If not, we spend diplomacy until it reaches 0.
        keepSpendingDiplomacy = false;
        do
        {
            foreach (Empire relationsImprovedEmpire in encounteredEmpires)
            {
                if (totalDiplomaticCapacity > 0)
                {
                    ConsiderDiplomacySpending(relationsImprovedEmpire);
                }
                else
                {
                    return;
                }
            }
        }
        while ((keepSpendingDiplomacy) && (totalDiplomaticCapacity > 0));
    }

    void ConsiderDiplomacySpending(Empire relationsImprovedEmpire)
    {
        if (relationsImprovedEmpire.AtWarWithDiscoveredBy)
            {
                return;
                //TODO - enable investing diplomacy to end wars
            }
        else if (relationsImprovedEmpire.orientation == DiplomaticOrientation.Xenophilic)
        {
            totalDiplomaticCapacity--;
            relationsImprovedEmpire.relationsTowardDiscoveredBy++;
            keepSpendingDiplomacy = true;
        }
        else if ((relationsImprovedEmpire.orientation == DiplomaticOrientation.Moderate) && (GameManager.Instance.playerDiplomacyTowardModerates))
        {
            totalDiplomaticCapacity--;
            relationsImprovedEmpire.relationsTowardDiscoveredBy++;
            keepSpendingDiplomacy = true;
        }
        else if ((relationsImprovedEmpire.orientation == DiplomaticOrientation.Xenophobic) && (GameManager.Instance.playerDiplomacyTowardXenophobes))
        {
            totalDiplomaticCapacity--;
            relationsImprovedEmpire.relationsTowardDiscoveredBy++;
            keepSpendingDiplomacy = true;
        }
        else if ((relationsImprovedEmpire.orientation == DiplomaticOrientation.Extermination) && (GameManager.Instance.playerDiplomacyTowardExterminators))
        {
            totalDiplomaticCapacity--;
            relationsImprovedEmpire.relationsTowardDiscoveredBy++;
            keepSpendingDiplomacy = true;
        }
        else
        {
            return;
        }
    }

    void FightWars()
    {           
        int warDivisor = empiresAtWarWithThisEmpire.Count;
        int fleetStrengthPerWar = (fleetStrength / warDivisor);
        int shipDamageRoll = UnityEngine.Random.Range(1, 4);
        int damageDealtPerWar = ((fleetStrengthPerWar * shipDamageRoll) / MagicNumbers.Instance.fleetDamageInflictionDivisor);
        if (LogManager.Instance.logsEnabled)
        {
            if (LogManager.Instance.warLogsEnabled)
            {
                Debug.Log($"In {GameManager.Instance.spaceYear}, {Name}'s fleets will deal {damageDealtPerWar}, to each empire they are at war with.");
            }
        }

        foreach (Empire warEnemy in empiresAtWarWithThisEmpire)
        {
            warDamageThisYear += damageDealtPerWar;
        }

        // Reduce both side's fleetStrength by an amount each year, until it reaches zero
        // This should take into account who is stronger, and make the weaker lose more while they lose less
        // But a given year may have some randomness, as well.

        // Ideally, both wars and diplomacy need to happen among AI empires as well, not just between them and the player.

        // Each empire script does this, so there's no reason to loop.
        //foreach (Empire enemyEmpire in GameManager.Instance.knownEmpires)
        //{

        // Old war system, only has AI empires attack
        //if (isPlayer)
        //{
        //    return;
        //}

        //if (AtWarWithDiscoveredBy)
        //{
        //    int enemyFleetStrength = fleetStrength;
        //    int playerFleetStrength = GameManager.Instance.playerEmpire.fleetStrength;
        //    int weakerEmpireFleetStrength;
        //    Empire weakerEmpire;
        //    Empire strongerEmpire;
        //    int combinedFleetStrength = enemyFleetStrength + playerFleetStrength;
        //    if (enemyFleetStrength < playerFleetStrength)
        //    {
        //        weakerEmpireFleetStrength = enemyFleetStrength;
        //        weakerEmpire = this;
        //        strongerEmpire = GameManager.Instance.playerEmpire;
        //    }
        //    else
        //    {
        //        weakerEmpireFleetStrength = GameManager.Instance.playerEmpire.fleetStrength;
        //        weakerEmpire = GameManager.Instance.playerEmpire;
        //        strongerEmpire = this;
        //    }
        //    int roll = UnityEngine.Random.Range(1, (combinedFleetStrength + 1));
        //    if (roll > weakerEmpireFleetStrength)
        //    {
        //        // Weaker empire loses outright if under 20% strength

        //        // TODO - fix these if statements, and un-comment
        //        //if (weakerEmpire.fleetStrength < ((weakerEmpire.militaryCapacity * MagicNumbers.Instance.fleetStrengthMaximumAsMultipleOfMilitaryCapacity) / MagicNumbers.Instance.fleetStrengthKillingBlowLevel))
        //        //{
        //        //    weakerEmpire.isDefeated = true;
        //        //    weakerEmpire.defeatedBy = strongerEmpire.Name;
        //        //}
        //        // Weaker empire loses 30% of their fleet strength, stronger loses 10%
        //        weakerEmpire.fleetStrength = (((100 - MagicNumbers.Instance.weakFleetStrengthLossReduction) * weakerEmpire.fleetStrength) / 100);
        //        strongerEmpire.fleetStrength = (((100 - MagicNumbers.Instance.strongFleetStrengthVictoryReduction) * strongerEmpire.fleetStrength) / 100);
        //    }
        //    else
        //    {
        //        // Stronger empire loses outright if under 20% strength

        //        // TODO - fix these if statements, and un-comment
        //        //if (strongerEmpire.fleetStrength < ((strongerEmpire.militaryCapacity * MagicNumbers.Instance.fleetStrengthMaximumAsMultipleOfMilitaryCapacity) / MagicNumbers.Instance.fleetStrengthKillingBlowLevel))
        //        //{
        //        //    strongerEmpire.isDefeated = true;
        //        //    strongerEmpire.defeatedBy = weakerEmpire.Name;
        //        //}
        //        // Stronger empire loses 25% of their fleet strength, weaker loses 15%
        //        strongerEmpire.fleetStrength = (((100 - MagicNumbers.Instance.strongFleetStrengthLossReduction) * strongerEmpire.fleetStrength) / 100);
        //        weakerEmpire.fleetStrength = (((100 - MagicNumbers.Instance.weakFleetStrengthVictoryReduction) * weakerEmpire.fleetStrength) / 100);
        //    }

        //    if (weakerEmpire.fleetStrength < 1)
        //    {
        //        weakerEmpire.isDefeated = true;
        //        weakerEmpire.defeatedBy = strongerEmpire.Name;
        //    }

        //    if (strongerEmpire.fleetStrength < 1)
        //    {
        //        strongerEmpire.isDefeated = true;
        //        strongerEmpire.defeatedBy = weakerEmpire.Name;
        //    }


            // These never get to zero, so the war will not end
            // REMOVE if other system works well
            //if (enemyEmpire.fleetStrength > GameManager.Instance.playerEmpire.fleetStrength)
            //{
            //    // Multiply by 9 then divide by 10, to reduce by roughly 10%
            //    enemyEmpire.fleetStrength = ((enemyEmpire.fleetStrength * MagicNumbers.Instance.fleetStrengthWinnerReduction) / MagicNumbers.Instance.fleetStrengthMaximumAsMultipleOfMilitaryCapacity);
            //    // Multiply by 8 then divide by 10, to reduce by roughly 20%
            //    GameManager.Instance.playerEmpire.fleetStrength = ((GameManager.Instance.playerEmpire.fleetStrength * MagicNumbers.Instance.fleetStrengthLoserReduction) / MagicNumbers.Instance.fleetStrengthMaximumAsMultipleOfMilitaryCapacity);
            //}
            //else if (enemyEmpire.fleetStrength < GameManager.Instance.playerEmpire.fleetStrength)
            //{
            //    // Multiply by  then divide by 10, to reduce by roughly 10%
            //    GameManager.Instance.playerEmpire.fleetStrength = ((GameManager.Instance.playerEmpire.fleetStrength * MagicNumbers.Instance.fleetStrengthWinnerReduction) / MagicNumbers.Instance.fleetStrengthMaximumAsMultipleOfMilitaryCapacity);
            //    // Multiply by 8 then divide by 10, to reduce by roughly 20%
            //    enemyEmpire.fleetStrength = ((enemyEmpire.fleetStrength * MagicNumbers.Instance.fleetStrengthLoserReduction) / MagicNumbers.Instance.fleetStrengthMaximumAsMultipleOfMilitaryCapacity);
            //}
        // }
    }

    void BuildShips()
    {
        if (fleetStrength < (militaryCapacity * MagicNumbers.Instance.fleetStrengthMaximumAsMultipleOfMilitaryCapacity))
        {
            fleetStrength += militaryCapacity;
        }
    }

    void CheckForDefeat()
    {
        if (isDefeated == true)
        {
            if (isPlayer && !defeatAnnounced)
            {
                GameManager.Instance.playerLoss = true;
                string defeatNotifaction = $"In {GameManager.Instance.spaceYear} ESE, the {Name} was subjugated by the {defeatedBy}. \n " +
                    $"With their fleet in shambles, {race.raceHomeworld} was invaded, and {rulerName} was captured. \n \n" +
                    $"Now {race.raceAdjective} civilization will only live on in the history books. \n \n" +
                    $"You lose, Imperator.";

                defeatAnnounced = true;
                // TODO - random race leader words?

                // GameManager.Instance.isRunning = false; Notification will pause.
                AddNotificationToList(defeatNotifaction);
            }
            if (!isPlayer && !defeatAnnounced)
            {
                GameManager.Instance.currentWars--;
                GameManager.Instance.empiresAtWarWithPlayer.Remove(this);
                string victoryNotifaction = $"In {GameManager.Instance.spaceYear} ESE, the {Name} was subjugated by the {defeatedBy}. \n " +
                $"With their fleet in shambles, {race.raceHomeworld} was invaded, and {rulerName} was captured. \n \n" +
                $"Now, {race.raceAdjective} civilization will only live on in the history books.";
                // GameManager.Instance.isRunning = false; Notification will pause.
                AddNotificationToList(victoryNotifaction);

                defeatAnnounced = true;
            }
            foreach (Empire enemy in empiresAtWarWithThisEmpire)
            {
                enemy.empiresAtWarWithThisEmpire.Remove(this);
            }

        }
    }
}