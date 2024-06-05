using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class PlayerInventory : MonoBehaviour
{
	[SerializeField] private Transform inventoryPanel;
	[SerializeField] private Color selected, unselected;
	[SerializeField] private TMP_Text itemText;
	[SerializeField] private GameObject objectInHand;
    private InventorySlot[] inventorySlots;
	private InventorySlot selectedSlot;
	private int slotIndex;

	private CharacterMovement characterMovement;

	public GameObject[] prefabs;

	private void Start()
    {
		characterMovement = GetComponent<CharacterMovement>();
		characterMovement._onRatTransformation.AddListener(HideInventory);

		inventorySlots = inventoryPanel.GetComponentsInChildren<InventorySlot>();
		SetSelectedSlot(0);
    }

	private void Update()
	{
		if (characterMovement.GetPlayerState() == PlayerState.RAT) return;

		if (Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			slotIndex = (slotIndex + 1) >= inventorySlots.Length ? 0 : slotIndex + 1;
			SetSelectedSlot(slotIndex);
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			slotIndex = (slotIndex - 1) < 0 ? inventorySlots.Length - 1 : slotIndex - 1;
			SetSelectedSlot(slotIndex);
		}

		if(Input.GetMouseButtonDown(0) && selectedSlot.itemDescription != null)
		{
			selectedSlot.UseItem();
		}
	}

	public bool AddItem(ItemScriptable itemDescription)
	{
		for (int i = inventorySlots.Length - 1; i >= 0; i--)
		{
			if (inventorySlots[i].itemDescription != null && inventorySlots[i].itemDescription.itemTag == itemDescription.itemTag && inventorySlots[i].GetItemNumber() < 100)
			{
				inventorySlots[i].AddItemNumber(1);
				return true;
			}
		}
		for (int i = inventorySlots.Length - 1; i >= 0; i--)
		{
			if (inventorySlots[i].itemDescription == null)
			{
				inventorySlots[i].SetItem(itemDescription);
				if (i == slotIndex)
				{
					StopAllCoroutines();
					StartCoroutine(ShowItemName(selectedSlot.itemDescription.itemName));

					if (itemDescription.itemPrefab != null)
					{
						objectInHand.gameObject.SetActive(true);

						objectInHand.GetComponent<MeshFilter>().mesh = selectedSlot.itemDescription.itemPrefab.GetComponent<MeshFilter>().sharedMesh;
						objectInHand.GetComponent<MeshRenderer>().material = selectedSlot.itemDescription.itemPrefab.GetComponent<MeshRenderer>().sharedMaterial;
					}
					else
					{
						objectInHand.gameObject.SetActive(false);
					}
				}

				return true;
			}
		}

		return false;
	}

	private void SetSelectedSlot(int slotIndex)
	{
		for (int i = 0; i < inventorySlots.Length; i++)
		{
			if (i == slotIndex)
			{
				selectedSlot = inventorySlots[i];
				selectedSlot.transform.GetComponent<Image>().color = selected;
			}
			else
				inventorySlots[i].transform.GetComponent<Image>().color = unselected;
		}
		if(selectedSlot.itemDescription != null)
		{
			StopAllCoroutines();
			StartCoroutine(ShowItemName(selectedSlot.itemDescription.itemName));
			if (selectedSlot.itemDescription.itemPrefab)
			{
				objectInHand.gameObject.SetActive(true);

				objectInHand.GetComponent<MeshFilter>().mesh = selectedSlot.itemDescription.itemPrefab.GetComponent<MeshFilter>().sharedMesh;
				objectInHand.GetComponent<MeshRenderer>().material = selectedSlot.itemDescription.itemPrefab.GetComponent<MeshRenderer>().sharedMaterial;
			}
			else
			{
				objectInHand.gameObject.SetActive(false);
			}
		}
		else
		{
			objectInHand.gameObject.SetActive(false);
		}
	}

	private IEnumerator ShowItemName(string itemName)
	{
		itemText.text = itemName;
		itemText.gameObject.SetActive(true);
		itemText.color = Color.white;

		yield return new WaitForSeconds(1f);

		while(itemText.color.a > 0)
		{
			itemText.color -= new Color(0, 0, 0, .01f);
			yield return null;
		}

		itemText.gameObject.SetActive(false);
		itemText.color = Color.white - new Color(0, 0, 0, 1f);
	}

	private void HideInventory()
	{
		foreach (InventorySlot slot in inventorySlots)
		{
			slot.AddItemNumber(-slot.GetItemNumber());
		}
		inventoryPanel.gameObject.SetActive(false);
	}

	public void HideHandItem()
	{
		objectInHand.SetActive(false);
	}
}
