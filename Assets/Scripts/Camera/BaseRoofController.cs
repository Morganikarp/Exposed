using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRoofController : MonoBehaviour
{
    GameObject roof;
    SpriteMask mask;

    public bool Inside;

    void Start()
    {
        roof = transform.GetChild(0).gameObject;
        mask = GetComponent<SpriteMask>();

        Inside = true;
    }

    // Update is called once per frame
    void Update()
    {
        roof.SetActive(!Inside);
        mask.enabled = Inside;
    }
}
