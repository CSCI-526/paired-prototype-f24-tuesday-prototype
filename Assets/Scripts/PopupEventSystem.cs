using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DateTriggerer
{
    public string dateTime, popupEventId;
}

public class PopupEventSystem : MonoBehaviour
{
    public List<DateTriggerer> dateTriggerer;

    private GameVariables gameVariables;
    private GameObject popupEventPanel;
    private GameObject buttonPrefab;
    private PopupEventList eventList;

    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();
        popupEventPanel = canvas.transform.Find("Panel - PopupEvent").gameObject;
        buttonPrefab = Resources.Load<GameObject>("Prefabs/Button - Choice");

        eventList = PopupEventList.GetInstance();
        eventList.LoadEvents();
    }

    public void UpdateOnTick()
    {
        DateTriggerer triggerer = dateTriggerer.Find(triggerer => triggerer.dateTime == gameVariables.systemInfo.currentDateTimeString);
        if (triggerer != null)
        {
            GameObject.Find("IndependentSystems").GetComponent<DaySystem>().TogglePause();
            PopupEvent popupEvent = eventList.GetEventById(triggerer.popupEventId);
            popupEventPanel.transform.Find("Title").GetComponent<Text>().text = popupEvent.title;
            popupEventPanel.transform.Find("Description").GetComponent<Text>().text = popupEvent.description;
            if (popupEvent.choices.Count > 0)
            {
                List<GameObject> buttons = PopupEventPanelCreateButtons(popupEvent.choices.Count);
                for (int i = 0; i < buttons.Count; i++)
                {
                    GameObject button = buttons[i];
                    Choice choice = popupEvent.choices[i];
                    button.transform.Find("Text").GetComponent<Text>().text = choice.description;
                    button.GetComponent<Button>().onClick.AddListener(() => choice.ApplyEffects());
                    foreach (ChoiceEffect effect in choice.effects)
                    {
                        KeyValuePair<Info, FieldInfo>? variable = gameVariables.GetVariable(effect.gameVariableName);
                        try
                        {
                            Info info = variable.Value.Key;
                            if (info.GetType() == typeof(BudgetInfo))
                            {
                                FieldInfo field = variable.Value.Value;
                                if (field.FieldType != typeof(int))
                                    throw new System.Exception();
                                int currentBudget = (int)field.GetValue(info);
                                if (currentBudget + (int)effect.value < 0)
                                    button.GetComponent<Button>().interactable = false;
                            }
                        }
                        catch { Debug.LogError($"PopupEventSystem: Invalid Variable {effect.gameVariableName}"); }
                    }
                }
            }
            else
            {
                PopupEventPanelCreateButtons(1);
            }
            popupEventPanel.SetActive(true);
        }
    }

    private List<GameObject> PopupEventPanelCreateButtons(int count)
    {
        foreach (Transform child in popupEventPanel.transform)
            if (child.gameObject.name.Contains("Button - Choice"))
                Destroy(child.gameObject);

        List<GameObject> buttons = new List<GameObject>();
        for (int i = count - 1; i >= 0; i--)
        {
            GameObject newButton = Instantiate(buttonPrefab, popupEventPanel.transform);
            RectTransform rectTransform = newButton.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 30 + 50 * i);
            newButton.GetComponent<Button>().onClick.AddListener(() => popupEventPanel.SetActive(false));
            buttons.Add(newButton);
        }
        return buttons;
    }
}
