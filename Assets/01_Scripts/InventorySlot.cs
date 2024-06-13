using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.Netcode;

public class InventorySlot : NetworkBehaviour
{
	private Image itemImage;
	private TMP_Text itemNumberText;
	private int itemNumber;
	public ItemScriptable itemDescription;

	private Camera playerCamera;
	private PlayerInventory playerInventory;

    [SerializeField] private GameObject currentPrefab;

    private void Start()
	{
		itemImage = transform.GetChild(0).GetComponent<Image>();
		itemNumberText = transform.GetComponentInChildren<TMP_Text>();
		itemImage.gameObject.SetActive(false);
		itemNumberText.gameObject.SetActive(false);

		playerInventory = transform.parent.parent.parent.GetComponentInParent<PlayerInventory>();
		playerCamera = playerInventory.GetComponentInChildren<Camera>();
    }

	public void SetItem(ItemScriptable itemScriptable)
	{
		if (!IsOwner) return;

		itemDescription = itemScriptable;
		itemImage.gameObject.SetActive(true);
		itemImage.sprite = itemDescription.itemSprite;

		//foreach (GameObject prefab in playerInventory.prefabs)
		//{
		//	if (prefab == itemDescription.itemPrefab)
		//		currentPrefab = prefab;
		//}
		//if (currentPrefab == null) currentPrefab = playerInventory.prefabs[0];

		AddItemNumber(1);
	}

	public void UseItem()
	{
		if (!IsOwner) return;
		if (itemDescription == null) return;

		Vector3 instantiatePos = Camera.main.transform.position + Camera.main.transform.forward;
		Quaternion instantiateRot = Camera.main.transform.parent.transform.rotation;
		CharacterTarget usingPlayer = FindObjectOfType<CharacterTarget>();
		Vector3 throwDirection = usingPlayer.GetComponentInChildren<Camera>().transform.forward * 12 + Vector3.up * 3;
		Vector3 velocity = usingPlayer.transform.GetComponent<CharacterController>().velocity;

		SpawnObjectServerRpc(instantiatePos, instantiateRot, throwDirection, velocity);

		//if (isLocalPlayer)
		//{
		//	Vector3 instantiatePos = playerCamera.transform.position + playerCamera.transform.forward;
		//	Quaternion instantiateRot = playerCamera.transform.parent.transform.rotation;
		//	GameObject item = Instantiate(currentPrefab, instantiatePos, instantiateRot);
		//	item.GetComponent<ItemObject>().Use(playerCamera.GetComponentInParent<CharacterTarget>());

		//  CmdSpawnCube();
		//}

		AddItemNumber(-1);
	}

	[ServerRpc(RequireOwnership = false)]
	private void SpawnObjectServerRpc(Vector3 instantiatePos, Quaternion instantiateRot, Vector3 throwDirection, Vector3 velocity)
	{
		GameObject item = Instantiate(itemDescription.itemPrefab, instantiatePos, instantiateRot);
		item.GetComponent<NetworkObject>().Spawn(true);
		item.GetComponent<ItemObject>().Use(throwDirection, velocity);
	}

	//[Command]
 //   void CmdSpawnCube()
 //   {
 //       if (!isLocalPlayer)
 //       {

 //       }

 //       RpcSpawnCube();
 //   }

 //   [ClientRpc]
 //   void RpcSpawnCube()
 //   {
 //       if (!isLocalPlayer)
 //       {
 //           Vector3 instantiatePos = playerCamera.transform.position + playerCamera.transform.forward;
 //           Quaternion instantiateRot = playerCamera.transform.parent.transform.rotation;
 //           GameObject item = Instantiate(currentPrefab, instantiatePos, instantiateRot);
 //           item.GetComponent<ItemObject>().Use(playerCamera.GetComponentInParent<CharacterTarget>());
 //       }
 //   }

    public void AddItemNumber(int value)
	{
		itemNumber += value;
		if(itemNumber >= 1)
		{
			itemNumberText.gameObject.SetActive(true);
			itemNumberText.text = itemNumber.ToString();
		}
		else
		{
			itemNumberText.gameObject.SetActive(false);
		}

		if (itemNumber <= 0)
		{
			itemDescription = null;
			itemImage.gameObject.SetActive(false);
			playerInventory.HideHandItem();
		}
	}
	public int GetItemNumber() { return itemNumber; }
}
