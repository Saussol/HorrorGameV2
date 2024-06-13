using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ItemObject : NetworkBehaviour
{
	public ItemScriptable itemDescription;

	//Launch when object in inventory is used
	public virtual void Use(CharacterTarget usingPlayer)
	{

	}

	[ServerRpc(RequireOwnership = false)]
	public void DestroyObjectServerRpc()
	{
		GetComponent<NetworkObject>().Despawn(true);
	}
}
