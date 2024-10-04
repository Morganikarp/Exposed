using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject Target;
    Vector3 targetPos;
    Vector3 Velo = Vector3.zero;
    float camSpeed = .1f;

    void Update()
    {
        targetPos = Target.GetComponent<Transform>().position;

        if (Input.GetMouseButton(1))
        {
            Vector3 lookDir = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f) - targetPos;
            lookDir /= Vector3.Magnitude(lookDir);
            targetPos += lookDir * 4;
        }

    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, targetPos.x, camSpeed), Mathf.Lerp(transform.position.y, targetPos.y, camSpeed), -10f);
    }
}
