using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colonization : MonoBehaviour
{

    public SectorDetails sectorDetails;


    void Start()
    {
        sectorDetails.sectorName = "Colonization";
        sectorDetails.fundingAllocation = 0.0f;
        sectorDetails.growthLevelsAchieved = 0;
        sectorDetails.currentInvestment = 0;
        sectorDetails.neededInvestment = MagicNumbers.Instance.initialUpgradeCost;
        sectorDetails.sectorScienceMultiplier = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
