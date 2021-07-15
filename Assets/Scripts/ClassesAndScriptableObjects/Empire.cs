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
    public bool initiatedContactWithPlayer;
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
    public Empire defeatedBy;

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
    public int unusedAllocations;

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
        economy.sectorName = "Economy";
        exploration.sectorName = "Exploration";
        colonization.sectorName = "Colonization";
        military.sectorName = "Military";
        science.sectorName = "Science";
        diplomacy.sectorName = "Diplomacy";
        isDefeated = false;

        if (isPlayer)
        {
            AtWarWithDiscoveredBy = false;
            AlliedWithPlayer = false;
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
            orientation = DiplomaticOrientation.Moderate;
            diplomacyOrientation = DiplomaticOrientation.Moderate;
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

        string compiledString;
        if (!initiatedContactWithPlayer)
        {
            compiledString = $"Your explorers have made contact with aliens known as the {Name}. \n \n " +
            $"{madlib}\n \n " +
            $"Press Space to continue.";
        }
        else
        {
            compiledString = $"Our empire has been contacted by representatives of the {Name}, who recently discovered us.. \n \n " +
            $"{madlib}\n \n " +
            $"Press Space to continue.";
        }
        if ((!isPlayer) && (discoveredBy.isPlayer))
        {
            AddNotificationToList(compiledString);
        }

        if (isPlayer)
        {
            DisplayInstructions();
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

        //if (!isPlayer)
        //{
        //    CheckForUnusedAllocations();
        //}

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

        ColonizePlanets();

        CalculateAndRollToBeDiscovered();
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
            defeatedBy = empiresAtWarWithThisEmpire[0];
            if (defeatedBy == GameManager.Instance.playerEmpire)
            {
                GameManager.Instance.playerDefeatedEmpires++;
            }
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
            // Adds 5% (modified in MagicNumbers) of ally's fleet strength to your fleet strength
            if (fleetStrength < (militaryCapacity * MagicNumbers.Instance.fleetStrengthMaximumAsMultipleOfMilitaryCapacity))
            {
                float allyFleetBonus = (alliedEmpire.fleetStrength * MagicNumbers.Instance.allyFleetBonusMultiplier);
                int allyFleetInt = (int)Math.Round(allyFleetBonus); // Rounds to nearest int
                fleetStrength += allyFleetInt;
                if (LogManager.Instance.allyFleetBonusLogs)
                {
                    Debug.Log($"Adding fleet bonus for {Name}, of {allyFleetInt}, due to being allied with {alliedEmpire.Name} in space year {GameManager.Instance.spaceYear}.");
                }
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
            float iteratedInvestment = ((grossEmpireProduct * currentSector.fundingAllocation) / (100 / MagicNumbers.Instance.allocationIterationAmount)); // Divides investment into pie slices based on allocations
            iteratedInvestment += ((bonusResourcesFromEventsAndTrade * currentSector.fundingAllocation) / (100 / MagicNumbers.Instance.allocationIterationAmount)); // Adds trade to investment slices based on allocations
            iteratedInvestment += (iteratedInvestment * currentSector.sectorScienceMultiplier); // Multiplies investment by the empire's science bonus
            currentSector.currentInvestment += iteratedInvestment; // Adds the invested amount
            while (currentSector.currentInvestment > currentSector.neededInvestment) // Determines if the investment is sufficient to upgrade
            {
                UpgradeEmpire(currentSector); // Explores a star, researches a tech, adds a fleet, etc.
                UpgradeSector(currentSector); // Upgrades growth level for sector, subtracts growth cost, increases costs for the next sector upgrade
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
            case "Economy":
                GrowGEP();
                break;

            case "Exploration":
                ExploreStar();
                break;

            case "Colonization":
                //TODO May want to eventually have individual planets with their own bonuses that roll here
                if (discoveredPlanets > 0)
                {
                    colonizedPlanets++;
                    discoveredPlanets--;
                    if (isPlayer)
                    {
                        string colonizedPlanetNotification = $"A {race.raceAdjective} colony ship has just landed on a new world. \n" +
                            $"Your empire's GEP will increase at a slightly faster rate from now on, as the population grows, \n " +
                            $"the colony develops infrastructure, and trade flourishes with your other worlds. \n \n" +
                            $"(Each economic upgrade going forward will be {MagicNumbers.Instance.planetGrossEmpireProductContribution} larger.)";
                        AddNotificationToList(colonizedPlanetNotification);
                    }
                }
                else
                {
                    colonyShips++;
                }

                break;

            case "Military":
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

            case "Science":
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
                        if (diplomacy.sectorScienceMultiplier == 5 || diplomacy.sectorScienceMultiplier == 10 || diplomacy.sectorScienceMultiplier == 30)
                        {
                            string diplomacyCongratulations = $"Recent scientific breakthroughs will allow {race.raceName} diplomats to reach out \n " +
                                "to less welcoming aliens and improve our relations with them. The galaxy became a more peaceful place today.";
                            AddNotificationToList(diplomacyCongratulations);
                        }
                        break;
                }
                break;

            case "Diplomacy":
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
                if (isPlayer)
                {
                    string planetDiscoveredButNoShips = $"{Name} science vessels have just located an unclaimed, habitable planet, ready to be colonized.  \n" +
                            $"Make sure some of your GEP is dedicated to colonization, as each upgrade there will prepare colonists and a ship for a new planet.";
                    AddNotificationToList(planetDiscoveredButNoShips);
                }
            }
        }
        else if (random < MagicNumbers.Instance.explorationDiscoverEmpireChanceThreshold)
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
                DiscoverAlienEmpireOrBeDiscovered(this, false);
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
        float treasureMultiplier = UnityEngine.Random.Range(MagicNumbers.Instance.treasureMinPortionOfGEP, MagicNumbers.Instance.treasureMaxPortionOfGEP);
        // Old system based on GEP - too small of rewards
        //float treasureAmount = (grossEmpireProduct * treasureMultiplier);
        float treasureAmount = (exploration.neededInvestment * treasureMultiplier);
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

    void DiscoverAlienEmpireOrBeDiscovered(Empire discoveredByEmpire, bool discoveredPlayer)
    {
        // TODO - record who discovered empire

        GameObject tempEmpireObject = Instantiate(GameManager.Instance.alienEmpire);
        Empire discoveredEmpire;
        discoveredEmpire = tempEmpireObject.GetComponent<Empire>();
        if (discoveredPlayer)
        {
            discoveredEmpire.initiatedContactWithPlayer = true;
        }
        discoveredEmpire.discoveredBy = discoveredByEmpire;
        discoveredEmpire.degreesFromPlayer = (discoveredByEmpire.degreesFromPlayer + 1);
        discoveredByEmpire.encounteredEmpires.Add(discoveredEmpire);
        GameManager.Instance.allEmpires.Add(discoveredEmpire);
        if (LogManager.Instance.logsEnabled)
        {
            if (LogManager.Instance.empireDiscovered)
            {
                Debug.Log($"An alien empire was discovered by {discoveredByEmpire.name} in {GameManager.Instance.spaceYear}.");
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
        // Add spaceYear ie 1980 Extra-Solar Era
        string spaceYear = $"E.S.E. {GameManager.Instance.spaceYear}:    ";
        // Add to Notification List
        string compiledNotification = spaceYear + notificationText;
        GameManager.Instance.notificationsToDisplay.Add(compiledNotification);
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
            // This would not be foreach, as each empire modifies their discoverer directly. Will need to be changed with Diplomacy 2.0.
            //foreach (Empire reduceRelationsEmpire in encounteredEmpires)

            if (!alliedWithDiscoveredBy)
            {
                ReduceRelationsByDiplomaticOrientation();
            }

            // Xenophobia Multiplier Goes Here
            if (((relationsTowardDiscoveredBy < MagicNumbers.Instance.warThreshold) && (militaryCapacity > 0) && (fleetStrength > 5)) && !AtWarWithDiscoveredBy)
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
            }
            // 50+ diplomacy can stop a war
            if ((relationsTowardDiscoveredBy >= MagicNumbers.Instance.peaceThreshold) && AtWarWithDiscoveredBy)
            {
                AtWarWithDiscoveredBy = false;
                warInitiated = false;
                discoveredBy.warInitiated = false;
                if (discoveredBy == GameManager.Instance.playerEmpire)
                {
                    GameManager.Instance.currentWars--;
                    discoveredBy.empiresAtWarWithThisEmpire.Remove(this);
                    empiresAtWarWithThisEmpire.Remove(discoveredBy);
                    string peaceNotification = $"Through hard diplomatic work, extensive communication, and better understanding, \n " +
                        $"we are now at peace with the {Name}. Hopefully it continues.";
                    AddNotificationToList(peaceNotification);
                }
                else
                {
                    discoveredBy.empiresAtWarWithThisEmpire.Remove(this);
                    empiresAtWarWithThisEmpire.Remove(discoveredBy);
                    string nonPlayerPeaceNotifcation = $"The {Name} and the {discoveredBy.Name} are no longer at war. For unknown reasons, the fighting has stopped.";
                    AddNotificationToList(nonPlayerPeaceNotifcation);
                }
            }

            // Multiplied by 1 to make sure the price doesn't drop to zero prior to planet colonization
            if ((relationsTowardDiscoveredBy >= (1 * MagicNumbers.Instance.tradeThreshold) * (1 + colonizedPlanets)) && (!tradingWithDiscoveredBy))
            {
                tradePartnerEmpires.Add(discoveredBy);
                discoveredBy.tradePartnerEmpires.Add(this);
                if (degreesFromPlayer == 1)
                {
                    string playerTradePartnerNotification = $"As our relations with the {Name} have improved, trade has begun between our empires. \n " +
                    $"This will result in greater economic growth for both empires.";
                    AddNotificationToList(playerTradePartnerNotification);
                }
                if (LogManager.Instance.empireRelationsLogs)
                {
                    Debug.Log($"The {Name} and the {discoveredBy.Name} became trading partners in {GameManager.Instance.spaceYear}.");
                }
                tradingWithDiscoveredBy = true;
            }
            // Multiplied by 1 to make sure the price doesn't drop to zero prior to planet colonization
            if ((relationsTowardDiscoveredBy > ( MagicNumbers.Instance.allianceThreshold) * (1 + colonizedPlanets)) && (!alliedWithDiscoveredBy))
            {
                alliedEmpires.Add(discoveredBy);
                discoveredBy.alliedEmpires.Add(this);
                if (degreesFromPlayer == 1)
                {
                    string playerAllyNotification = $"With deepening diplomatic and social ties, we have entered a military alliance with {Name}. \n " +
                    $"We will mutually defend each other, bolstering one another's fleet strength.";
                    AddNotificationToList(playerAllyNotification);
                }
                if (LogManager.Instance.empireRelationsLogs)
                {
                    Debug.Log($"The {Name} and the {discoveredBy.Name} became allies in {GameManager.Instance.spaceYear}.");
                }
                alliedWithDiscoveredBy = true;
            }
        }
    }

    void ReduceRelationsByDiplomaticOrientation()
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

        // We don't want to spend diplomacy forever, and we can't go from first contact to alliance in a year. So we stop when we're out of totalDiplomaticCapacity, but also after X loops per turn.
        if (totalDiplomaticCapacity > 0)
        {
            int i = 0;
            while ((i < MagicNumbers.Instance.perEmpireDiplomacySpendingPerYear) && (totalDiplomaticCapacity > 0))
            {
                i++;
                ConsiderDiplomacySpending();
            }
        }
    }

    void ConsiderDiplomacySpending()
    {
        foreach (Empire relationsImprovedEmpire in encounteredEmpires)
        {

            if (relationsImprovedEmpire.AtWarWithDiscoveredBy)
            {
                continue;
                //TODO - enable investing diplomacy to end wars
            }

            if (relationsImprovedEmpire == discoveredBy)
            {
                // Avoid investing in diplomacy toward your discoverer - until 2.0
                continue;
            }

            if (relationsImprovedEmpire.alliedWithDiscoveredBy)
            {
                continue;
            }

            else if (relationsImprovedEmpire.orientation == DiplomaticOrientation.Xenophilic)
            {
                CalculateCostAndSubtractDiplomacy(relationsImprovedEmpire);
            }

            else if ((relationsImprovedEmpire.orientation == DiplomaticOrientation.Moderate) && (GameManager.Instance.playerEmpire.diplomacy.sectorScienceMultiplier > MagicNumbers.Instance.diplomacyModerateUnlockRank))
            {
                CalculateCostAndSubtractDiplomacy(relationsImprovedEmpire);
            }

            else if ((relationsImprovedEmpire.orientation == DiplomaticOrientation.Xenophobic) && (GameManager.Instance.playerEmpire.diplomacy.sectorScienceMultiplier > MagicNumbers.Instance.diplomacyXenophobeUnlockRank))
            {
                CalculateCostAndSubtractDiplomacy(relationsImprovedEmpire);
            }

            else if ((relationsImprovedEmpire.orientation == DiplomaticOrientation.Extermination) && (GameManager.Instance.playerEmpire.diplomacy.sectorScienceMultiplier > MagicNumbers.Instance.diplomacyExterminatorUnlockRank))
            {
                CalculateCostAndSubtractDiplomacy(relationsImprovedEmpire);
            }
        }
    }

    void CalculateCostAndSubtractDiplomacy(Empire relationsImprovedEmpire)
    {
        // Because the friction of xenophobic relations is abstracted away through greater yearly subtraction, we can ignore it here
        int baseDiplomaticCost = relationsImprovedEmpire.colonizedPlanets;
        if (totalDiplomaticCapacity > baseDiplomaticCost)
        {
            totalDiplomaticCapacity -= baseDiplomaticCost;
            relationsImprovedEmpire.relationsTowardDiscoveredBy++;
            keepSpendingDiplomacy = true;
        }
    }

    void FightWars()
    {           
        int warDivisor = empiresAtWarWithThisEmpire.Count;
        int fleetStrengthPerWar = (fleetStrength / warDivisor);
        foreach (Empire warEnemy in empiresAtWarWithThisEmpire)
        {
            // Roll for damage
            int shipDamageRoll = UnityEngine.Random.Range(MagicNumbers.Instance.inclusiveMinShipDamageRoll, MagicNumbers.Instance.exclusiveMaxShipDamageRoll);
            int damageDealtToWarEnemy = (fleetStrengthPerWar * shipDamageRoll) / MagicNumbers.Instance.fleetHitPoints;
            warEnemy.warDamageThisYear += damageDealtToWarEnemy;
            if (LogManager.Instance.logsEnabled)
            {
                if (LogManager.Instance.warLogsEnabled)
                {
                    Debug.Log($"In {GameManager.Instance.spaceYear}, the {fleetStrength} {Name} fleets fighting on the {warEnemy.race.raceAdjective} front destroyed {damageDealtToWarEnemy} of the {warEnemy.fleetStrength} {warEnemy.Name} fleets.");
                }
            }
        }
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
                string defeatNotifaction = $"After {GameManager.Instance.spaceYear} years of interstellar history, the {Name} was subjugated by the {defeatedBy.Name}. \n " +
                    $"With their fleet in shambles, {race.raceHomeworld} was invaded, and {rulerName} was captured. \n \n" +
                    $"{race.raceAdjective} civilization will only live on in the history books. \n \n" +
                    $"You lose, Imperator.";
                warInitiated = false;

                defeatAnnounced = true;
                // TODO - random race leader words?

                // GameManager.Instance.isRunning = false; Notification will pause.
                AddNotificationToList(defeatNotifaction);
            }
            if (!isPlayer && !defeatAnnounced)
            {
                GameManager.Instance.currentWars--;
                GameManager.Instance.empiresAtWarWithPlayer.Remove(this);
                string victoryNotifaction = $"Ultimately, the {Name} was subjugated by the {defeatedBy.Name}. \n " +
                $"With their fleet in shambles, {race.raceHomeworld} was invaded, and {rulerName} was captured. \n \n" +
                $"Now, {race.raceAdjective} civilization will only live on in the history books.";
                AddNotificationToList(victoryNotifaction);
                warInitiated = false;
                defeatAnnounced = true;
                AllocatePlanets(this, defeatedBy);
            }
            foreach (Empire enemy in empiresAtWarWithThisEmpire)
            {
                enemy.empiresAtWarWithThisEmpire.Remove(this);
                if (enemy.empiresAtWarWithThisEmpire.Count == 0)
                {
                    enemy.warInitiated = false;
                }
            }

        }
    }

    //void CheckForUnusedAllocations()
    //{
    //    float assignedSectorAllocations = 0;
    //    foreach (SectorDetails sector in empireSectors)
    //    {
    //        assignedSectorAllocations += sector.fundingAllocation;
    //    }
    //    unusedAllocations = (MagicNumbers.Instance.numberOfAllocationSegments - (int)assignedSectorAllocations);
    //}

    void DisplayInstructions()
    {
        string instructionNotification = $"Welcome to Abstract Space! You are the Emperor of the {GameManager.Instance.playerEmpire.Name}. \n \n" +
            $"(Empire customization will be coming in a future build.) \n" +
            $"Your empire's success will depend on effective allocation of resources. \n" +
            $"You will determine, via the \"Allocate\" button, how much of the pie goes to economic growth, exploration, science, and so on. \n" +
            $"Time flows quickly in Abstract Space, but the game will be paused when you hit space, as well as when you're allocating resources. \n \n" +
            // TODO - make title customizable and apply to this
            $"You can adjust the default allocations if you like, or just see what happens. Best of luck, Emperor! \n \n" +
            $"(Press Space or click Resume to begin)";
        AddNotificationToList(instructionNotification);
    }

    void AllocatePlanets(Empire defeatedEmpire, Empire conqueringEmpire)
    {
        float colonizablePlanetOverlapRoll = UnityEngine.Random.Range(MagicNumbers.Instance.inclusiveMinColonizablePlanetOverlapInclusiveFloat, MagicNumbers.Instance.inclusiveMaxColonizablePlanetOverlapInclusiveFloat);
        int colonizablePlanetsThroughConquest = Convert.ToInt32(defeatedEmpire.colonizedPlanets * colonizablePlanetOverlapRoll);
        conqueringEmpire.discoveredPlanets += colonizablePlanetsThroughConquest;
        if (conqueringEmpire == GameManager.Instance.playerEmpire && (colonizablePlanetsThroughConquest > 0))
        {
            string availablePlanetsNotification = $"With the subjugation of the {defeatedEmpire.Name}, the {conqueringEmpire.Name} now has complete control of their space, \n" +
                $"including the {defeatedEmpire.colonizedPlanets} worlds they had colonized. {colonizablePlanetsThroughConquest} of these worlds are suitable for your people, \n" +
                $"and will be colonized as soon as your ships are ready.";
            AddNotificationToList(availablePlanetsNotification);
        }

        else if (conqueringEmpire == GameManager.Instance.playerEmpire)
        {
            string availablePlanetsNotification = $"With the subjugation of the {defeatedEmpire.Name}, the {conqueringEmpire.Name} now has complete control of their space, \n" +
                $"including the {defeatedEmpire.colonizedPlanets} worlds they had colonized. Unfortunately, none of these worlds are suitable for your people, \n" +
                $"but this war has bought the {conqueringEmpire.Name} peace on this front.";
            AddNotificationToList(availablePlanetsNotification);
        }
    }

    void ColonizePlanets()
    {
        if ((discoveredPlanets > 0) && (colonyShips > 0))
        {
            discoveredPlanets--;
            colonizedPlanets++;
            colonyShips--;

            if (isPlayer)
            {
                string colonizedPlanetNotification = $"A {race.raceAdjective} colony ship has just landed on a new world. \n" +
                    $"Your empire's GEP will increase at a slightly faster rate from now on, as the population grows, \n " +
                    $"the colony develops infrastructure, and trade flourishes with your other worlds. \n \n" +
                    $"(Each economic upgrade going forward will be {MagicNumbers.Instance.planetGrossEmpireProductContribution} larger.)";
                AddNotificationToList(colonizedPlanetNotification);
            }
        }
    }

    void CalculateAndRollToBeDiscovered()
    {
        float empireSizeRisk = (colonizedPlanets * MagicNumbers.Instance.colonizedPlanetDiscoveryRiskMultiplier);
        float galacticCommunicationsRisk = ((encounteredEmpires.Count - GameManager.Instance.playerDefeatedEmpires) * MagicNumbers.Instance.knownRacesDiscoveryRiskMultiplier);
        float activeWarsRisk = (empiresAtWarWithThisEmpire.Count * MagicNumbers.Instance.warDiscoveryRiskMultiplier);
        float discoveryChanceFloat = (MagicNumbers.Instance.baseYearlyChanceToBeDiscovered * (1.0f + empireSizeRisk + galacticCommunicationsRisk + activeWarsRisk));
        int discoveryChanceInt = (int)discoveryChanceFloat;
        int discoveryRoll = UnityEngine.Random.Range(1, MagicNumbers.Instance.riskThresholdToBeDiscovered);
        if (discoveryChanceInt > discoveryRoll)
        {
            DiscoverAlienEmpireOrBeDiscovered(this, true);
        }
        if (LogManager.Instance.logsEnabled)
        {
            if (LogManager.Instance.beDiscoveredRollLogsEnabled)
            {
                Debug.Log($"In {GameManager.Instance.spaceYear}, the sum discovery risk of the {Name} was {discoveryChanceInt} and the roll was {discoveryRoll}");
            }
        }
    }
}