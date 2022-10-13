using UnityEngine;

public class ScoreCollectable : MonoBehaviour
{
    ObjectPooler objectPooler;
    public float rotateSpeed;
    private void OnEnable()
    {
        rotateSpeed = 180;
        objectPooler = ObjectPooler.SharedInstance;
    }

    void Update()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("ScoreCollectable") || other.gameObject.CompareTag("HealthCollectable"))
        {
            gameObject.SetActive(false);
            gameObject.transform.SetParent(objectPooler.gameObject.transform, false);
        }
    }
}
