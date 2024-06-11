using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : QuestInteractable
{
	//public bool hasFire = false;

	private void Awake()
	{
		canInteract = true;
	}

	public override void Interact()
	{
		base.Interact();

        QuestSpawner.Instance._pickUpFire.Invoke();

        canInteract = false;
    }
}
