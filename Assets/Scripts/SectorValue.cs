using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorValue : MonoBehaviour
{
    public string sectorName;
    public float fundingAllocation;
    public int growthLevelsAchieved;
    public float currentInvestment;
    public float neededInvestment;
    public float sectorScienceMultiplier;
    public ScriptableSectorValues sectorValuesTemplate;


    void Start()
    {
        sectorName = sectorValuesTemplate.sectorName;
        fundingAllocation = sectorValuesTemplate.fundingAllocation;
        growthLevelsAchieved = sectorValuesTemplate.growthLevelsAchieved;
        currentInvestment = sectorValuesTemplate.currentInvestment;
        neededInvestment = sectorValuesTemplate.neededInvestment;
        sectorScienceMultiplier = sectorValuesTemplate.sectorScienceMultiplier;
    }
}
