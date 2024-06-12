using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class QuestSpawner : MonoBehaviour
{
    public List<Transform> questsSpawnPoints;
    //public List<GameObject> questPrefabs;
    public List<Quest> quests;

    public List<Transform> bottleSpawnPoints;

    [SerializeField] private int bottleNumber;
    [SerializeField] private GameObject[] bottlePrefabs;
    [SerializeField] private int materialNumber;
    [SerializeField] private GameObject materialPrefab;

    [SerializeField] private GameObject questUIPrefab;
    [SerializeField] private Transform questUIParent;
    public Color questDoneColor;

    public bool allQuestEnded;

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
        SpawnQuestObjects();
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

            if (quest.questPrefab != null)
            {
                GameObject questObject = Instantiate(quest.questPrefab, questsSpawnPoints[spawnIndex].position, questsSpawnPoints[spawnIndex].rotation);
                questObject.GetComponent<QuestInteractable>().linkedQuest = quest;
            }
            //GameObject questObject = Instantiate(quest.questPrefab, questsSpawnPoints[spawnIndex].position, questsSpawnPoints[spawnIndex].rotation);
            GameObject questUI = Instantiate(questUIPrefab, questUIParent);

            //questObject.GetComponent<QuestInteractable>().linkedQuest = quest;
            quest.questUI = questUI;

            questUI.transform.GetChild(0).GetComponent<TMP_Text>().text = quest.questName;
            if(quest.questLength > 1)
                questUI.transform.GetChild(1).gameObject.SetActive(true);
			else
                questUI.transform.GetChild(1).gameObject.SetActive(false);

            if (quest.questObjects.Count > 0)
			{
				foreach (QuestInteractable obj in quest.questObjects)
				{
                    obj.linkedQuest = quest;
				}
			}

            availableSpawnIndices.RemoveAt(randomIndex);
        }
    }

    private void SpawnQuestObjects()
    {
        if(materialNumber + bottleNumber > bottleSpawnPoints.Count)
		{
            bottleNumber = bottleSpawnPoints.Count - materialNumber;
		}

        List<Transform> availableSpawnPoints = new List<Transform>(bottleSpawnPoints);


        for (int i = 0; i < materialNumber; i++)
		{
            Transform spawnPos = availableSpawnPoints[Random.Range(0, availableSpawnPoints.Count)];
            GameObject material = Instantiate(materialPrefab, spawnPos.position, Quaternion.identity);
            availableSpawnPoints.Remove(spawnPos);
		}
		for (int i = 0; i < bottleNumber; i++)
		{
            Transform spawnPos = availableSpawnPoints[Random.Range(0, availableSpawnPoints.Count)];
            GameObject bottle = Instantiate(bottlePrefabs[Random.Range(0, bottlePrefabs.Length)], spawnPos.position, Quaternion.identity);
            availableSpawnPoints.Remove(spawnPos);
        }
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
            allQuestEnded = true;
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
    public GameObject questUI;

    public void CheckQuest()
	{
        if (questValidated) return;

        questCurrentStatus++;

        if(questLength > 1)
		{
            questUI.transform.GetChild(1).GetComponent<Slider>().value = (float)questCurrentStatus / (float)questLength;
		}

        if(questCurrentStatus >= questLength)
		{
            questUI.transform.GetChild(0).GetComponent<TMP_Text>().color = QuestSpawner.Instance.questDoneColor;
            questUI.transform.GetChild(1).GetComponent<Slider>().value = 1;
            questValidated = true;
            QuestSpawner.Instance.CheckQuest();
		}
	}
    public void CheckQuest(int currentValue)
    {
        if (questValidated) return;

        questCurrentStatus = currentValue;

        if (questLength > 1)
        {
            questUI.transform.GetChild(1).GetComponent<Slider>().value = (float)questCurrentStatus / (float)questLength;
        }

        if (questCurrentStatus >= questLength)
        {
            questUI.transform.GetChild(0).GetComponent<TMP_Text>().color = QuestSpawner.Instance.questDoneColor;
            questUI.transform.GetChild(1).GetComponent<Slider>().value = 1;
            questValidated = true;
            QuestSpawner.Instance.CheckQuest();
        }
    }
}
