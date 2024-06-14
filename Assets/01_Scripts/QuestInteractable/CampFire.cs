using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CampFire : QuestInteractable
{
	private void Awake()
	{
		canInteract = true;
	}

	public override void Interact()
	{
		base.Interact();

        QuestSpawner.Instance._pickUpFire.Invoke();

		TakeFireServerRpc();
    }

	[ServerRpc(RequireOwnership = false)]
	private void TakeFireServerRpc()
	{
		TakeFireClientRpc();
	}

	[ClientRpc]
	private void TakeFireClientRpc()
	{
		canInteract = false;
	}
}
