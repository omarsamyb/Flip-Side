using CjLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class minion : MonoBehaviour
{

    public float xspeed;
    public float yspeed;
    private float moveSpeed;

    private Vector3 rotation0target0 = new Vector3(-180, 180, 0);
    public Vector3 v3Current = Vector3.zero;
    private Vector3 positionTarget = new Vector3(0, 4, 0);

    // Start is called before the first frame update
    void Start()
    {
        xspeed = 0.6f;
        yspeed = 0.15f;
        moveSpeed = 0.1f;
        transform.eulerAngles = v3Current;
        Jump();
    }
    private void Jump()
    {
        StartCoroutine(RotateTo(v3Current, rotation0target0));
        StartCoroutine(TranslateTo(transform.position, positionTarget));
        //yield returjn RotateTo(v3Current, rotation0target1);
    }

    IEnumerator RotateTo(Vector3 from, Vector3 to)
    {
        float t0;
        float t1 = 0;
        for (t0 = 0f; t0 < 1f; t0 += yspeed * Time.deltaTime)
        {
            v3Current = new Vector3(Mathf.Lerp(from.x, to.x, t1), Mathf.Lerp(from.y, to.y, t0), 0);
            transform.eulerAngles = v3Current;
            t1 += xspeed * Time.deltaTime;
            if (v3Current.x == -180)
                yspeed = 2f;
            yield return null;
        }
        transform.eulerAngles = to;
        v3Current = to;
    }
    IEnumerator TranslateTo(Vector3 from, Vector3 to)
    {
        for (float t = 0f; t < 1f; t += moveSpeed * Time.deltaTime)
        {
            transform.position = Vector3.Lerp(from, to, t);
            if (v3Current.x == -180)
                moveSpeed = 4f;
            yield return null;
        }
        transform.position = to;
    }
}
