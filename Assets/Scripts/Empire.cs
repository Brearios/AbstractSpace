using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empire : MonoBehaviour
{
    public Race race;
    public string Name;
    public string Adjective;
    public string rulerName;
    public int grossEmpireProduct;
    public int discoveredPlanets;
    public int colonizedPlanets;
    public SectorValues economy;
    public SectorValues exploration;
    public SectorValues colonization;
    public SectorValues military;
    public SectorValues science;
    public SectorValues diplomacy;
    public List<SectorValues> empireSectors = new List<SectorValues>();
    

    // Start is called before the first frame update
    void Start()
    {
        empireSectors.Add(economy);
        empireSectors.Add(exploration);
        empireSectors.Add(colonization);
        empireSectors.Add(military);
        empireSectors.Add(science);
        empireSectors.Add(diplomacy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
