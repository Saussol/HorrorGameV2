using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchQuest : QuestInteractable
{
    public GameObject fireVFX;
    public Transform vfxSpawnPoint;

	private void Start()
	{
		QuestSpawner.Instance._pickUpFire.AddListener(() =>
		{
			canInteract = true;
		});
	}

	public override void Interact()
    {
		base.Interact();

        Instantiate(fireVFX, vfxSpawnPoint.position, vfxSpawnPoint.rotation);

		canInteract = false;
    }
}
