using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ChoiceEffect
{
    public string gameVariableName;
    public object value;
    public float minValue; // Minimum value for clamping
    public float maxValue; // Maximum value for clamping

    public ChoiceEffect(string gameVariableName, object value, float minValue, float maxValue)
    {
        this.gameVariableName = gameVariableName;
        this.value = value;
        this.minValue = minValue;
        this.maxValue = maxValue;
    }

    public virtual void ApplyEffect() { }
}

public class ChoiceEffectAdd_f : ChoiceEffect
{
    public ChoiceEffectAdd_f(string gameVariableName, float value, float minValue = 0f, float maxValue = 100f)
        : base(gameVariableName, value, minValue, maxValue) { }

    public override void ApplyEffect()
    {
        GameVariables gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();
        KeyValuePair<Info, FieldInfo>? variable = gameVariables.GetVariable(gameVariableName);
        try
        {
            Info info = variable.Value.Key;
            FieldInfo field = variable.Value.Value;
            if (field.FieldType != typeof(float))
                throw new System.Exception();

            float currentValue = (float)field.GetValue(info);
            float newValue = currentValue + (float)value;
            newValue = Mathf.Clamp(newValue, minValue, maxValue);

            field.SetValue(info, newValue);
            Debug.Log($"ChoiceEffectAdd_f({gameVariableName}, {value})");
        }
        catch { Debug.LogError($"ChoiceEffectAdd_f: Invalid Variable {gameVariableName}"); }
    }
}

public class ChoiceEffectAdd_i : ChoiceEffect
{
    public ChoiceEffectAdd_i(string gameVariableName, int value, float minValue = 0, float maxValue = 100)
        : base(gameVariableName, value, minValue, maxValue) { }

    public override void ApplyEffect()
    {
        GameVariables gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();
        KeyValuePair<Info, FieldInfo>? variable = gameVariables.GetVariable(gameVariableName);
        try
        {
            Info info = variable.Value.Key;
            FieldInfo field = variable.Value.Value;
            if (field.FieldType != typeof(int))
                throw new System.Exception();

            int currentValue = (int)field.GetValue(info);
            int newValue = currentValue + (int)value;
            newValue = (int)Mathf.Clamp(newValue, minValue, maxValue);

            field.SetValue(info, newValue);
            Debug.Log($"ChoiceEffectAdd_i({gameVariableName}, {value})");
        }
        catch { Debug.LogError($"ChoiceEffectAdd_i: Invalid Variable {gameVariableName}"); }
    }
}

public class Choice
{
    public string description;
    public List<ChoiceEffect> effects;
    public Choice(string description, List<ChoiceEffect> effects)
    {
        this.description = description;
        this.effects = effects;
    }

    public void ApplyEffects()
    {
        foreach (ChoiceEffect effect in effects)
            effect.ApplyEffect();
        ConstructionSystem constructionSystem = GameObject.Find("IndependentSystems").GetComponent<ConstructionSystem>();
        if (constructionSystem != null)
        {
            constructionSystem.CheckBudget();
        }
        else
        {
            Debug.LogError("ConstructionSystem component not found on the specified GameObject.");
        }
    }
}