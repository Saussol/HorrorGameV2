using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum ItemType
{
	DROPABLE,
	USABLE
}

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Item", order = 1)]
public class ItemScriptable : ScriptableObject
{
	[Title("Your Item")]
	public string itemName;
	public string itemTag;
	public ItemType itemType;
	[PreviewField]
	public Sprite itemSprite;
	[PreviewField]
	public GameObject itemPrefab;
}
