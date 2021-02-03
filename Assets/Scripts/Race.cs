using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Race : MonoBehaviour
{
    public string raceName;
    public string raceAdjective;
    public string raceHomeworld;
    public string locomation;
    public string typeOfRace;
    public string numberOfAppendages;
    public string typesOfAppendages;
    public string eyeDetails;
    public string externalCovering;
    public string societalUnit;
    public string governmentTypes;
    public string currentStatus; // Allied, War, Defeated, Peace

    public ScriptableRace raceTemplate;

    private void Start()
    {
        raceName = raceTemplate.raceName;
        raceHomeworld = raceTemplate.raceHomeworld;
        locomation = raceTemplate.locomation;
        typeOfRace = raceTemplate.typeOfRace;
        numberOfAppendages = raceTemplate.numberOfAppendages;
        typesOfAppendages = raceTemplate.typesOfAppendages;
        eyeDetails = raceTemplate.eyeDetails;
        externalCovering = raceTemplate.externalCovering;
        societalUnit = raceTemplate.societalUnit;
        governmentTypes = raceTemplate.governmentTypes;
    }
}
