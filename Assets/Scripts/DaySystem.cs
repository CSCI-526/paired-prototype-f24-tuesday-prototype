using System;
using System.Collections;
using UnityEngine;

public class DaySystem : MonoBehaviour
{
    public GameVariables gameVariables;
    private DateTime currentDateTime;
    public float repeatRate = 6f;

    void Start()
    {
        if (gameVariables == null)
        {
            Debug.LogError("GameVariables component is not assigned!");
            return;
        }

        if (!DateTime.TryParse(gameVariables.currentDateTimeString, out currentDateTime))
        {
            Debug.LogError("Invalid initial date format in GameVariables");
            return;
        }

        StartCoroutine(UpdateDateTimeEverySixSeconds());
    }

    IEnumerator UpdateDateTimeEverySixSeconds()
    {
        // 无限循环，每次循环先等待6秒，然后增加一天
        while (true)
        {
            //Debug.Log(currentDateTime.ToString("yyyy-MM-dd"));
            yield return new WaitForSeconds(repeatRate);  // 首先等待6秒
            currentDateTime = currentDateTime.AddDays(1);  // 然后增加一天
            gameVariables.currentDateTimeString = currentDateTime.ToString("yyyy-MM-dd");
        }
    }
}
