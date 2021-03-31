using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    public static LogManager Instance;
    public bool logsEnabled;
    public bool sectorUpgradeLogs;
    public bool empireUpgradeLogs;
    public bool processingEmpiresLogs;
    public bool calculateProgressLogs;
    public bool fleetUpgradeLogs;
    public bool growGEPLogs;
    public bool alienAllocationLogs;
    public bool initializeFundingLogs;
    public bool addSectorsToListLogs;
    public bool generateBiologyLogs;
    public bool trackAlienAllocationLoop;
    public bool alienEmpireCatchUpLogs;
    public bool empireDiscovered;
    public bool tradeBenefitLogs;
    public bool allyFleetBonusLogs;
    public bool empireRelationsLogs;
    public bool warLogsEnabled;

    //public bool displaySectorUpgradeLogs;
    //public bool displaySectorUpgradeLogs;
    //public bool displaySectorUpgradeLogs;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
}
