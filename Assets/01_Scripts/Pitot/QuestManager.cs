using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestSpawner : MonoBehaviour
{
    public List<Transform> questsSpawnPoints;
    //public List<GameObject> questPrefabs;
    public List<Quest> quests;

    public List <Transform> botlleSpawnPoints;
    public GameObject bottlePrefab;
    public int bottleQuantity;

    private int questDone = 0;

    [HideInInspector] public UnityEvent _pickUpFire;

    //Singleton
    public static QuestSpawner Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        if (_pickUpFire == null) _pickUpFire = new UnityEvent();
    }

    void Start()
    {
        SpawnQuests();
    }

    void SpawnQuests()
    {
        List<int> availableSpawnIndices = new List<int>();
        for (int i = 0; i < questsSpawnPoints.Count; i++)
        {
            availableSpawnIndices.Add(i);
        }

        foreach (var quest in quests)
        {
            int randomIndex = Random.Range(0, availableSpawnIndices.Count);
            int spawnIndex = availableSpawnIndices[randomIndex];

            GameObject questObject = Instantiate(quest.questPrefab, questsSpawnPoints[spawnIndex].position, questsSpawnPoints[spawnIndex].rotation);

            questObject.GetComponent<QuestInteractable>().linkedQuest = quest;

            if(quest.questObjects.Count > 0)
			{
				foreach (QuestInteractable obj in quest.questObjects)
				{
                    obj.linkedQuest = quest;
				}
			}

            availableSpawnIndices.RemoveAt(randomIndex);
        }
    }

    private void SpawnBottles()
    {

    }

    public void ValidateQuest()
	{

	}

    public void CheckQuest()
	{
        bool allDone = true;

		foreach (Quest quest in quests)
		{
            if (!quest.questValidated) allDone = false;
		}

		if (allDone)
		{
            Debug.Log("<color=green>YOU WIN</color>");
            //TO DO end game with victory status
		}
	}

/*private void Bottles()
    {
        if (botlleSpawnPoints.Count < questPrefabs.Count)
        {
            Debug.LogError("Il n'y a pas assez de points de spawn pour toutes les quêtes !");
            return;
        }

        List<int> availableSpawnIndices = new List<int>();
        for (int i = 0; i < botlleSpawnPoints.Count; i++)
        {
            availableSpawnIndices.Add(i);
        }

        foreach (var questPrefab in questPrefabs)
        {
            int randomIndex = Random.Range(0, availableSpawnIndices.Count);
            int spawnIndex = availableSpawnIndices[randomIndex];

            Instantiate(questPrefab, botlleSpawnPoints[spawnIndex].position, botlleSpawnPoints[spawnIndex].rotation);

            availableSpawnIndices.RemoveAt(randomIndex);
        }
    }*/
}

[System.Serializable]
public class Quest
{
    public string questName;
    public int questLength;
    private int questCurrentStatus;
    public GameObject questPrefab;
    public bool questValidated;
    public List<QuestInteractable> questObjects = new List<QuestInteractable>();

    public void CheckQuest()
	{
        if (questValidated) return;

        questCurrentStatus++;
        if(questCurrentStatus >= questLength)
		{
            questValidated = true;
            QuestSpawner.Instance.CheckQuest();
		}
	}
}
