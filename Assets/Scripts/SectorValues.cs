using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sector Value", menuName = "Sector Value")]

public class SectorValues : ScriptableObject
{
    public string sectorName;
    public float fundingAllocation;
    public int growthLevelsAchieved;
    public float currentInvestment;
    public float neededInvestment;
    public float sectorScienceMultiplier;
}
