using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empire : MonoBehaviour
{
    public Race race;
    public bool AtWarWithPlayer;
    public bool AlliedWithPlayer;
    public string Name;
    public bool isPlayer;

    public ScriptableEmpire empireTemplate;

    // Removed until I see how the paragraphs play out
    // public string Adjective;

    public string rulerName;
    public float grossEmpireProduct;
    public float bonusResourcesFromEvents;
    public int exploredStars;
    public int discoveredPlanets;
    public int colonizedPlanets;
    public int colonyShips;
    public int fleetStrength;
    public int relationsTowardPlayer;
    public int diplomaticCapacity; // Represents diplomats, analysts, space anthropologists
    // 1-100, with 1 being war, 2-34 being hostile, 35-65 being peace, 66-99 being trade, and 100 being allies
    public float economyAllocation;
    public float explorationAllocation;
    public float colonizationAllocation;
    public float militaryAllocation;
    public float scienceAllocation;
    public float diplomacyAllocation;

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
        if (isPlayer)
        {
            if (LogManager.Instance.logsEnabled)
            {
                if (LogManager.Instance.addSectorsToList)
                {
                    Debug.Log($"Attempting to add 6 sectors to {Name}'s empireSectors list, in space year {GameManager.Instance.spaceYear}.");
                }
            }

            empireSectors.Add(economy);
            empireSectors.Add(exploration);
            empireSectors.Add(colonization);
            empireSectors.Add(military);
            empireSectors.Add(science);
            empireSectors.Add(diplomacy);

            if (LogManager.Instance.logsEnabled)
            {
                if (LogManager.Instance.addSectorsToList)
                {
                    Debug.Log($"Added {empireSectors.Count} sectors to {Name}'s empireSectors list, in space year {GameManager.Instance.spaceYear}.");
                }
            }
        }
    }

    private void Update()
    {
    economyAllocation = economy.fundingAllocation;
    explorationAllocation = exploration.fundingAllocation;
    colonizationAllocation = colonization.fundingAllocation;
    militaryAllocation = military.fundingAllocation;
    scienceAllocation = science.fundingAllocation;
    diplomacyAllocation = diplomacy.fundingAllocation;
}
}
