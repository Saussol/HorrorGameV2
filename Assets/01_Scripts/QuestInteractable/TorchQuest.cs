using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TorchQuest : QuestInteractable
{
    public GameObject fireVFX;
    public Transform vfxSpawnPoint;
	private bool CanFire = true;


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

		if (CanFire)
		{
			GameObject vfxInstance = Instantiate(fireVFX, vfxSpawnPoint.position, vfxSpawnPoint.rotation);
			vfxInstance.GetComponent<NetworkObject>().Spawn(true);
			vfxInstance.transform.Rotate(90, 0, 0);
            CanFire = false;
        }

		QuestSpawner.Instance.CheckQuestClientRpc(linkedQuest.questName);
        //linkedQuest.CheckQuest();

		canInteract = false;
    }
}
