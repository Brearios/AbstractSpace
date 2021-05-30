using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScripts : MonoBehaviour
{
    //TODO - why can't I reference a specific sector instead of repeating myself? Use a list?
    //TODO - better UI would be for each to have a +/- button, and a reset button - for later
    public void ReduceAllocation()
    {
        if (GameManager.Instance.currentSector.fundingAllocation > 0)
        {
            GameManager.Instance.currentSector.fundingAllocation--;
            GameManager.Instance.numberOfAllocationsAvailable++;
        }
    }
    public void IncreaseAllocation()
    {
        CheckAvailableAllocations();
        if (GameManager.Instance.allocationsAvailable = true)
        {
            GameManager.Instance.currentSector.fundingAllocation++;
            GameManager.Instance.numberOfAllocationsAvailable--;
        }
    }

    public void SetEconomyAsCurrentSector()
    {
        GameManager.Instance.currentSector = GameManager.Instance.playerEmpire.economy;
    }

    public void SetExplorationAsCurrentSector()
    {
        GameManager.Instance.currentSector = GameManager.Instance.playerEmpire.exploration;
    }

    public void SetColonizationAsCurrentSector()
    {
        GameManager.Instance.currentSector = GameManager.Instance.playerEmpire.colonization;
    }

    public void SetMilitaryAsCurrentSector()
    {
        GameManager.Instance.currentSector = GameManager.Instance.playerEmpire.military;
    }

    public void SetScienceAsCurrentSector()
    {
        GameManager.Instance.currentSector = GameManager.Instance.playerEmpire.science;
    }

    public void SetDiplomacyAsCurrentSector()
    {
        GameManager.Instance.currentSector = GameManager.Instance.playerEmpire.diplomacy;
    }

    public void isRunningToggle()
    {
        GameManager.Instance.isRunning = !GameManager.Instance.isRunning;
    }

    public void CheckAvailableAllocations()
    {
        float totalSectorAllocations = 0;
        foreach (SectorDetails sector in GameManager.Instance.playerEmpire.empireSectors)
        {
            totalSectorAllocations += sector.fundingAllocation;
        }
        if (totalSectorAllocations >= (100 / MagicNumbers.Instance.allocationIterationAmount))
        {
            GameManager.Instance.allocationsAvailable = false;
        }
        else
        {
            GameManager.Instance.allocationsAvailable = true;
        }

    }
}
