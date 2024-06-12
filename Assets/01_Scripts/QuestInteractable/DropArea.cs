using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropArea : QuestInteractable
{
    int currentObject;
    int previousObject;

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
            linkedQuest.CheckQuest(currentObject);
            if (linkedQuest.questValidated)
            {
                foreach (Collider collider in sphereDrop)
                {
                    if (collider.GetComponent<Throwable>() && collider.GetComponent<Throwable>().itemDescription.itemTag == "item.plank")
                    {
                        Destroy(collider.GetComponent<Throwable>());
                    }
                }
            }
        }
    }
}
