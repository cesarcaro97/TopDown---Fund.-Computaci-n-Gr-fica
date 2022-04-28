using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private bool spawnOnStart = true;
    [SerializeField] private GameObject prefab = null;

    private void Start()
    {
        if (spawnOnStart)
            Spawn();
    }

    public void Spawn()
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
