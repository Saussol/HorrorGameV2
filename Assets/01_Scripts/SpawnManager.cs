using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;

    public Transform GetSpawnPoint()
	{
		return spawnPoints[Random.Range(0, spawnPoints.Length)];
	}
}
