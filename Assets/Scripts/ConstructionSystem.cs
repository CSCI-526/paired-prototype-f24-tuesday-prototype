using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionSystem : MonoBehaviour
{
    public GameObject constructionPanel;
    public enum BuildingType { POLICE_STATION, HOSPITAL, FIRE_DEPARTMENT }

    private GameVariables gameVariables;


    void Start()
    {
        gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();

        Button policeStationButton = constructionPanel.transform.Find("Button - Police Station").GetComponent<Button>();
        policeStationButton.onClick.AddListener(() => build(BuildingType.POLICE_STATION));
        Button hospitalButton = constructionPanel.transform.Find("Button - Hospital").GetComponent<Button>();
        hospitalButton.onClick.AddListener(() => build(BuildingType.HOSPITAL));
        Button fireDepartmentButton = constructionPanel.transform.Find("Button - Fire Department").GetComponent<Button>();
        fireDepartmentButton.onClick.AddListener(() => build(BuildingType.FIRE_DEPARTMENT));
    }

    void build(BuildingType buildingType)
    {
        if (buildingType == BuildingType.POLICE_STATION)
        {
            gameVariables.resourcesInfo.money -= 5000;
            gameVariables.statisticsInfo.crimeRate -= 10;
        }
        else if (buildingType == BuildingType.HOSPITAL)
        {
            gameVariables.resourcesInfo.money -= 5000;
            gameVariables.statisticsInfo.healthRate += 10;
        }
        else if (buildingType == BuildingType.FIRE_DEPARTMENT)
        {
            gameVariables.resourcesInfo.money -= 5000;
            gameVariables.statisticsInfo.fireRisk -= 10;
        }
        GetComponent<MapSystem>().build(buildingType);
    }
}
