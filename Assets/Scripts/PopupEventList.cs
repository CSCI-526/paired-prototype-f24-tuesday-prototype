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

            "COVID Outbreak!",
            "A sudden outbreak of COVID has hit the city, overwhelming hospitals and requiring immediate resource allocation.",
            new List<Choice> {
                new Choice(
                    "Provide emergency medical funding!",
                    new List<ChoiceEffect> {
                        new ChoiceEffectAdd_f("money", -50000), // 为紧急医疗分配资金
                        new ChoiceEffectAdd_i("happiness", -10), // 幸福指数因恐慌和隔离措施下降
                        new ChoiceEffectAdd_i("healthRate", 25), // 医疗系统增强，健康率提高
                        new ChoiceEffectAdd_i("population", -1000) // 部分人口因疫情失去
                    }
                ),
                new Choice(
                    "Impose strict lockdown and wait for the outbreak to subside.",
                    new List<ChoiceEffect> {
                        new ChoiceEffectAdd_i("happiness", -30), // 严格封锁导致幸福感大幅下降
                        new ChoiceEffectAdd_i("crimeRate", 5), // 封锁引发恐慌和社会不稳定，犯罪率增加
                        new ChoiceEffectAdd_i("healthRate", 10), // 封锁有效缓解疫情，健康率略微提升
                        new ChoiceEffectAdd_i("population", -500) // 依然有部分人口因疫情失去
                    }
                )
            }
        ));
        events.Add(new PopupEvent(
            "harris",

            "Harris was shot by a bullet",
            "On *****, Donald Trump was shot...",
            new List<Choice> {
                new Choice(
                    "Raise your hand!",
                    new List<ChoiceEffect> {
                        new ChoiceEffectAdd_i("crimeRate", 1)
                    }
                ),
                new Choice(
                    "Flee!",
                    new List<ChoiceEffect> {
                        new ChoiceEffectAdd_i("healthRate", 1)
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
