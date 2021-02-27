using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxController : MonoBehaviour
{
    public Text overviewText;
    public Text expansionText;
    public Text scienceText;
    public Text spaceYearText;
    public Text allocationText;
    public Text militaryText;
    public Text diplomacyText;
    public Text selectedSectorText;
    public Text notificationText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        overviewText.text = $"Empire Stats: \n " +
            $"\n Economy: {GameManager.Instance.playerEmpire.economy.growthLevelsAchieved}" +
            $"\n Exploration: {GameManager.Instance.playerEmpire.exploration.growthLevelsAchieved} " +
            $"\n Colonization: {GameManager.Instance.playerEmpire.colonization.growthLevelsAchieved}" +
            $"\n Military: {GameManager.Instance.playerEmpire.military.growthLevelsAchieved}" +
            $"\n Science: {GameManager.Instance.playerEmpire.science.growthLevelsAchieved}" +
            $"\n Diplomacy: {GameManager.Instance.playerEmpire.diplomacy.growthLevelsAchieved}";

        expansionText.text = $"Explored Stars: {GameManager.Instance.playerEmpire.exploredStars} \n \n" +
            $"Colony Candidates: {GameManager.Instance.playerEmpire.discoveredPlanets} \n \n" +
            $"Colony Ships on Standby: {GameManager.Instance.playerEmpire.colonyShips} \n \n" +
            $"Colonies: {GameManager.Instance.playerEmpire.colonizedPlanets}";

        scienceText.text = $"Science Level: {GameManager.Instance.playerEmpire.science.growthLevelsAchieved} \n \n" +
            $"Sector Multiplers \n \n" +
            $"Economy: {GameManager.Instance.playerEmpire.economy.sectorScienceMultiplier.ToString("0.0")} \n" +
            $"Exploration: {GameManager.Instance.playerEmpire.exploration.sectorScienceMultiplier.ToString("0.0")} \n" +
            $"Colonization: {GameManager.Instance.playerEmpire.colonization.sectorScienceMultiplier.ToString("0.0")} \n" +
            $"Military: {GameManager.Instance.playerEmpire.military.sectorScienceMultiplier.ToString("0.0")} \n" +
            $"Science: {GameManager.Instance.playerEmpire.science.sectorScienceMultiplier.ToString("0.0")} \n" +
            $"Diplomacy: {GameManager.Instance.playerEmpire.diplomacy.sectorScienceMultiplier.ToString("0.0")}";

        spaceYearText.text = $"{GameManager.Instance.spaceYear} ESE \n" +
            "(Extra-Solar Era)";

        allocationText.text = $"GEP: ${GameManager.Instance.playerEmpire.grossEmpireProduct.ToString("0.00")} Quadracreds \n \n" +
            // TODO  $"Added by Trade: {variableForTradingGEP&AllyGEP} \n \n"
            $"Economy: {GameManager.Instance.playerEmpire.economy.fundingAllocation} \n" +
            $"Exploration: {GameManager.Instance.playerEmpire.exploration.fundingAllocation} \n" +
            $"Colonization: {GameManager.Instance.playerEmpire.colonization.fundingAllocation} \n" +
            $"Military: {GameManager.Instance.playerEmpire.military.fundingAllocation} \n" +
            $"Science: {GameManager.Instance.playerEmpire.science.fundingAllocation} \n" +
            $"Diplomacy: {GameManager.Instance.playerEmpire.diplomacy.fundingAllocation}";

        //RESOLVED This needs to somehow account for losing ships/strength, right? And how do I track active wars?
        militaryText.text = $"Annual Fleet Construction Capacity: {GameManager.Instance.playerEmpire.militaryCapacity} \n \n" +
            $"Fleet Strength (FS): {GameManager.Instance.playerEmpire.fleetStrength} \n \n" +
            // TODO  $"Allies Fleet Strength: {variableForAlliesFleetStrength} \n \n"
            $"Active Wars: {GameManager.Instance.currentWars} \n \n" +
            $"Enemy FS: {GameManager.Instance.playerEmpire.colonizedPlanets}";

        //TODO - fill in diplomacy, double-check list count - may need -1 or +1
        diplomacyText.text = $"Known Empires: {(GameManager.Instance.knownEmpires.Count)} \n \n" +
            $"Allies: {GameManager.Instance.playerEmpire.discoveredPlanets} \n" +
            $"Trading Partners: {GameManager.Instance.playerEmpire.discoveredPlanets} \n" +
            $"Peaceful Coexistence: {(GameManager.Instance.knownEmpires.Count - GameManager.Instance.currentWars)} \n" +
            $"Hostile: {GameManager.Instance.playerEmpire.discoveredPlanets} \n" +
            $"At War: {GameManager.Instance.currentWars} \n" +
            $"Defeated: {GameManager.Instance.playerDefeatedEmpires} \n \n" +
            $"Diplomatic Capacity: {GameManager.Instance.playerEmpire.yearlyDiplomaticCapacity} \n" +
            $"Needed Capacity: Placeholder \n";

        selectedSectorText.text = $"{GameManager.Instance.currentSector}";

        notificationText.text = GameManager.Instance.currentNotification;
    }
}
