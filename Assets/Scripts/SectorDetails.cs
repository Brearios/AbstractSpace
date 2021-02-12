using UnityEngine;

[System.Serializable]

public class SectorDetails

{

    public string sectorName;
    public float fundingAllocation;
    public int growthLevelsAchieved;
    public float currentInvestment;
    public float neededInvestment;
    public float sectorScienceMultiplier;
    
    // REMOVE - Shouldn't need this anymore, initialized in Start of the parent
    // public ScriptableSectorValues sectorValuesTemplate;

    //void Start()
    //{
    //    sectorName = sectorValuesTemplate.sectorName;
    //    if (LogManager.Instance.logsEnabled)
    //    {
    //        if (LogManager.Instance.initializeFundingLogs)
    //        {
    //            Debug.Log($"Initializing funding for {sectorName}, in space year {GameManager.Instance.spaceYear}.");
    //        }
    //    }
    //    fundingAllocation = sectorValuesTemplate.fundingAllocation;
    //    growthLevelsAchieved = sectorValuesTemplate.growthLevelsAchieved;
    //    currentInvestment = sectorValuesTemplate.currentInvestment;
    //    neededInvestment = sectorValuesTemplate.neededInvestment;
    //    sectorScienceMultiplier = sectorValuesTemplate.sectorScienceMultiplier;
    //}
}
