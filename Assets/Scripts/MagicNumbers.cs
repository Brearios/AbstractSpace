using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicNumbers : MonoBehaviour
{
    public static MagicNumbers Instance;
    public int initialUpgradeCost = 30; // Starting cost of the first upgrade
    public int allocationIterationAmount = 10;
    public float allocationPercentage;
    public int numberOfAllocationSegments;
    public float upgradeCostMultiplier = 1.07f; // How much the cost of upgrades increases each time you upgrade
    public float planetGrossEmpireProductContribution; // How much an economic growth level is added to, per colonized planet
    // public int totalCivilizations; Unused - could be used to code a maximum number of concurrent empires
    public string PlayerEmpireName = "Human Space Alliance";
    public string PlayerAdjective = "Human";
    public string PlayerRuler = "Dwight Eisenhower";
    public int startingEconomicUnits = 10; // Starting economic units for each empire
    public float startingEconomicOutputPerUnit = 1.0f; // Starting output per EU
    // public int StartingGrossEmpireProduct = startingEconomicUnits; // Changed from 10 to startingEconomicUnits to set up Colony Economy Scaling
            // Then I realized there's no need for this variable
    public int economicUnitsPerColony = 1; // EUs gained per colony
    public float economicOutputIncreasePerUpgrade = 0.1f; // How much economic output increases per EU per upgrade
    public int startingColonizedPlanets = 1;
    public int startingFleetStrength = 1;
    public int startingRelations = 100;
    
    public int allianceThreshold = 200;
    public int tradeThreshold = 150;
    public int peaceThreshold = 50;
    public int warThreshold = 10;
    public int perEmpireDiplomacySpendingPerYear = 15;
    public int explorationDiscoverEmpireChanceThreshold = 67;
    public int fleetHitPoints = 10; // Amount of damage taken to destroy one fleet strength
    public float tradeBonusMultiplier = 0.025f; // This is multiplied by your trading partner's GEP to determine how much you gain from trade
    public float allyFleetBonusMultiplier = 0.05f;
    public float treasureMinPortionOfGEP = 0.3f;
    public float treasureMaxPortionOfGEP = 0.3f;
    public int fleetStrengthMaximumAsMultipleOfMilitaryCapacity; // Currently starting at 25 - so if your military capacity is 4, your max fleet strength would be 100.
    public int exterminatorRelationsReduction;
    public int xenophobicRelationsReduction;
    public int moderateRelationsReduction;
    public int xenophilicRelationsReduction;
    public int exclusiveMaxShipDamageRoll = 6;
    public int inclusiveMinShipDamageRoll = 2;
    public float inclusiveMinColonizablePlanetOverlapInclusiveFloat = .2f;
    public float inclusiveMaxColonizablePlanetOverlapInclusiveFloat = .8f;

    // Not currently in use
    // REMOVE if not needed
    //public int fleetStrengthWinnerReduction; // starting at 9 - numerator of the fraction, with fleetStrengthMaximumAsMultipleOfMilitaryCapactity as denominator, for how much the winner keeps
    //public int fleetStrengthLoserReduction; // starting at 8 - numerator of the fraction, with fleetStrengthMaximumAsMultipleOfMilitaryCapactity as denominator, for how much the loser keeps

    public int strongFleetStrengthVictoryReduction; // starting at 10%
    public int strongFleetStrengthLossReduction; // starting at 25%
    public int weakFleetStrengthVictoryReduction; // starting at 15%
    public int weakFleetStrengthLossReduction; // starting at 25%
    public int fleetStrengthKillingBlowLevel; // starting at 5 - if a fleet is below 20% total capacity, and suffers a loss, that empire is defeated
    public int diplomacyModerateUnlockRank = 5; // science points in diplomacy to improve relations with moderate aliens
    public int diplomacyXenophobeUnlockRank = 10; // science points in diplomacy to improve relations with moderate aliens
    public int diplomacyExterminatorUnlockRank = 30; // science points in diplomacy to improve relations with moderate aliens
    public int riskThresholdToBeDiscovered = 5000; // roll number to determine if you are discovered by an alien race - should keep odds fairly low
    public int baseYearlyChanceToBeDiscovered = 22; // starting at 20 - represents yearly "risk" of being discovered by alien explorers
    public float colonizedPlanetDiscoveryRiskMultiplier = .1f; // multiple of base yearly chance for each planet colonized
    public float knownRacesDiscoveryRiskMultiplier = .1f; // multiple of base yearly chance for each race known
    public float warDiscoveryRiskMultiplier = .3f; // Wars are very impactful. Their effects invite investigation into their cause.






    // Needs a starting grossEmpireProduct

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        allocationPercentage = (100 / allocationIterationAmount);
        numberOfAllocationSegments = (100 / allocationIterationAmount);
    }
}
