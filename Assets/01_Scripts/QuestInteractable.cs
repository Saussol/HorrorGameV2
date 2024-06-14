using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class QuestInteractable : NetworkBehaviour
{
	[HideInInspector] public bool canInteract;
	public bool needToHold;
	public string interactText;

	public Quest linkedQuest;

	public virtual void Interact()
	{
		if (!canInteract) return;

		Debug.Log($"Interacted with {gameObject.name}");
	}

	public virtual void StopInteract()
	{
		Debug.Log($"Stopped interacting with {gameObject.name}");
	}

	public virtual void ValidateQuest()
	{
		Debug.Log($"Quest validated {linkedQuest.questName}");
	}
}
