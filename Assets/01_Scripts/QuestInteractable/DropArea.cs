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
            }
        }
    }

	public override void ValidateQuest()
	{
		base.ValidateQuest();
        Collider[] sphereDrop = Physics.OverlapSphere(transform.position, 5f);
        foreach (Collider collider in sphereDrop)
        {
            if (collider.GetComponent<Throwable>() && collider.GetComponent<Throwable>().itemDescription.itemTag == "item.plank")
            {
                Destroy(collider.GetComponent<Throwable>());
            }
        }
    }
}
