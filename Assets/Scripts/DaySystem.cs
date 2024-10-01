using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DaySystem : MonoBehaviour
{
    public GameObject TimeModule;
    public GameObject FinalPage;
    public Text timeText;
    public int currentMonth;
    public bool todayIsNewMonth = false;
    public float repeatRate = 1f, maxSpeed = 8f, minSpeed = 0.5f;

    private GameVariables gameVariables;
    private Slider timeSlider;
    private DateTime currentDateTime;
    private Image sliderFillImage;
    private Calculation calculation;


    public void Init()
    {
        timeSlider = TimeModule.transform.Find("Slider").GetComponent<Slider>();
        timeSlider.maxValue = 1;
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
            yield return new WaitUntil(() => !gameVariables.systemInfo.pause);
            while (elapsedTime < 1)
            {
                yield return new WaitForSeconds(step);
                if (!gameVariables.systemInfo.pause)
                {
                    elapsedTime += step / repeatRate;
                    timeSlider.value = elapsedTime;
                    UpdateTimeText(elapsedTime);
                    GetComponent<MapSystem>().UpdateOnTick();
                }
            }

            if (elapsedTime >= 1)
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
                if (currentDateTime.ToString("yyyy-MM-dd") == "2024-04-01")
                {
                    gameVariables.systemInfo.pause = true;
                    calculation.FinalGradeCalculation();
                    FinalPage.SetActive(true);
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

    private void UpdateTimeText(float elapsedTime)
    {
        int hours = (int)(24 * elapsedTime);
        int minutes = (int)(1440 * elapsedTime) % 60;
        timeText.text = string.Format("{0:00}:{1:00}", hours, minutes);
    }

    public void TogglePause()
    {
        gameVariables.systemInfo.pause = !gameVariables.systemInfo.pause;
        gameVariables.systemInfo.pauseShow = gameVariables.systemInfo.pause ? "Paused" : "Unpaused";
    }

    public void SpeedUp()
    {
        repeatRate = Mathf.Clamp(repeatRate / 2, minSpeed, maxSpeed);
    }

    public void SpeedDown()
    {
        repeatRate = Mathf.Clamp(repeatRate * 2, minSpeed, maxSpeed);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (EventSystem.current.currentSelectedGameObject != null)
                EventSystem.current.SetSelectedGameObject(null);
            else
                TogglePause();
        }
    }
}
