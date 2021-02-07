using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNamesAndElements : MonoBehaviour
{
    public static RandomNamesAndElements Instance;

    public List<string> emperorNameGenerationList = new List<string>{ "Zekruk", "Qhakun", "Mies'ins", "Yulgaers", "Ghekrets", "Serea", "Ilgik", "Tazil", "Bas'ael", "Vadhuh", "Evroks", "Xungits", "Khicrets" };
    public List<string> raceNameGenerationList = new List<string>{ "Alkari", "Bulrathi", "Darloks", "Humans", "Klackons", "Meklar", "Mrrshan", "Psilon", "Sakkra", "Silicoids", "Elerians", "Gnolams", "Trilarians" };
    public List<string> raceAdjectiveGenerationList = new List<string>{ "Alkaran", "Bulrath", "Darlokian", "Human", "Klackon", "Meklar", "Mrrshan", "Psilon", "Sakkran", "Siliconian", "Elerian", "Gnolan", "Trilarian" };
    public List<string> raceHomeworldGenerationList = new List<string>{ "Altair", "Ursa", "Nazin", "Sol", "Kholdan", "Meklon", "Fieras", "Menta", "Sssla", "Cryslon", "Berylia", "Gnol", "Wavya" };
    public List<string> locomationGenerationList = new List<string>{ "bipedal", "tripedal", "quadrupedal", "floating", "flying", "serpentine" };
    public List<string> typeOfRaceGenerationList = new List<string>{ "mammalian", "amphibian", "reptilian", "avian", "insectoid", "aquatic", "fungoid", "plantoid", "robotic" };
    public List<string> numberOfAppendagesGenerationList = new List<string>{ "two", "three", "four", "six", "many" };
    public List<string> typesOfAppendagesGenerationList = new List<string>{ "hands", "claws", "clawed hands", "wings", "tentacles" };
    public List<string> eyeDetailsGenerationList = new List<string>{ "one eye", "two eyes", "three eyes", "many eyes", "compound eyes" };
    public List<string> externalCoveringGenerationList = new List<string>{ "an exoskeleton", "fur", "scales", "skin" };
    public List<string> societalUnitGenerationList = new List<string>{ "in solitary isolation", "with immediate family", "with extended family groups", "in large tribes", "in cities", "in tightly-packed megacities" };
    public List<string> governmentTypesGenerationList = new List<string>{ "is a democracy", "is a dictatorship", "is a technocracy", "is a oligarchy", "is a monarchy", "is a theocracy", "is non-existent - anarchy" };
    public List<string> boastWordGenerationList = new List<string> { "Great", "Indomintable", "Invincible", "Galactic", "Eternal" };
    public List<string> governmentWordGenerationList = new List<string> { "Federaion", "Alliance", "Confederacy", "League", "Coalition", "Union", "Consortium", "High", "Grand" };
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
}

