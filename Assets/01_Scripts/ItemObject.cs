using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
	public ItemScriptable itemDescription;

	//Launch when object in inventory is used
	public virtual void Use(CharacterTarget usingPlayer)
	{

	}
}
