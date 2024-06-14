using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DropArea : QuestInteractable
{
    int currentObject;
    int previousObject;
    bool plankDeactivated;

    private void Update()
    {
        if (linkedQuest.questValidated)
        {
            RemoveThrowableClientRpc();
            return;
        }

        Collider[] sphereDrop = Physics.OverlapSphere(transform.position, 5f);

        currentObject = 0;
        foreach (Collider collider in sphereDrop)
        {
            if (collider.GetComponent<Throwable>() && collider.GetComponent<Throwable>().itemDescription.itemTag == "item.plank")
            {
                currentObject++;
                Debug.Log(collider.gameObject.name);
            }
        }
        if (currentObject != previousObject)
        {
            previousObject = currentObject;
            Debug.Log("new plank added");
			if (IsServer)
			{
                QuestSpawner.Instance.CheckQuestClientRpc(linkedQuest.questName, currentObject);
                RemoveThrowableClientRpc();
            }
        }
    }

    [ClientRpc]
	private void RemoveThrowableClientRpc()
	{
        Collider[] sphereDrop = Physics.OverlapSphere(transform.position, 5f);
        if (linkedQuest.questValidated)
        {
            if (!plankDeactivated)
            {
                foreach (Collider collider in sphereDrop)
                {
                    if (collider.GetComponent<Throwable>() && collider.GetComponent<Throwable>().itemDescription.itemTag == "item.plank")
                    {
                        Destroy(collider.GetComponent<Throwable>());
                    }
                }
                plankDeactivated = true;
            }
        }
    }
}
