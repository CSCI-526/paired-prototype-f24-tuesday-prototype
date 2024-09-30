using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Reflection;
using UnityEngine;

public interface Info { }

public class SystemInfo : Info
{
    public string currentDateTimeString = "2024-01-01";
    public int pause = 1; // 0 means continue, 1 means pause
    public string pauseShow = "Paused";

}

public class ResourcesInfo : Info
{
    public float money = 100000; // Amount of money available
    public int population = 100000; // Current city population
    public int happiness = 100; // Overall city happiness index
}

public class StatisticsInfo : Info
{
    public int crimeRate = 0; // Current crime rate percentage
    public int healthRate = 0; // Current health rate percentage
    public int fireRisk = 50; // Current fire rate percentage
}

public class BudgetInfo : Info
{
    public float totalBudget = 100000f;
    public float health_budget = 0f;
    public float crime_budget = 0f;
    public float constructionBudget = 20f;
    public float reserved_budget;

    public BudgetInfo()
    {
        reserved_budget = totalBudget;
    }
}

public class GameVariables : MonoBehaviour
{
    public SystemInfo systemInfo;
    public ResourcesInfo resourcesInfo;
    public StatisticsInfo statisticsInfo;
    public BudgetInfo budgetInfo;

    private void Start()
    {
        systemInfo = new SystemInfo();
        resourcesInfo = new ResourcesInfo();
        statisticsInfo = new StatisticsInfo();
        budgetInfo = new BudgetInfo();
        
        GameObject.Find("IndependentSystems").GetComponent<DaySystem>().Init();
        GameObject.Find("IndependentSystems").GetComponent<MapSystem>().Init();
    }

    // ? is guided by chatGPT
    public KeyValuePair<Info, FieldInfo>? GetVariable(string variableName)
    {
        FieldInfo field;
        field = systemInfo.GetType().GetField(variableName, BindingFlags.Public | BindingFlags.Instance);
        if (field != null)
            return new KeyValuePair<Info, FieldInfo>(systemInfo, field);
        field = resourcesInfo.GetType().GetField(variableName, BindingFlags.Public | BindingFlags.Instance);
        if (field != null)
            return new KeyValuePair<Info, FieldInfo>(resourcesInfo, field);
        field = statisticsInfo.GetType().GetField(variableName, BindingFlags.Public | BindingFlags.Instance);
        if (field != null)
            return new KeyValuePair<Info, FieldInfo>(statisticsInfo, field);
        return null;
    }
}
