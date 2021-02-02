using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Empire", menuName = "Empire")]

public class Empire : ScriptableObject
{
    public Race race;
    public bool AtWarWithPlayer;
    public bool AlliedWithPlayer;
    public string Name;
    public string Adjective;
    public string rulerName;
    public float grossEmpireProduct;
    public float bonusResourcesFromEvents;
    public int exploredStars;
    public int discoveredPlanets;
    public int colonizedPlanets;
    public int fleetStrength;
    public int relationsTowardPlayer;
    public int diplomaticCapacity; // Represents diplomats, analysts, space anthropologists
    // 1-100, with 1 being war, 2-34 being hostile, 35-65 being peace, 66-99 being trade, and 100 being allies
    public SectorValues economy;
    public SectorValues exploration;
    public SectorValues colonization;
    public SectorValues military;
    public SectorValues science;
    public SectorValues diplomacy;
    // Potentially could add population numbers, to be factored into grossEmpireProduct, with a sector for spending that would increase growth rate
    public List<SectorValues> empireSectors = new List<SectorValues>();

    public Empire(Race raceDetails, string empName, string empAdjective, string ruler)
    {
        race = raceDetails;
        Name = empName;
        Adjective = empAdjective;
        rulerName = ruler;
        grossEmpireProduct = MagicNumbers.Instance.StartingGrossEmpireProduct;
    }
}
