using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public ItemScriptable[] itemPrefabs;

    //Singleton
    public static PrefabManager Instance { get; private set; }

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
    }

    public GameObject GetPrefabByTag(string itemTag)
	{
		foreach (var item in itemPrefabs)
		{
            if(item.itemTag == itemTag)
			{
                return item.itemPrefab;
			}
		}
        return null;
	}
}
