using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
    ObjectPooler objectPooler;
    private void OnEnable()
    {
        objectPooler = ObjectPooler.SharedInstance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            gameObject.SetActive(false);
            gameObject.transform.SetParent(objectPooler.gameObject.transform, false);
        }
    }
}
