using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;


public class PopupEvent
{
    public string id;
    public string title;
    //public Sprite picture;
    public string description;
    public List<Choice> choices;
    public PopupEvent(string id, string title, string description, List<Choice> choices)
    {
        this.id = id;
        this.title = title;
        this.description = description;
        this.choices = choices;
    }
}

public class PopupEventList
{
    private static PopupEventList instance;
    public List<PopupEvent> events;

    private PopupEventList()
    {
        events = new List<PopupEvent>();
        LoadEvents();
    }

    public static PopupEventList GetInstance()
    {
        if (instance == null)
            instance = new PopupEventList();
        return instance;
    }

    public void LoadEvents()
    {
        events.Add(new PopupEvent(
            "covid",
            "COVID Outbreak Hits the City!",
            "A sudden outbreak of COVID has overwhelmed hospitals and healthcare resources. The citizens are in fear, and drastic actions are needed to manage the situation effectively.",
            new List<Choice> {
                new Choice(
                    "Allocate emergency funds from health budget to hospitals to bolster their resources and support healthcare workers.",
                    new List<ChoiceEffect> {
                        new ChoiceEffectAdd_i("healthBudget", -2000),
                        new ChoiceEffectAdd_i("happiness", 5),
                        new ChoiceEffectAdd_i("healthRate", -20),
                        new ChoiceEffectAdd_i("crimeRate", 5),
                        new ChoiceEffectAdd_i("population", -500),
                    }
                ),
                new Choice(
                    "Implement a strict city-wide lockdown, limiting movement and social gatherings using the crime budget.",
                    new List<ChoiceEffect> {
                        new ChoiceEffectAdd_i("happiness", -20),
                        new ChoiceEffectAdd_i("crimeRate", -10),
                        new ChoiceEffectAdd_i("healthRate", -10),
                        new ChoiceEffectAdd_i("population", -2000),
                        new ChoiceEffectAdd_i("crimeBudget", -2000)
                    }
                )
            }
        ));
        events.Add(new PopupEvent(
            "heatwave",
            "Severe Heatwave Strikes!",
            "A record-breaking heatwave has hit the city, causing power outages, increasing the risk of fires, and putting vulnerable citizens at risk. The city needs immediate relief or long-term solutions to prevent future crises.",
            new List<Choice> {
                new Choice(
                    "Set up emergency cooling centers and free water using the health budget",
                    new List<ChoiceEffect> {
                        new ChoiceEffectAdd_i("healthBudget", -1000),
                        new ChoiceEffectAdd_i("happiness", 10),
                        new ChoiceEffectAdd_i("healthRate", -5),
                        new ChoiceEffectAdd_i("crimeRate", 5),
                        new ChoiceEffectAdd_i("fireRisk", -10),
                    }
                ),
                new Choice(
                    "Upgrade the city’s infrastructure for future heatwaves using the construction budget",
                    new List<ChoiceEffect> {
                        new ChoiceEffectAdd_i("constructionBudget", -1000),
                        new ChoiceEffectAdd_i("happiness", -5),
                        new ChoiceEffectAdd_i("healthRate", -10),
                        new ChoiceEffectAdd_i("fireRisk", -20),
                    }
                )
            }
        ));
    }

    public PopupEvent GetEventById(string id)
    {
        return events.Find(e => e.id == id);
    }
}
