using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicNumbers : MonoBehaviour
{
    public static MagicNumbers Instance;
    public int initialUpgradeCost;
    public int allocationIterationAmount = 5;
    public float upgradeCostMultiplier;
    public float planetGrossEmpireProductContribution; // How much an economic growth level is added to, per colonized planet
    public int totalCivilizations;
    public string PlayerEmpireName = "Human Space Alliance";
    public string PlayerAdjective = "Human";
    public string PlayerRuler = "Dwight Eisenhower";

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
    }
}
