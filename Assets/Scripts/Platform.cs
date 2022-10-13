using UnityEngine;

public class Platform : MonoBehaviour
{
    private PlatformSpawner platformSpawner;
    public GameObject obstaclePrefab;
    public GameObject obstaclePrefabVariant0;
    public GameObject obstaclePrefabVariant1;
    public GameObject scoreCollectablePrefab;
    public GameObject healthCollectablePrefab;
    ObjectPooler objectPooler;

    private void OnEnable()
    {
        objectPooler = ObjectPooler.SharedInstance;
        platformSpawner = GameObject.FindObjectOfType<PlatformSpawner>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            while(transform.childCount != 13)
            {
                transform.GetChild(13).gameObject.SetActive(false);
                transform.GetChild(13).SetParent(objectPooler.gameObject.transform, false);
            }
            if(transform.position.y > 0)
                platformSpawner.SpawnPlatform(true, true, true, false, true);
            else
                platformSpawner.SpawnPlatform(true, true, true, true, false);
        }
    }

    public void SpawnObstacle()
    {
        if (Random.Range(0, 2) == 1)
        {
            int obstacleType = Random.Range(0, 3);
            if (obstacleType == 0)
                objectPooler.SpawnFromPool("Obstacle", transform.GetChild(Random.Range(1, 4)).transform.localPosition, Quaternion.identity, transform);
            else if (obstacleType == 1)
                objectPooler.SpawnFromPool("Obstacle Variant0", transform.GetChild(Random.Range(6, 8)).transform.localPosition, Quaternion.identity, transform);
            else if (obstacleType == 2 && Random.Range(0, 4) == 0)
                objectPooler.SpawnFromPool("Obstacle Variant1", transform.GetChild(2).transform.localPosition, Quaternion.identity, transform);
        }
    }

    public void SpawnScoreCollectable()
    {
        int matIndex = Random.Range(0, 3);
        int number = Random.Range(0, 6);
        float prevX = -1;
        for (int i = 0; i < number; i++)
        {
            Vector3 randomPoint = RandomPointOnGround(GetComponent<Collider>());
            Vector3 collectablePosition = transform.GetChild(4).transform.localPosition;
            collectablePosition.x = randomPoint.x;
            collectablePosition.z = randomPoint.z;
            GameObject scoreCollectable = objectPooler.SpawnFromPool("ScoreCollectable", collectablePosition, scoreCollectablePrefab.transform.rotation, transform);
            if (collectablePosition.x != prevX)
            {
                matIndex = Random.Range(0, 3);
            }
            scoreCollectable.GetComponent<MeshRenderer>().material = GameManager.game.scoreCollectableMatArr[matIndex];
            prevX = collectablePosition.x;
        }
    }

    public void SpawnHealthCollectable()
    {
        bool spawn = (Random.Range(0, 10) == 0);
        if (spawn)
        {
            Vector3 randomPoint = RandomPointOnGround(GetComponent<Collider>());
            Vector3 collectablePosition = transform.GetChild(4).transform.localPosition;
            collectablePosition.x = randomPoint.x;
            collectablePosition.z = randomPoint.z;
            objectPooler.SpawnFromPool("HealthCollectable", collectablePosition, Quaternion.identity, transform);
        }
    }

    Vector3 RandomPointOnGround(Collider collider)
    {
        // idk why collider center changes when spawning from pool
        Vector3 point = transform.position;
        point.x = Random.Range(0, 3);
        point.z = Random.Range(0-collider.bounds.extents.z, collider.bounds.extents.z);

        if(point.x == 0) point.x = -3;
        else if (point.x == 1) point.x = 0;
        else point.x = 3;
        return point;
    }
}
