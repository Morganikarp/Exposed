using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawnerController : MonoBehaviour
{
    public GameObject FishPrefab;
    public Sprite[] FishSkins;

    public float spawnRadius;

    public int threatLevel;
    public float spawnDelay;
    public float baseSize;
    public bool SpawnEnemy;
    public float currentSpawnDelay;

    void Start()
    {
        spawnRadius = 20f;
        threatLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ThreatStats();

        if (SpawnEnemy)
        {
            Spawn();
        }
    }

    void ThreatStats()
    {
        switch (threatLevel)
        {
            case 0:
                spawnDelay = 20f;
                baseSize = 2f;
                break;
            case 1:
                spawnDelay = 15f;
                baseSize = 4f;
                break;
            case 2:
                spawnDelay = 7.5f;
                baseSize = 5f;
                break;
            case 3:
                spawnDelay = 3f;
                baseSize = 6f;
                break;

        }
    }

    void Spawn()
    {
        GameObject Fish = Instantiate(FishPrefab) as GameObject;

        float fishRadian = Random.Range(0, 360) * (Mathf.PI / 180);
        Fish.transform.position = new(transform.position.x + spawnRadius * Mathf.Cos(fishRadian), transform.position.y + spawnRadius * Mathf.Sin(fishRadian), 0f);

        float fishSize = Random.Range(-1, 1) + baseSize;
        Fish.transform.localScale = new(fishSize, fishSize, 1);

        Fish.GetComponent<FishController>().Aggression = threatLevel;

        Fish.GetComponent<SpriteRenderer>().sprite = FishSkins[Random.Range(0, FishSkins.Length)];
        StartCoroutine("DelayTime");
    }

    IEnumerator DelayTime()
    {
        SpawnEnemy = false;
        currentSpawnDelay = Random.Range(-1.5f, 1.5f) + spawnDelay;
        yield return new WaitForSeconds(currentSpawnDelay);
        SpawnEnemy = true;
    }
}
