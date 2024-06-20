using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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

		AddItemNumber(1);
	}

	public void DropAllItem()
	{
		for (int i = 0; i < itemNumber; i++)
		{
			Vector3 instantiatePos = Camera.main.transform.position + new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
			Quaternion instantiateRot = Camera.main.transform.parent.transform.rotation;

			SpawnObjectServerRpc(instantiatePos, instantiateRot, Vector3.up, Vector3.zero, itemDescription.itemTag);
		}
	}

	public void UseItem()
	{
		if (!IsOwner) return;
		if (itemDescription == null) return;

		Vector3 instantiatePos = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
		Quaternion instantiateRot = Camera.main.transform.parent.transform.rotation;
		CharacterTarget usingPlayer = GetComponentInParent<CharacterTarget>();
		Vector3 throwDirection = usingPlayer.GetComponentInChildren<Camera>().transform.forward * 12 + Vector3.up * 3;
		Vector3 velocity = usingPlayer.transform.GetComponent<CharacterController>().velocity;

		SpawnObjectServerRpc(instantiatePos, instantiateRot, throwDirection, velocity, itemDescription.itemTag);

		AddItemNumber(-1);
	}

	[ServerRpc]
	private void SpawnObjectServerRpc(Vector3 instantiatePos, Quaternion instantiateRot, Vector3 throwDirection, Vector3 velocity, string prefabTag)
	{
		GameObject item = Instantiate(PrefabManager.Instance.GetPrefabByTag(prefabTag), instantiatePos, instantiateRot);
		item.GetComponent<NetworkObject>().Spawn(true);
		item.GetComponent<ItemObject>().Use(throwDirection, velocity);
	}

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
