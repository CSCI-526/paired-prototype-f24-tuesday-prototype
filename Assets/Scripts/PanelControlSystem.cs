using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class PanelButtonPair
{
    public GameObject panel, button;
    public string GetPairName()
    {
        string[] PanelNames = panel.name.Split(' ');
        return PanelNames[PanelNames.Length - 1];
    }
}

public class PanelControlSystem : MonoBehaviour
{
    public List<PanelButtonPair> panelButtonPairOnClickOthersDisappear;
    public List<PanelButtonPair> panelButtonPairOnClickOthersPresent;

    private PanelButtonPair currentPairOnClickOthersDisappear;
    private GameObject decisionPanel, statisticsPanel;

    // Start is called before the first frame update
    void Start()
    {
        PanelButtonPair infoPair = panelButtonPairOnClickOthersDisappear.Find(pair => pair.GetPairName() == "Info");
        infoPair.panel.SetActive(true);
        currentPairOnClickOthersDisappear = infoPair;

        foreach (PanelButtonPair pair in panelButtonPairOnClickOthersDisappear)
        {
            Button button = pair.button.GetComponent<Button>();
            button.onClick.AddListener(() => OnClickOthersDisappear());
        }
    }

    public void OnClickOthersDisappear()
    {
        OpenPanel(EventSystem.current.currentSelectedGameObject);
    }

    public void OpenPanel(GameObject panel)
    {
        PanelButtonPair clickedPair = panelButtonPairOnClickOthersDisappear.Find(pair => pair.button == panel);
        if (currentPairOnClickOthersDisappear == null)
        {
            clickedPair.panel.SetActive(true);
            currentPairOnClickOthersDisappear = clickedPair;
        }
        else if (currentPairOnClickOthersDisappear == clickedPair)
        {
            OnClickOthersDisappearClose();
        }
        else
        {
            currentPairOnClickOthersDisappear.panel.SetActive(false);
            clickedPair.panel.SetActive(true);
            currentPairOnClickOthersDisappear = clickedPair;
        }
    }

    public void OnClickOthersDisappearClose()
    {
        currentPairOnClickOthersDisappear.panel.SetActive(false);
        currentPairOnClickOthersDisappear = null;
    }

    public void OnClickOthersPresentClose()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        PanelButtonPair clickedPair = panelButtonPairOnClickOthersPresent.Find(pair => pair.button == clickedButton);
        clickedPair.panel.SetActive(false);
    }
}
