using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionSystem : MonoBehaviour
{
    public GameObject constructionPanel;
    public Text budgetText, costText;
    public enum BuildingType { POLICE_STATION, HOSPITAL, FIRE_DEPARTMENT }

    private GameVariables gameVariables;
    private Button policeStationButton, hospitalButton, fireDepartmentButton;

    public void Init()
    {
        gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();

        policeStationButton = constructionPanel.transform.Find("Button - Police Station").GetComponent<Button>();
        policeStationButton.onClick.AddListener(() => build(BuildingType.POLICE_STATION));
        hospitalButton = constructionPanel.transform.Find("Button - Hospital").GetComponent<Button>();
        hospitalButton.onClick.AddListener(() => build(BuildingType.HOSPITAL));
        fireDepartmentButton = constructionPanel.transform.Find("Button - Fire Department").GetComponent<Button>();
        fireDepartmentButton.onClick.AddListener(() => build(BuildingType.FIRE_DEPARTMENT));

        CheckBudget();
    }

    public void CheckBudget()
    {
        if (gameVariables.budgetInfo.constructionBudget < gameVariables.statisticsInfo.constructionCost)
        {
            policeStationButton.interactable = false;
            hospitalButton.interactable = false;
            fireDepartmentButton.interactable = false;
        }
        else
        {
            policeStationButton.interactable = true;
            hospitalButton.interactable = true;
            fireDepartmentButton.interactable = true;
        }
        budgetText.text = gameVariables.budgetInfo.constructionBudget.ToString();
        costText.text = gameVariables.statisticsInfo.constructionCost.ToString();
    }

    void build(BuildingType buildingType)
    {
        if (buildingType == BuildingType.POLICE_STATION)
        {
            gameVariables.budgetInfo.constructionBudget -= gameVariables.statisticsInfo.constructionCost;
            gameVariables.statisticsInfo.crimeRate -= 10;
        }
        else if (buildingType == BuildingType.HOSPITAL)
        {
            gameVariables.budgetInfo.constructionBudget -= gameVariables.statisticsInfo.constructionCost;
            gameVariables.statisticsInfo.healthRate += 10;
        }
        else if (buildingType == BuildingType.FIRE_DEPARTMENT)
        {
            gameVariables.budgetInfo.constructionBudget -= gameVariables.statisticsInfo.constructionCost;
            gameVariables.statisticsInfo.fireRisk -= 10;
        }
        GetComponent<MapSystem>().build(buildingType);

        CheckBudget();
    }
}
