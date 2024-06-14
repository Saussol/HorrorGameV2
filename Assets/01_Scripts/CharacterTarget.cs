using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Unity.Burst.CompilerServices;
using Unity.Netcode;

public class CharacterTarget : NetworkBehaviour
{
	private Transform playerCamera;
	private PlayerInventory playerInventory;
	private CharacterMovement characterMovement;
	[SerializeField] private int playerReach;
	private HUDManager hUDManager;

	private QuestInteractable currentQuestInteraction;

	private void Start()
	{
		playerCamera = GetComponentInChildren<Camera>().transform;
		playerInventory = GetComponentInChildren<PlayerInventory>();
		characterMovement = GetComponent<CharacterMovement>();
		hUDManager = GetComponentInChildren<HUDManager>();
	}

	private void Update()
	{
		if (!IsOwner) return;
		if (characterMovement.GetPlayerState() == PlayerState.RAT) return;

		RaycastHit hit;

		if(Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, playerReach))
		{
			if (hit.transform.GetComponent<ItemObject>())
			{
				hUDManager.DiplayIndication($"pick up {hit.transform.GetComponent<ItemObject>().itemDescription.itemName}");
				if (Input.GetKeyDown(KeyCode.E))
				{
					if (playerInventory.AddItem(hit.transform.GetComponent<ItemObject>().itemDescription))
					{
						hit.transform.GetComponent<ItemObject>().DestroyObjectServerRpc();
                    }

				}
			}
			else if (hit.transform.GetComponent<QuestInteractable>() && hit.transform.GetComponent<QuestInteractable>().canInteract)
			{
				currentQuestInteraction = hit.transform.GetComponent<QuestInteractable>();

				if (hit.transform.GetComponent<HoldInteract>() && !hit.transform.GetComponent<HoldInteract>().iAmFilling && hit.transform.GetComponent<HoldInteract>().GetFillState()) return;

				hUDManager.DiplayIndication(currentQuestInteraction.interactText);
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
			{
				if(hUDManager != null)
					hUDManager.HideIndication();
			}
		}
		else
		{
			if (hUDManager != null)
				hUDManager.HideIndication();

			if (currentQuestInteraction != null && currentQuestInteraction.GetComponent<HoldInteract>() &&
				currentQuestInteraction.GetComponent<HoldInteract>().GetFillState() && !currentQuestInteraction.GetComponent<HoldInteract>().iAmFilling) return;

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
