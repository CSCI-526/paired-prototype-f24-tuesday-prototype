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

public class DecisionSystem : MonoBehaviour
{
    public GameObject decisionPanel;
    public List<Decision> decisions; // List of all decisions based on date

    private GameVariables gameVariables; // Reference to your GameVariables class
    private DaySystem daySystem;
    private Button decision1Button; // Button for option A
    private Button decision2Button; // Button for option B

    void Start()
    {
        gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();
        daySystem = GameObject.Find("IndependentSystems").GetComponent<DaySystem>();

        decision1Button = decisionPanel.transform.Find("Button - Decision1").GetComponent<Button>();
        decision2Button = decisionPanel.transform.Find("Button - Decision2").GetComponent<Button>();
        HideDecisionButtons(); // Initially hide the buttons
    }

    public void EvaluateDecision()
    {
        string currentDate = gameVariables.systemInfo.currentDateTimeString; // Assuming this is a string in "YYYY-MM-DD" format

        // Loop through decisions to find the right one based on the current date
        foreach (Decision decision in decisions)
        {
            if (decision.date == currentDate)
            {
                daySystem.TogglePause();
                // panelControlSystem.OpenPanel(decisionPanel);
                GameObject.Find("Canvas").GetComponent<PanelControlSystem>().OpenPanel(decisionPanel);
                ShowDecisionOptions(decision);
                return;
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
        decision1Button.onClick.AddListener(() => ApplyDecisionEffects(decision.healthBudgetEffectA/100, decision.crimeBudgetEffectA/100));
        decision2Button.onClick.AddListener(() => ApplyDecisionEffects(decision.healthBudgetEffectB/100, decision.crimeBudgetEffectB/100));

        // Show buttons when a new decision is available
        decision1Button.gameObject.SetActive(true);
        decision2Button.gameObject.SetActive(true);
    }

    private void ApplyDecisionEffects(float healthEffect, float crimeEffect)
    {
        float happiness = gameVariables.resourcesInfo.happiness / 100f;
        float availableMoney = gameVariables.resourcesInfo.money;
        
        float trueHealthBudget = healthEffect * happiness * availableMoney;
        float trueCrimeBudget = crimeEffect * happiness * availableMoney;
        float remainingMoney = availableMoney - (trueHealthBudget + trueCrimeBudget);
        float constructionBudget = remainingMoney * 0.2f;
        float newMoney = remainingMoney * 0.8f;

        // Update the budget variables in GameVariables
        gameVariables.budgetInfo.healthBudget = trueHealthBudget;
        gameVariables.budgetInfo.crimeBudget = trueCrimeBudget;
        gameVariables.budgetInfo.constructionBudget = constructionBudget;

        // Update the money variable in ResourcesInfo
        gameVariables.resourcesInfo.money = newMoney;

        Debug.Log($"Health Budget: {trueHealthBudget}, Crime Budget: {trueCrimeBudget}, Construction Budget: {constructionBudget}, New Money: {newMoney}");

        // Hide buttons after a decision has been made & unpause
        HideDecisionButtons();
        daySystem.TogglePause();
        GetComponent<ConstructionSystem>().CheckBudget();
        GameObject.Find("Canvas").GetComponent<PanelControlSystem>().OnClickOthersDisappearClose();
    }

    private void HideDecisionButtons()
    {
        decision1Button.gameObject.SetActive(false);
        decision2Button.gameObject.SetActive(false);
    }
}

