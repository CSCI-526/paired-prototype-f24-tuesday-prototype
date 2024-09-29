using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DaySystem : MonoBehaviour
{
    public GameObject sliderGameObject;
    public Text timeText;
    private GameVariables gameVariables;
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
                }
                else
                {
                    break;
                }
            }

            if (elapsedTime >= repeatRate)
            {
                currentDateTime = currentDateTime.AddDays(1);
                gameVariables.systemInfo.currentDateTimeString = currentDateTime.ToString("yyyy-MM-dd");

                GetComponent<PopupEventSystem>().UpdateOnTick();
                elapsedTime = 0;
                if (timeSlider != null)
                    timeSlider.value = 0;
                UpdateTimeText(elapsedTime);
            }
        }
    }

    void UpdateSliderColor(float normalizedTime)
    {
        Color dayColor = Color.cyan;
        Color nightColor = Color.blue;
        sliderFillImage.color = Color.Lerp(nightColor, dayColor, Mathf.PingPong(normalizedTime * 2, 1));
    }

    void UpdateTimeText(float elapsedTime)
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
