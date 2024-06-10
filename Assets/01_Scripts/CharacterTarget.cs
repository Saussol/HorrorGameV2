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
				HUDManager.Instance.DiplayIndication(hit.transform.GetComponent<QuestInteractable>().interactText);
				if (Input.GetKeyDown(KeyCode.E))
				{
					hit.transform.GetComponent<QuestInteractable>().Interact();
				}
			}
			else
				HUDManager.Instance.HideIndication();
		}
		else
			HUDManager.Instance.HideIndication();
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
