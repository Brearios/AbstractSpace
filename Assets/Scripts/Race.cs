using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Race : MonoBehaviour
{
    public string[] raceNameGenerationList = { "Alkari", "Bulrathi", "Darloks", "Humans", "Klackons", "Meklar", "Mrrshan", "Psilon", "Sakkra", "Silicoids", "Elerians", "Gnolams", "Trilarians" };
    public string raceName;
    public string[] raceHomeworldGenerationList = { "Altair", "Ursa", "Nazin", "Sol", "Kholdan", "Meklon", "Fieras", "Menta", "Sssla", "Cryslon", "Berylia", "Gnol", "Wavya" };
    public string raceHomeworld;
    public string[] locomationGenerationList = { "bipedal", "tripedal", "quadrupedal", "floating", "flying", "serpentine" };
    public string locomation;
    public string[] typeOfRaceGenerationList = { "mammalian", "amphibian", "reptilian", "avian", "insectoid", "aquatic", "fungoid", "plantoid", "robotic" };
    public string typeOfRace;
    public string[] numberOfAppendagesGenerationList = { "two", "three", "four", "six", "many" };
    public string numberOfAppendages;
    public string[] typesOfAppendagesGenerationList = { "hands", "claws", "wings", "tentacles" };
    public string typesOfAppendages;
    public string[] eyeDetailsGenerationList = { "one eye", "two eyes", "three eyes", "many eyes", "compound eyes" };
    public string eyeDetails;
    public string[] externalCoveringGenerationList = { "an exoskeleton", "fur", "scales", "skin" };
    public string externalCovering;
    public string[] societalUnitGenerationList = { "in solitary isolation", "with immediate family", "with extended family groups", "in large tribes", "in cities", "in tightly-packed megacities"};
    public string societalUnit;
    public string[] governmentTypesGenerationList = { "is a democracy", "is a dictatorship", "is a technocracy", "is a oligarchy", "is a monarchy", "is a theocracy", "is non-existent - anarchy" };
    public string governmentTypes;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
