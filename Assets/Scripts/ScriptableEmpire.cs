using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Empire", menuName = "Empire")]

public class ScriptableEmpire : ScriptableObject
{
    public string Name;
    public string Adjective;
    public string rulerName;

    public SectorDetails economy;
    public SectorDetails exploration;
    public SectorDetails colonization;
    public SectorDetails military;
    public SectorDetails science;
    public SectorDetails diplomacy;
}