using System.Collections;
using UnityEngine;

public class Calculation : MonoBehaviour
{
    public GameVariables gameVariables;
    public float taxRatePerPerson = 0.1f;
    public float repeatRate = 1f;

    private int crimeRate;
    private int healthRate;
    private int fireRisk;
    private int happiness;
    private int population;

    public void Init()
    {
        StartCoroutine(CalculateHappinessPeriodically());
    }
    void UpdateRates()
    {
        if (gameVariables != null)
        {
            crimeRate = gameVariables.statisticsInfo.crimeRate;
            healthRate = gameVariables.statisticsInfo.healthRate;
            fireRisk = gameVariables.statisticsInfo.fireRisk;
            population = gameVariables.resourcesInfo.population;
            happiness = gameVariables.resourcesInfo.happiness;
        }
    }


    IEnumerator CalculateHappinessPeriodically()
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
                    elapsedTime = 0;
                    break;
                }
            }
            if (elapsedTime >= repeatRate)
            {
                UpdateRates();
                CalculateHappiness();
                elapsedTime = 0;
            }
        }
    }

    public void CalculateHappiness()
    {
        UpdateRates();
        float populationFactor = Mathf.Log10(Mathf.Max(1, population));
        int impact = (int)(((crimeRate-10) + (-healthRate-10) + (fireRisk-10)) * populationFactor / 30);
        int newHappiness = Mathf.Clamp(happiness - impact, 0, 100);
        gameVariables.resourcesInfo.happiness = newHappiness;
    }

    public void ApplyTaxes()
    {
        float taxCollected = population * taxRatePerPerson;
        gameVariables.resourcesInfo.money += taxCollected;
        Debug.Log($"Tax collected: {taxCollected}, Total money: {gameVariables.resourcesInfo.money}");
    }
}
