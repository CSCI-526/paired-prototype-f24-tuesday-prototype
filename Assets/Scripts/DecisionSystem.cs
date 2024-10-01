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
    public string date;
}

public class DecisionSystem : MonoBehaviour
{
    public GameObject decisionPanel;
    public List<Decision> decisions;
    private GameVariables gameVariables;
    private DaySystem daySystem;
    private Button decision1Button;
    private Button decision2Button;

    void Start()
    {
        gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();
        daySystem = GameObject.Find("IndependentSystems").GetComponent<DaySystem>();

        decision1Button = decisionPanel.transform.Find("Button - Decision1").GetComponent<Button>();
        decision2Button = decisionPanel.transform.Find("Button - Decision2").GetComponent<Button>();
        HideDecisionButtons();
    }

    public void EvaluateDecision()
    {
        string currentDate = gameVariables.systemInfo.currentDateTimeString;

        // Loop through decisions to find the right one based on the current date
        foreach (Decision decision in decisions)
        {
            if (decision.date == currentDate)
            {
                daySystem.TogglePause();
                GameObject.Find("Canvas").GetComponent<PanelControlSystem>().OpenPanel(decisionPanel);
                ShowDecisionOptions(decision);
                return;
            }
        }
    }

    private void ShowDecisionOptions(Decision decision)
    {
        Debug.Log($"Options for {decision.date}: {decision.optionA} and {decision.optionB}");
        
        decision1Button.GetComponentInChildren<Text>().text = decision.optionA;
        decision2Button.GetComponentInChildren<Text>().text = decision.optionB;
        decision1Button.onClick.RemoveAllListeners();
        decision2Button.onClick.RemoveAllListeners();
        decision1Button.onClick.AddListener(() => ApplyDecisionEffects(decision.healthBudgetEffectA/100, decision.crimeBudgetEffectA/100));
        decision2Button.onClick.AddListener(() => ApplyDecisionEffects(decision.healthBudgetEffectB/100, decision.crimeBudgetEffectB/100));

        // Show buttons when a new decision is available
        decision1Button.gameObject.SetActive(true);
        decision2Button.gameObject.SetActive(true);
    }

    private void ApplyDecisionEffects(float healthEffect, float crimeEffect)
    {
        float happiness = gameVariables.resourcesInfo.happiness / 100f;
        int availableMoney = gameVariables.resourcesInfo.money;
        
        int trueHealthBudget = (int)(healthEffect * happiness * availableMoney);
        int trueCrimeBudget = (int)(crimeEffect * happiness * availableMoney);
        int remainingMoney = availableMoney - (trueHealthBudget + trueCrimeBudget);
        int constructionBudget = (int)(remainingMoney * 0.2f);
        int newMoney = (int)(remainingMoney * 0.8f);

        // Update the budget variables in GameVariables
        gameVariables.budgetInfo.healthBudget += trueHealthBudget;
        gameVariables.budgetInfo.crimeBudget += trueCrimeBudget;
        gameVariables.budgetInfo.constructionBudget += constructionBudget;
        gameVariables.resourcesInfo.money = newMoney;

        Debug.Log($"Health Budget: {trueHealthBudget}, Crime Budget: {trueCrimeBudget}, Construction Budget: {constructionBudget}, New Money: {newMoney}");
        
        HideDecisionButtons();
        GetComponent<ConstructionSystem>().CheckBudget();
        GameObject.Find("Canvas").GetComponent<PanelControlSystem>().OpenPanel(GameObject.Find("Canvas").transform.Find("Panel - Budget").gameObject);
    }

    private void HideDecisionButtons()
    {
        decision1Button.gameObject.SetActive(false);
        decision2Button.gameObject.SetActive(false);
    }
}

