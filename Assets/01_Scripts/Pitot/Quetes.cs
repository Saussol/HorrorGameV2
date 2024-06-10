using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Quetes : MonoBehaviour
{
    [SerializeField] public List<Transform> questsPos = new List<Transform>();
    [SerializeField] public List<GameObject> quests = new List<GameObject>();
    [SerializeField] public List<Transform> bottlePos = new List<Transform>();
    [SerializeField] public GameObject bottlePrefab;
    [SerializeField] int bottleNumber;

    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();

    void Start()
    {
        Quests();
    }

    private void Quests()
    {
        List<Transform> availablePos = new List<Transform>(questsPos);
        List<int> usedIndexes = new List<int>();

        foreach (GameObject quest in quests)
        {
            int randomIndex;
            Transform spawnTransform;

            do
            {
                randomIndex = Random.Range(0, availablePos.Count);
            } while (usedIndexes.Contains(randomIndex));

            spawnTransform = availablePos[randomIndex];
            usedIndexes.Add(randomIndex);

            Instantiate(quest, spawnTransform.position, Quaternion.identity);
        }
    }
}
