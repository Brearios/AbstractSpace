using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicNumbers : MonoBehaviour
{
    public static MagicNumbers Instance;
    public int initialUpgradeCost;
    public int allocationIterationAmount = 5;
    public float allocationPercentage;
    public int numberOfAllocationSegments;
    public float upgradeCostMultiplier;
    public float planetGrossEmpireProductContribution; // How much an economic growth level is added to, per colonized planet
    public int totalCivilizations;
    public string PlayerEmpireName = "Human Space Alliance";
    public string PlayerAdjective = "Human";
    public string PlayerRuler = "Dwight Eisenhower";
    public int StartingGrossEmpireProduct = 10;
    public float treasurePortionOfGEP = 0.3f;
    public float fleetStrengthUpgradeMultiplier;
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
        allocationPercentage = (allocationIterationAmount / 100);
        numberOfAllocationSegments = (100 / allocationIterationAmount);
    }
}
