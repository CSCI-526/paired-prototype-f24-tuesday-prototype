using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Reflection;
using UnityEngine;

public interface Info { }

public class SystemInfo : Info
{
    public string currentDateTimeString = "2024-01-01";
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
    public int fireRisk = 0;
}

public class GameVariables : MonoBehaviour
{
    public SystemInfo systemInfo;
    public ResourcesInfo resourcesInfo;
    public StatisticsInfo statisticsInfo;

    private void Start()
    {
        systemInfo = new SystemInfo();
        resourcesInfo = new ResourcesInfo();
        statisticsInfo = new StatisticsInfo();
        GameObject.Find("IndependentSystems").GetComponent<DaySystem>().Init();
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
