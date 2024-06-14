using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CampFire : QuestInteractable
{
	public NetworkVariable<bool> fireTaken = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

	private void Awake()
	{
		canInteract = true;
	}

	public override void Interact()
	{
		base.Interact();

        QuestSpawner.Instance._pickUpFire.Invoke();

		TakeFireServerRpc();

        canInteract = false;
    }

	[ServerRpc(RequireOwnership = false)]
	private void TakeFireServerRpc()
	{
		fireTaken.Value = true;
	}
}
