using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sector Value", menuName = "Sector Value")]

public class ScriptableSectorValues : ScriptableObject
{
    public string sectorName;
    public int fundingAllocation;
    public int growthLevelsAchieved;
    public float currentInvestment;
    public float neededInvestment;
    public float sectorScienceMultiplier;
}
