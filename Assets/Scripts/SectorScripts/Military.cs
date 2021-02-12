using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Military : MonoBehaviour
{
    public SectorDetails sectorDetails;

    // Start is called before the first frame update
    void Start()
    {
        sectorDetails.sectorName = "Military";
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
