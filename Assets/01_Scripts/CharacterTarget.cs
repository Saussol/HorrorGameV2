using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Unity.Burst.CompilerServices;

public class CharacterTarget : MonoBehaviour
{
	private Transform playerCamera;
	private PlayerInventory playerInventory;
	private CharacterMovement characterMovement;
	[SerializeField] private int playerReach;

	private QuestInteractable currentQuestInteraction;

	private void Start()
	{
		playerCamera = GetComponentInChildren<Camera>().transform;
		playerInventory = GetComponentInChildren<PlayerInventory>();
		characterMovement = GetComponent<CharacterMovement>();
	}

	private void Update()
	{
		if (characterMovement.GetPlayerState() == PlayerState.RAT) return;

		RaycastHit hit;

		if(Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, playerReach))
		{
			if (hit.transform.GetComponent<ItemObject>())
			{
				HUDManager.Instance.DiplayIndication($"pick up {hit.transform.GetComponent<ItemObject>().itemDescription.itemName}");
				if (Input.GetKeyDown(KeyCode.E))
				{
					if (playerInventory.AddItem(hit.transform.GetComponent<ItemObject>().itemDescription))
					{
                        Destroy(hit.transform.gameObject);

						//CmdDestoy(hit.transform.gameObject);
                    }

				}
			}
			else if (hit.transform.GetComponent<QuestInteractable>() && hit.transform.GetComponent<QuestInteractable>().canInteract)
			{
				currentQuestInteraction = hit.transform.GetComponent<QuestInteractable>();
				HUDManager.Instance.DiplayIndication(currentQuestInteraction.interactText);
				if (Input.GetKeyDown(KeyCode.E))
				{
					currentQuestInteraction.Interact();
				}
				if (currentQuestInteraction.needToHold && Input.GetKeyUp(KeyCode.E))
				{
					currentQuestInteraction.StopInteract();
				}
			}
			else
				HUDManager.Instance.HideIndication();
		}
		else
		{
			HUDManager.Instance.HideIndication();

			if (currentQuestInteraction != null && currentQuestInteraction.needToHold)
			{
				currentQuestInteraction.StopInteract();
				currentQuestInteraction = null;
			}
		}
	}


	//[Command]
 //   private void CmdDestoy(GameObject obj)
 //   {
 //       if (!isLocalPlayer)
	//	{
 //           Destroy(obj);
 //       }

	//	RpcDestroy(obj);
 //   }

	//[ClientRpc]
 //   private void RpcDestroy(GameObject obj)
 //   {
 //       if (!isLocalPlayer)
 //       {
 //           Destroy(obj);
 //       }
 //   }
}
