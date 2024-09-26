using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class DisplayVariable : MonoBehaviour
{
    public GameVariables gameVariables;
    public string variableName;
    private Text displayText;

    void Start()
    {
        displayText = GetComponent<Text>();
        if (displayText == null)
        {
            Debug.LogError("DisplayVariable script requires a Text component on the same GameObject.");
            return;
        }

        if (gameVariables == null)
        {
            Debug.LogError("GameVariables component is not set in DisplayVariable script.");
            return;
        }
    }

    void Update()
    {
        if (gameVariables != null && !string.IsNullOrEmpty(variableName))
        {
            FieldInfo field = gameVariables.GetType().GetField(variableName, BindingFlags.Public | BindingFlags.Instance);
            if (field != null)
            {
                object value = field.GetValue(gameVariables);
                if (value != null)
                {
                    // Convert any type of value to a string for displaying
                    displayText.text = value.ToString();
                }
                else
                {
                    Debug.LogWarning("The specified variable exists but its value is null.");
                }
            }
            else
            {
                Debug.LogWarning("The specified variable is not found.");
            }
        }
    }
}
