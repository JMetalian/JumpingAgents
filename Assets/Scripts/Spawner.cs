using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnableObjects;
    [Tooltip("Waiting amount for spawner.")]
    [SerializeField] private float minSpawningInterval;
    [SerializeField] private float maxSpawningInterval;

    private Jumper agent;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    private void Awake()
    {
        agent = GetComponentInChildren<Jumper>();
        agent.OnReset += DestroyAllSpawnedObjects; 
        StartCoroutine(nameof(Spawn));
    }
    private IEnumerator Spawn()
    {
        var obstacleSpawned = Instantiate(GetRandomSpawnableFromList(), transform.position, transform.rotation, transform);
        spawnedObjects.Add(obstacleSpawned);
        yield return new WaitForSeconds(Random.Range(minSpawningInterval, maxSpawningInterval));
        StartCoroutine(nameof(Spawn));
    }
    private GameObject GetRandomSpawnableFromList()
    {
        int randomIndex = UnityEngine.Random.Range(0, spawnableObjects.Count);
        return spawnableObjects[randomIndex];
    }
    private void DestroyAllSpawnedObjects()
    {
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            Destroy(spawnedObjects[i]);
            spawnedObjects.RemoveAt(i);
        }
    }
}
