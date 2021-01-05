using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Race : MonoBehaviour
{
    public string raceName;
    public string raceHomeworld;
    public string[] locomation = { "bipedal", "tripedal", "quadrupedal", "floating", "flying", "serpentine" };
    public string[] typeOfRace = { "mammalian", "amphibian", "reptilian", "avian", "insectoid", "aquatic", "fungoid", "plantoid", "robotic" };
    public int numberOfAppendages;
    public string[] typesOfAppendages = { "hands", "claws", "wings", "tentacles" };
    public string[] eyeDetails = { "one eye", "two eyes", "three eyes", "many eyes", "compound eyes" };
    public string[] externalCovering = { "an exoskeleton", "fur", "scales", "skin" };
    public string[] societalUnit = { "in solitary isolation", "with immediate family", "with extended family groups", "in large tribes", "in cities", "in tightly-packed megacities"};
    public string[] governmentTypes = { "is a democracy", "is a dictatorship", "is a technocracy", "is a oligarchy", "is a monarchy", "is a theocracy", "is non-existent - anarchy" };
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
