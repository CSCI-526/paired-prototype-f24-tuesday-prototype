using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ConstructionSystem;

public class MapSystem : MonoBehaviour
{
    public GameObject map;
    [Tooltip("How many population should a point represent.")]
    public float populationScaleFactor = 100.0f;

    private GameVariables gameVariables;
    private GameObject populations, buildings;
    private GameObject populationPrefab, buildingPrefab;
    private Vector3 mapBoundMin, mapBoundMax;
    private int numPoints;

    public void Init()
    {
        gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();
        numPoints = Mathf.CeilToInt(gameVariables.resourcesInfo.population / populationScaleFactor);

        SpriteRenderer spriteRenderer = map.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) { mapBoundMin = spriteRenderer.bounds.min; mapBoundMax = spriteRenderer.bounds.max; }
        else { mapBoundMin = Vector3.zero; mapBoundMax = Vector3.zero; }

        populations = new GameObject("Populations");
        populations.transform.SetParent(map.transform);
        populationPrefab = Resources.Load<GameObject>("Prefabs/Population");
        buildings = new GameObject("Buildings");
        buildings.transform.SetParent(map.transform);
        buildingPrefab = Resources.Load<GameObject>("Prefabs/Building");

        for (int i = 0; i < numPoints; i++)
        {
            Vector2 randomPos = new Vector2(
                Random.Range(mapBoundMin.x, mapBoundMax.x),
                Random.Range(mapBoundMin.y, mapBoundMax.y)
            );

            GameObject population = Instantiate(populationPrefab, randomPos, Quaternion.identity);
            PopulationStatus populationStatus = population.AddComponent<PopulationStatus>();
            if (Random.value * 100 < gameVariables.resourcesInfo.happiness)
                populationStatus.isHappy = true;
            else
                populationStatus.isHappy = false;

            spriteRenderer = population.GetComponent<SpriteRenderer>();
            if (populationStatus.isHappy)
                spriteRenderer.color = Color.green;
            else
                spriteRenderer.color = Color.red;

            population.transform.parent = populations.transform;
        }
    }

    public void UpdateOnTick()
    {
        numPoints = Mathf.CeilToInt(gameVariables.resourcesInfo.population / populationScaleFactor);
        
        int numPointsOld = populations.transform.childCount;
        if (numPointsOld > numPoints)
            for (int i = numPointsOld - 1; i >= numPoints; i--)
                Destroy(populations.transform.GetChild(i).gameObject);
        else if (numPointsOld < numPoints)
            for (int i = numPointsOld; i < numPoints; i++)
            {
                Vector2 randomPos = new Vector2(
                    Random.Range(mapBoundMin.x, mapBoundMax.x),
                    Random.Range(mapBoundMin.y, mapBoundMax.y)
                );

                GameObject population = Instantiate(populationPrefab, randomPos, Quaternion.identity);
                PopulationStatus populationStatus = population.AddComponent<PopulationStatus>();
                if (Random.value * 100 < gameVariables.resourcesInfo.happiness)
                    populationStatus.isHappy = true;
                else
                    populationStatus.isHappy = false;

                SpriteRenderer spriteRenderer = population.GetComponent<SpriteRenderer>();
                if (populationStatus.isHappy)
                    spriteRenderer.color = Color.green;
                else
                    spriteRenderer.color = Color.red;

                population.transform.parent = populations.transform;
            }

        int numPointsHappy = 0;
        foreach (Transform population in populations.transform)
        {
            PopulationStatus populationStatus = population.GetComponent<PopulationStatus>();
            if (populationStatus.isHappy)
                numPointsHappy++;
        }
        float pointHappiness = (float)numPointsHappy / numPoints * 100;

        if (pointHappiness < gameVariables.resourcesInfo.happiness)
        {
            // change from unhappy to happy
            int diff = Mathf.CeilToInt((gameVariables.resourcesInfo.happiness - pointHappiness) / 100 * numPoints);
            List<Transform> unhappyPopulation = new List<Transform>();

            foreach (Transform population in populations.transform)
            {
                PopulationStatus populationStatus = population.GetComponent<PopulationStatus>();
                if (!populationStatus.isHappy)
                    unhappyPopulation.Add(population);
            }

            for (int i = 0; i < diff && unhappyPopulation.Count > 0; i++)
            {
                int index = Random.Range(0, unhappyPopulation.Count);
                Transform population = unhappyPopulation[index];
                PopulationStatus populationStatus = population.GetComponent<PopulationStatus>();

                populationStatus.isHappy = true;
                population.GetComponent<SpriteRenderer>().color = Color.green;

                unhappyPopulation.RemoveAt(index);
            }
        }
        else if (pointHappiness > gameVariables.resourcesInfo.happiness)
        {
            // change from happy to unhappy
            int diff = Mathf.CeilToInt((pointHappiness - gameVariables.resourcesInfo.happiness) / 100 * numPoints);
            List<Transform> happyPopulation = new List<Transform>();

            foreach (Transform population in populations.transform)
            {
                PopulationStatus populationStatus = population.GetComponent<PopulationStatus>();
                if (populationStatus.isHappy)
                    happyPopulation.Add(population);
            }

            for (int i = 0; i < diff && happyPopulation.Count > 0; i++)
            {
                int index = Random.Range(0, happyPopulation.Count);
                Transform population = happyPopulation[index];
                PopulationStatus populationStatus = population.GetComponent<PopulationStatus>();

                populationStatus.isHappy = false;
                population.GetComponent<SpriteRenderer>().color = Color.red;

                happyPopulation.RemoveAt(index);
            }
        }
    }

    public void build(BuildingType buildingType)
    {
        Vector2 randomPos = new Vector2(
            Random.Range(mapBoundMin.x, mapBoundMax.x),
            Random.Range(mapBoundMin.y, mapBoundMax.y)
        );

        GameObject building = Instantiate(buildingPrefab, randomPos, Quaternion.identity);
        SpriteRenderer spriteRenderer = building.GetComponent<SpriteRenderer>();
        if (buildingType == BuildingType.POLICE_STATION)
            spriteRenderer.color = Color.blue;
        else if (buildingType == BuildingType.HOSPITAL)
            spriteRenderer.color = Color.magenta;
        else if (buildingType == BuildingType.FIRE_DEPARTMENT)
            spriteRenderer.color = new Color(1f, 0.5f, 0f);

        building.transform.parent = buildings.transform;
    }
}
