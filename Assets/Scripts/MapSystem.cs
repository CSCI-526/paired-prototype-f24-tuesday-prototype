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

            spriteRenderer = population.GetComponent<SpriteRenderer>();
            if (Random.value * 100 < gameVariables.resourcesInfo.happiness)
                spriteRenderer.color = Color.green;
            else
                spriteRenderer.color = Color.red;

            population.transform.parent = populations.transform;
        }
    }

    public void UpdateOnTick()
    {
        numPoints = Mathf.CeilToInt(gameVariables.resourcesInfo.population / populationScaleFactor);
        
        int oldPopulation = populations.transform.childCount;
        if (oldPopulation > numPoints)
            for (int i = oldPopulation - 1; i >= numPoints; i--)
                Destroy(populations.transform.GetChild(i).gameObject);
        else if (oldPopulation < numPoints)
            for (int i = oldPopulation; i < numPoints; i++)
            {
                Vector2 randomPos = new Vector2(
                    Random.Range(mapBoundMin.x, mapBoundMax.x),
                    Random.Range(mapBoundMin.y, mapBoundMax.y)
                );

                GameObject population = Instantiate(populationPrefab, randomPos, Quaternion.identity);

                SpriteRenderer spriteRenderer = population.GetComponent<SpriteRenderer>();
                if (Random.value * 100 < gameVariables.resourcesInfo.happiness)
                    spriteRenderer.color = Color.green;
                else
                    spriteRenderer.color = Color.red;

                population.transform.parent = populations.transform;
            }
        
        foreach (Transform population in populations.transform)
        {
            SpriteRenderer spriteRenderer = population.GetComponent<SpriteRenderer>();

            if (Random.value * 100 < gameVariables.resourcesInfo.happiness)
                spriteRenderer.color = Color.green;
            else
                spriteRenderer.color = Color.red;
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
