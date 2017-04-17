using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBlock : MonoBehaviour
{

    public GameObject buildAreaPrefab;

    void Start()
    {
        if (gameObject.name == "SpawnFighter(Clone)")
            Instantiate(buildAreaPrefab, transform, false);
    }

    void Update()
    {
    }
}