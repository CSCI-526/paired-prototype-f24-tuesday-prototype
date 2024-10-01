using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ChoiceEffect
{
    public string gameVariableName;
    public object value;

    public virtual void ApplyEffect() { }
}

public class ChoiceEffectAdd_f : ChoiceEffect
{
    
    public ChoiceEffectAdd_f(string gameVariableName, float value)
    {
        this.gameVariableName = gameVariableName;
        this.value = value;
    }

    public override void ApplyEffect()
    {
        GameVariables gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();
        // Guided by ChatGPT
        KeyValuePair<Info, FieldInfo>? variable = gameVariables.GetVariable(gameVariableName);
        try
        {
            Info info = variable.Value.Key;
            FieldInfo field = variable.Value.Value;
            if (field.FieldType != typeof(float))
                throw new System.Exception();
            float currentValue = (float)field.GetValue(info);
            field.SetValue(info, currentValue + (float)value);
            Debug.Log($"ChoiceEffectAdd_f({gameVariableName}, {value})");
        }
        catch { Debug.LogError($"ChoiceEffectAdd_f: Invalid Variable {gameVariableName}"); }
    }
}

public class ChoiceEffectAdd_i : ChoiceEffect
{
    public ChoiceEffectAdd_i(string gameVariableName, int value)
    {
        this.gameVariableName = gameVariableName;
        this.value = value;
    }

    public override void ApplyEffect()
    {
        GameVariables gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();
        // Guided by ChatGPT
        KeyValuePair<Info, FieldInfo>? variable = gameVariables.GetVariable(gameVariableName);
        try
        {
            Info info = variable.Value.Key;
            FieldInfo field = variable.Value.Value;
            if (field.FieldType != typeof(int))
                throw new System.Exception();
            int currentValue = (int)field.GetValue(info);
            int newValue = currentValue + (int)value;
            field.SetValue(info, Mathf.Clamp(newValue, 0, 100));
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
    }
}