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
    public int money = 10000; // Amount of unallocated money available
    public int population = 10000; // Current city population
    public int happiness = 50; // Overall city happiness index
    public string finalGrade = "A";
}

public class StatisticsInfo : Info
{
    public int crimeRate = 0; // Current crime rate percentage
    public int healthRate = 0; // Current health rate percentage
    public int fireRisk = 50; // Current fire rate percentage
    public int constructionCost = 500;
}

public class BudgetInfo : Info
{
    public int healthBudget = 0;
    public int crimeBudget = 0;
    public int constructionBudget = 0;
}

public class GameVariables : MonoBehaviour
{
    public SystemInfo systemInfo;
    public ResourcesInfo resourcesInfo;
    public StatisticsInfo statisticsInfo;
    public BudgetInfo budgetInfo;

    private GameObject systems;

    private void Start()
    {
        systemInfo = new SystemInfo();
        resourcesInfo = new ResourcesInfo();
        statisticsInfo = new StatisticsInfo();
        budgetInfo = new BudgetInfo();

        systems = GameObject.Find("IndependentSystems");
        systems.GetComponent<DaySystem>().Init();
        systems.GetComponent<MapSystem>().Init();
        systems.GetComponent<ConstructionSystem>().Init();
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
        field = budgetInfo.GetType().GetField(variableName, BindingFlags.Public | BindingFlags.Instance);
        if (field != null)
            return new KeyValuePair<Info, FieldInfo>(budgetInfo, field);
        return null;
    }
}
