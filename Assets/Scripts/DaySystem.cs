using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DaySystem : MonoBehaviour
{
    public GameVariables gameVariables;
    public GameObject sliderGameObject;
    public Text timeText;
    private Slider timeSlider;
    private DateTime currentDateTime;
    private Image sliderFillImage;
    public float repeatRate = 6f;

    public void Init()
    {
        if (sliderGameObject != null)
        {
            timeSlider = sliderGameObject.GetComponent<Slider>();
            sliderFillImage = sliderGameObject.GetComponentInChildren<Image>();
        }
        if (timeSlider != null)
        {
            timeSlider.maxValue = repeatRate;
        }
                
        if (gameVariables == null)
        {
            Debug.LogError("GameVariables component is not assigned!");
            return;
        }
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
                }
            }

            if (elapsedTime >= repeatRate)
            {
                // Update time and reset timer
                currentDateTime = currentDateTime.AddDays(1);
                gameVariables.systemInfo.currentDateTimeString = currentDateTime.ToString("yyyy-MM-dd");
                gameObject.GetComponent<PopupEventSystem>().UpdateOnTick();
                elapsedTime = 0;
                if (timeSlider != null)
                    timeSlider.value = 0;
            }
        }
    }

    void UpdateSliderColor(float normalizedTime)
    {
        // Example color transition: Night (0%) -> Day (50%) -> Night (100%)
        Color dayColor = Color.cyan;
        Color nightColor = Color.blue;
        sliderFillImage.color = Color.Lerp(nightColor, dayColor, Mathf.PingPong(normalizedTime * 2, 1));
    }

    void UpdateTimeText(float elapsedTime)
    {
        // 根据 elapsedTime 计算小时和分钟
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
