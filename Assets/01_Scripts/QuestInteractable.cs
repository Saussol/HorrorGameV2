using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInteractable : MonoBehaviour
{
	[HideInInspector] public bool canInteract;
	public string interactText;

    public virtual void Interact()
	{
		if (!canInteract) return;

		Debug.Log($"Interacted with {gameObject.name}");
	}
}
