using System;
using UnityEngine;

public class GameVariables : MonoBehaviour
{
    [Header("Date")]
    public string currentDateTimeString = "2024-01-01";

    [Header("Resources")]
    [Tooltip("Amount of money available.")]
    public float money = 100000;
    [Tooltip("Current city population.")]
    public int population = 100000;
    [Tooltip("Overall city happiness index.")]
    public int happiness = 100;

    [Header("Statistics")]
    [Tooltip("Current crime rate percentage.")]
    public int crimeRate = 0;
    [Tooltip("Current health rate percentage.")]
    public int healthRate = 0;
}
