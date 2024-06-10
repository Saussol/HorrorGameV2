using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInteractable : MonoBehaviour
{
	[HideInInspector] public bool canInteract;
	public bool needToHold;
	public string interactText;

    public virtual void Interact()
	{
		if (!canInteract) return;

		Debug.Log($"Interacted with {gameObject.name}");
	}

	public virtual void StopInteract()
	{
		Debug.Log($"Stopped interacting with {gameObject.name}");
	}
}
