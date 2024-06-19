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

	private void Start()
	{
		QuestSpawner.Instance._restoreFire.AddListener(() =>
		{
			RestoreFireServerRpc();
		});
	}

	public override void Interact()
	{
		base.Interact();

        QuestSpawner.Instance._pickUpFire.Invoke();
		QuestSpawner.Instance.firePickedUp = true;

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

	[ServerRpc(RequireOwnership = false)]
	private void RestoreFireServerRpc()
	{
		RestoreFireClientRpc();
	}

	[ClientRpc]
	private void RestoreFireClientRpc()
	{
		canInteract = true;
	}
}
