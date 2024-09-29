using System;
using System.Collections;
using UnityEngine;

public class DaySystem : MonoBehaviour
{
    public float repeatRate = 6f;
    private GameVariables gameVariables;
    private DateTime currentDateTime;

    public void Init()
    {
        gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();
        if (!DateTime.TryParse(gameVariables.systemInfo.currentDateTimeString, out currentDateTime))
        {
            Debug.LogError("Invalid initial date format in GameVariables");
            return;
        }

        StartCoroutine(UpdateDateTimeEverySixSeconds());
    }

    IEnumerator UpdateDateTimeEverySixSeconds()
    {
        float elapsedTime = 0;
        while (true)
        {
            //Debug.Log(currentDateTime.ToString("yyyy-MM-dd"));
            yield return new WaitUntil(() => gameVariables.systemInfo.pause == 0);
            while (elapsedTime < repeatRate)
            {
                yield return new WaitForSeconds(1);
                if (gameVariables.systemInfo.pause == 0)
                {
                    elapsedTime += 1;
                }
                else
                {
                    elapsedTime = 0; // Reset the timer if paused
                    break;
                }
            }

            if (elapsedTime >= repeatRate)
            {
                // Update time and reset timer
                currentDateTime = currentDateTime.AddDays(1);
                gameVariables.systemInfo.currentDateTimeString = currentDateTime.ToString("yyyy-MM-dd");
                GetComponent<PopupEventSystem>().UpdateOnTick();
                GetComponent<MapSystem>().UpdateOnTick();
                elapsedTime = 0;
            }
        }
    }

    public void TogglePause()
    {
        gameVariables.systemInfo.pause = gameVariables.systemInfo.pause == 0 ? 1 : 0;
        gameVariables.systemInfo.pauseShow = gameVariables.systemInfo.pause == 1 ? "Paused" : "Unpaused";
    }
}
