using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject platformVariantPrefab;
    ObjectPooler objectPooler;
    Vector3 platformNextSpawnPoint;
    Vector3 platformVariantNextSpawnPoint;
    int numberOfRenderedPlatforms = 10;

    void Start()
    {
        objectPooler = ObjectPooler.SharedInstance;
        platformNextSpawnPoint = platformPrefab.transform.position;
        platformVariantNextSpawnPoint = platformVariantPrefab.transform.position;
        for (int i = 0; i < numberOfRenderedPlatforms; i++)
        {
            if (i >= 2 && i < 5)
                SpawnPlatform(false, true, false, true, true);
            else if (i >= 5)
                SpawnPlatform(true, true, true, true, true);
            else
                SpawnPlatform(false, false, false, true, true);
        }
    }

    public void SpawnPlatform(bool spawnObstacles, bool spawnScoreCollectables, bool spawnHealthCollectables, bool lowerPlat, bool upperPlat)
    {
        if (lowerPlat)
        {
            GameObject clonedPlatformPrefab = objectPooler.SpawnFromPool("Platform", platformNextSpawnPoint, platformPrefab.transform.rotation, transform);
            platformNextSpawnPoint = clonedPlatformPrefab.transform.GetChild(0).transform.position;

            if (spawnObstacles)
                clonedPlatformPrefab.GetComponent<Platform>().SpawnObstacle();
            if (spawnScoreCollectables)
                clonedPlatformPrefab.GetComponent<Platform>().SpawnScoreCollectable();
            if (spawnHealthCollectables)
                clonedPlatformPrefab.GetComponent<Platform>().SpawnHealthCollectable();
        }
        if (upperPlat)
        {
            GameObject clonedPlatformVariantPrefab = objectPooler.SpawnFromPool("Platform Variant", platformVariantNextSpawnPoint, platformVariantPrefab.transform.rotation, transform);
            platformVariantNextSpawnPoint = clonedPlatformVariantPrefab.transform.GetChild(0).transform.position;

            if (spawnObstacles)
                clonedPlatformVariantPrefab.GetComponent<Platform>().SpawnObstacle();
            if (spawnScoreCollectables)
                clonedPlatformVariantPrefab.GetComponent<Platform>().SpawnScoreCollectable();
            if (spawnHealthCollectables)
                clonedPlatformVariantPrefab.GetComponent<Platform>().SpawnHealthCollectable();
        }
    }
}
