using System;
using System.Collections;
using System.Collections.Generic;
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
    private GameObject popupEventPanel;
    private GameVariables gameVariables;
    private PopupEventList eventList;

    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        popupEventPanel = canvas.transform.Find("Panel - PopupEvent").gameObject;

        gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();

        eventList = PopupEventList.GetInstance();
        eventList.LoadEvents();
    }

    public void UpdateOnTick()
    {
        DateTriggerer triggerer = dateTriggerer.Find(triggerer => triggerer.dateTime == gameVariables.systemInfo.currentDateTimeString);
        if (triggerer != null)
        {
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
        List<GameObject> buttons = new List<GameObject>();
        for (int i = count - 1; i >= 0; i--)
        {
            GameObject buttonPrefab = Resources.Load<GameObject>("Prefabs/Button - Choice");
            GameObject newButton = Instantiate(buttonPrefab, popupEventPanel.transform);
            RectTransform rectTransform = newButton.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 30 + 50 * i);
            newButton.GetComponent<Button>().onClick.AddListener(() => popupEventPanel.SetActive(false));
            buttons.Add(newButton);
        }
        return buttons;
    }
}
