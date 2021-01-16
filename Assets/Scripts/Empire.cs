using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empire : MonoBehaviour
{
    public Race race;
    public string Name;
    public string Adjective;
    public string rulerName;
    public float grossEmpireProduct;
    public int exploredStars;
    public int discoveredPlanets;
    public int colonizedPlanets;
    public int fleetStrength;
    public SectorValues economy;
    public SectorValues exploration;
    public SectorValues colonization;
    public SectorValues military;
    public SectorValues science;
    public SectorValues diplomacy;
    // Potentially could add population numbers, to be factored into grossEmpireProduct, with a sector for spending that would increase growth rate
    public List<SectorValues> empireSectors = new List<SectorValues>();
    public float economyScienceMultiplier;
    public float explorationScienceMultiplier;
    public float colonizationScienceMultiplier;
    public float militaryScienceMultiplier;
    public float scienceScienceMultiplier;
    public float diplomacyScienceMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        empireSectors.Add(economy);
        empireSectors.Add(exploration);
        empireSectors.Add(colonization);
        empireSectors.Add(military);
        empireSectors.Add(science);
        empireSectors.Add(diplomacy);

    economyScienceMultiplier = 0.0f;
    explorationScienceMultiplier = 0.0f;
    colonizationScienceMultiplier = 0.0f;
    militaryScienceMultiplier = 0.0f;
    scienceScienceMultiplier = 0.0f;
    diplomacyScienceMultiplier = 0.0f;
}

    // Update is called once per frame
    void Update()
    {
        
    }
}
