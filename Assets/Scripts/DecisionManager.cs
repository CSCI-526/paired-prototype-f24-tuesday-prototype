using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Decision
{
    public string optionA;
    public string optionB;
    public float healthBudgetEffectA;
    public float crimeBudgetEffectA;
    public float healthBudgetEffectB;
    public float crimeBudgetEffectB;
    public string date; // Store the date as a string (YYYY-MM-DD)
}


public class DecisionManager : MonoBehaviour
{
    public List<Decision> decisions; // List of all decisions based on date
    private GameVariables gameVariables; // Reference to your GameVariables class
    public Button decision1Button;
    public Button decision2Button;

    void Start()
    {
        gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();
    }

    public void EvaluateDecision()
    {
        string currentDate = gameVariables.systemInfo.currentDateTimeString; // Assuming this is a string in "YYYY-MM-DD" format

        // Loop through decisions to find the right one based on the current date
        foreach (Decision decision in decisions)
        {
            if (decision.date == currentDate)
            {
                // Show options A and B and their effects
                ShowDecisionOptions(decision);
                break; // Exit loop once the current date decision is found
            }
        }
    }

    private void ShowDecisionOptions(Decision decision)
    {
        // Implement UI logic to show the decision options
        Debug.Log($"Options for {decision.date}: {decision.optionA} and {decision.optionB}");

        // Set button texts
        decision1Button.GetComponentInChildren<Text>().text = decision.optionA;
        decision2Button.GetComponentInChildren<Text>().text = decision.optionB;

        // Clear previous listeners
        decision1Button.onClick.RemoveAllListeners();
        decision2Button.onClick.RemoveAllListeners();

        // Add listeners with effects for each option
        decision1Button.onClick.AddListener(() => ApplyDecisionEffects(decision.healthBudgetEffectA, decision.crimeBudgetEffectA));
        decision2Button.onClick.AddListener(() => ApplyDecisionEffects(decision.healthBudgetEffectB, decision.crimeBudgetEffectB));
    }

    private void ApplyDecisionEffects(float healthEffect, float crimeEffect)
    {
        // Calculate the true effects based on the current happiness score
        float happiness = gameVariables.resourcesInfo.happiness / 100f;

        // Calculate the budget effects
        float trueHealthBudget = healthEffect * happiness;
        float trueCrimeBudget = crimeEffect * happiness;
        float reservedBudget = 100f - (trueHealthBudget + trueCrimeBudget);

        // Update the budget variables in GameVariables
        gameVariables.budgetInfo.health_budget = trueHealthBudget;
        gameVariables.budgetInfo.crime_budget = trueCrimeBudget;
        gameVariables.budgetInfo.reserved_budget = reservedBudget;

        Debug.Log($"Health Budget: {trueHealthBudget}%, Crime Budget: {trueCrimeBudget}%, Reserved Budget: {reservedBudget}%");
    }
}
