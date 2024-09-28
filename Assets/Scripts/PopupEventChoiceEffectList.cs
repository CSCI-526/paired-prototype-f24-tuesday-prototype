using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public interface ChoiceEffect
{
    public void ApplyEffect();
}

public class ChoiceEffectAdd_f : ChoiceEffect
{
    public string gameVariableName;
    public float value;
    public ChoiceEffectAdd_f(string gameVariableName, float value)
    {
        this.gameVariableName = gameVariableName;
        this.value = value;
    }

    public void ApplyEffect()
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
            field.SetValue(info, currentValue + value);
            Debug.Log($"ChoiceEffectAdd_f({gameVariableName}, {value})");
        }
        catch { Debug.Log($"ChoiceEffectAdd_f: Invalid Variable {gameVariableName}"); }
    }
}

public class ChoiceEffectAdd_i : ChoiceEffect
{
    public string gameVariableName;
    public int value;
    public ChoiceEffectAdd_i(string gameVariableName, int value)
    {
        this.gameVariableName = gameVariableName;
        this.value = value;
    }

    public void ApplyEffect()
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
            field.SetValue(info, currentValue + value);
            Debug.Log($"ChoiceEffectAdd_i({gameVariableName}, {value})");
        }
        catch { Debug.Log($"ChoiceEffectAdd_i: Invalid Variable {gameVariableName}"); }
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