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
    private int INFINITE = 10000000;

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
                        new ChoiceEffectAdd_i("healthBudget", -2000, 0, INFINITE),
                        new ChoiceEffectAdd_i("happiness", 5),
                        new ChoiceEffectAdd_i("healthRate", -20),
                        new ChoiceEffectAdd_i("crimeRate", 5),
                        new ChoiceEffectAdd_i("population", -500, 0, INFINITE),
                    }
                ),
                new Choice(
                    "Implement a strict city-wide lockdown, limiting movement and social gatherings using the crime budget.",
                    new List<ChoiceEffect> {
                        new ChoiceEffectAdd_i("happiness", -20),
                        new ChoiceEffectAdd_i("crimeRate", -10),
                        new ChoiceEffectAdd_i("healthRate", -10),
                        new ChoiceEffectAdd_i("population", -2000, 0, INFINITE),
                        new ChoiceEffectAdd_i("crimeBudget", -2000, 0, INFINITE)
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
                        new ChoiceEffectAdd_i("healthBudget", -1000, 0, INFINITE),
                        new ChoiceEffectAdd_i("happiness", 10),
                        new ChoiceEffectAdd_i("healthRate", -5),
                        new ChoiceEffectAdd_i("crimeRate", 5),
                        new ChoiceEffectAdd_i("fireRisk", -10),
                    }
                ),
                new Choice(
                    "Upgrade the city’s infrastructure for future heatwaves using the construction budget",
                    new List<ChoiceEffect> {
                        new ChoiceEffectAdd_i("constructionBudget", -1000, 0, INFINITE),
                        new ChoiceEffectAdd_i("happiness", -5),
                        new ChoiceEffectAdd_i("healthRate", -10),
                        new ChoiceEffectAdd_i("fireRisk", -20),
                    }
                )
            }
        ));
        events.Add(new PopupEvent(
            "recession",
            "Economic Recession Hits!",
            "A sudden economic downturn has led to increased unemployment and business closures across the city. The government must act quickly to stabilize the economy and support affected citizens.",
            new List<Choice> {
                new Choice(
                    "Implement a job creation program using the construction budget",
                    new List<ChoiceEffect> {
                        new ChoiceEffectAdd_i("constructionBudget", -2000, 0, INFINITE),
                        new ChoiceEffectAdd_i("happiness", 15),
                        new ChoiceEffectAdd_i("healthRate", -5),
                        new ChoiceEffectAdd_i("crimeRate", -10),
                    }
                ),
                new Choice(
                    "Increase welfare support for the unemployed using the health budget",
                    new List<ChoiceEffect> {
                        new ChoiceEffectAdd_i("healthBudget", -1500, 0, INFINITE),
                        new ChoiceEffectAdd_i("happiness", 10),
                        new ChoiceEffectAdd_i("healthRate", -10),
                        new ChoiceEffectAdd_i("crimeRate", 5),
                    }
                ),
                new Choice(
                    "Cut city spending and wait for recovery",
                    new List<ChoiceEffect> {
                        new ChoiceEffectAdd_i("happiness", -10),
                        new ChoiceEffectAdd_i("healthRate", 5),
                        new ChoiceEffectAdd_i("crimeRate", 10),
                        new ChoiceEffectAdd_i("fireRisk", 5),
                    }
                )
            }
        ));
        events.Add(new PopupEvent(
            "flooding",
            "Severe Flooding Occurs!",
            "Heavy rains have caused widespread flooding across the city, damaging homes and infrastructure. Immediate action is required to assist affected citizens and prevent further damage.",
            new List<Choice> {
                new Choice(
                    "Allocate funds for emergency relief and rescue operations using the health budget",
                    new List<ChoiceEffect> {
                        new ChoiceEffectAdd_i("healthBudget", -2500, 0, INFINITE),
                        new ChoiceEffectAdd_i("happiness", -5),
                        new ChoiceEffectAdd_i("healthRate", -15),
                        new ChoiceEffectAdd_i("fireRisk", 5),
                        new ChoiceEffectAdd_i("crimeRate", 5),
                    }
                ),
                new Choice(
                    "Invest in improving drainage systems using the construction budget",
                    new List<ChoiceEffect> {
                        new ChoiceEffectAdd_i("constructionBudget", -3000, 0, INFINITE),
                        new ChoiceEffectAdd_i("happiness", 10),
                        new ChoiceEffectAdd_i("healthRate", 5),
                        new ChoiceEffectAdd_i("fireRisk", -10),
                        new ChoiceEffectAdd_i("crimeRate", -5),
                    }
                ),
                new Choice(
                    "Declare a state of emergency and provide temporary shelter",
                    new List<ChoiceEffect> {
                        new ChoiceEffectAdd_i("healthBudget", -1500, 0, INFINITE),
                        new ChoiceEffectAdd_i("happiness", 5),
                        new ChoiceEffectAdd_i("healthRate", -10),
                        new ChoiceEffectAdd_i("crimeRate", 0),
                        new ChoiceEffectAdd_i("fireRisk", 0),
                    }
                )
            }
        ));
        events.Add(new PopupEvent(
            "hostage",
            "Hostage Situation at the City Bank!",
            "A group of armed robbers has taken hostages at the city bank. The situation is tense, and immediate action is required to ensure the safety of the hostages and provide support for any victims injured during the robbery.",
            new List<Choice> {
                new Choice(
                    "Allocate additional funds for SWAT and negotiation teams using the crime budget",
                    new List<ChoiceEffect> {
                        new ChoiceEffectAdd_i("crimeBudget", -3000, 0, INFINITE),
                        new ChoiceEffectAdd_i("happiness", -5),
                        new ChoiceEffectAdd_i("crimeRate", -30),
                        new ChoiceEffectAdd_i("healthRate", 0),
                        new ChoiceEffectAdd_i("fireRisk", 0)
                    }
                ),
                new Choice(
                    "Provide immediate medical assistance to victims injured during the robbery using the health budget",
                    new List<ChoiceEffect> {
                        new ChoiceEffectAdd_i("healthBudget", -2000, 0, INFINITE),
                        new ChoiceEffectAdd_i("happiness", 10),
                        new ChoiceEffectAdd_i("healthRate", 15),
                        new ChoiceEffectAdd_i("crimeRate", -5),
                        new ChoiceEffectAdd_i("fireRisk", 0)
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
