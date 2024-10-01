﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DaySystem : MonoBehaviour
{
    public GameObject TimeModule;
    public Text timeText;
    public int currentMonth;
    public bool todayIsNewMonth = false;
    public float repeatRate = 6f;

    private GameVariables gameVariables;
    private Slider timeSlider;
    private DateTime currentDateTime;
    private Image sliderFillImage;
    private Calculation calculation;


    public void Init()
    {
        timeSlider = TimeModule.transform.Find("Slider").GetComponent<Slider>();
        timeSlider.maxValue = repeatRate;
        //sliderFillImage = TimeModule.transform.Find("Slider").GetComponentInChildren<Image>();

        gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();
        if (!DateTime.TryParse(gameVariables.systemInfo.currentDateTimeString, out currentDateTime))
        {
            Debug.LogError("Invalid initial date format in GameVariables");
            return;
        }

        calculation = GetComponent<Calculation>();

        currentMonth = currentDateTime.Month;

        StartCoroutine(UpdateDateTimeEverySixSeconds());
    }

    IEnumerator UpdateDateTimeEverySixSeconds()
    {
        float elapsedTime = 0;
        float step = 0.01f;
        while (true)
        {
            //Debug.Log(currentDateTime.ToString("yyyy-MM-dd"));
            yield return new WaitUntil(() => gameVariables.systemInfo.pause == 0);
            while (elapsedTime < repeatRate)
            {
                yield return new WaitForSeconds(step);
                if (gameVariables.systemInfo.pause == 0)
                {
                    elapsedTime += step;
                    if (timeSlider != null)
                        timeSlider.value = elapsedTime;
                        UpdateTimeText(elapsedTime);
                    // UpdateSliderColor(elapsedTime / repeatRate);
                    GetComponent<MapSystem>().UpdateOnTick();
                }
            }

            if (elapsedTime >= repeatRate)
            {
                currentDateTime = currentDateTime.AddDays(1);
                calculation.CalculateHappiness();
                if (currentMonth != currentDateTime.Month)
                {
                    currentMonth = currentDateTime.Month;
                }
                todayIsNewMonth = CheckIfMonthWillChange();
                if (todayIsNewMonth)
                {
                    calculation.ApplyTaxes();
                }
                gameVariables.systemInfo.currentDateTimeString = currentDateTime.ToString("yyyy-MM-dd");
                
                GetComponent<DecisionSystem>().EvaluateDecision();
                GetComponent<PopupEventSystem>().UpdateOnTick();
                elapsedTime = 0;
                if (timeSlider != null)
                    timeSlider.value = 0;
                UpdateTimeText(elapsedTime);
            }
        }
    }

    private bool CheckIfMonthWillChange()
    {
        DateTime nextDay = currentDateTime.AddDays(-1);
        return nextDay.Month != currentDateTime.Month;
    }


    //private void UpdateSliderColor(float normalizedTime)
    //{
    //    Color dayColor = Color.cyan;
    //    Color nightColor = Color.blue;
    //    sliderFillImage.color = Color.Lerp(nightColor, dayColor, Mathf.PingPong(normalizedTime * 2, 1));
    //}

    private void UpdateTimeText(float elapsedTime)
    {
        int hours = (int)(24 * elapsedTime / repeatRate);
        int minutes = (int)(1440 * elapsedTime / repeatRate) % 60;
        timeText.text = string.Format("{0:00}:{1:00}", hours, minutes);
    }

    public void TogglePause()
    {
        gameVariables.systemInfo.pause = gameVariables.systemInfo.pause == 0 ? 1 : 0;
        gameVariables.systemInfo.pauseShow = gameVariables.systemInfo.pause == 1 ? "Paused" : "Unpaused";
    }
}
