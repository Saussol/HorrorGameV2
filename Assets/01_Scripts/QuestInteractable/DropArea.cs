using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropArea : QuestInteractable
{
    int currentObject;
    int previousObject;
    bool plankDeactivated;

    private void Update()
    {
        if (linkedQuest.questValidated)
        {
            if (!plankDeactivated)
            {
                Collider[] sphere = Physics.OverlapSphere(transform.position, 5f);
                foreach (Collider collider in sphere)
                {
                    if (collider.GetComponent<Throwable>() && collider.GetComponent<Throwable>().itemDescription.itemTag == "item.plank")
                    {
                        Destroy(collider.GetComponent<Throwable>());
                    }
                }
                plankDeactivated = true;
            }
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
            linkedQuest.CheckQuestClientRpc(currentObject);
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
}
