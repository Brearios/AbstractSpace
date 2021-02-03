using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empire : MonoBehaviour
{
    public Race race;
    public bool AtWarWithPlayer;
    public bool AlliedWithPlayer;
    public string Name;

    public ScriptableEmpire empireTemplate;

    // Removed until I see how the paragraphs play out
    // public string Adjective;

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
    public SectorDetails economy;
    public SectorDetails exploration;
    public SectorDetails colonization;
    public SectorDetails military;
    public SectorDetails science;
    public SectorDetails diplomacy;
    // Potentially could add population numbers, to be factored into grossEmpireProduct, with a sector for spending that would increase growth rate
    public List<SectorDetails> empireSectors = new List<SectorDetails>();

    private void Start()
    {
        empireSectors.Add(economy);
        empireSectors.Add(exploration);
        empireSectors.Add(colonization);
        empireSectors.Add(military);
        empireSectors.Add(science);
        empireSectors.Add(diplomacy);
    }

    //public Empire(Race raceDetails, string empName, string empAdjective, string ruler)
    //{
    //    race = raceDetails;
    //    Name = empName;
    //    Adjective = empAdjective;
    //    rulerName = ruler;
    //    grossEmpireProduct = MagicNumbers.Instance.StartingGrossEmpireProduct;
    //}
}
