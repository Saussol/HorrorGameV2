using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class HoldInteract : QuestInteractable
{
    public Slider progressBar; // Référence au Slider UI
    public float fillSpeed = 0.5f; // Vitesse de remplissage de la barre

    private NetworkVariable<bool> isFilling = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private bool questEnded = false;
    public bool iAmFilling = false;

    public bool GetFillState()
	{
        return isFilling.Value;
	}

    private void Awake()
    {
        canInteract = true;
    }

	public override void Interact()
	{
		base.Interact();
        iAmFilling = true;
        ChangeIsFillingServerRpc(true);
		//isFilling.Value = true;
	}

	public override void StopInteract()
	{
		base.StopInteract();
        iAmFilling = false;
        ChangeIsFillingServerRpc(false);
        //isFilling.Value = false;

        if(!questEnded)
            progressBar.value = 0;
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeIsFillingServerRpc(bool filling)
	{
        isFilling.Value = filling;
	}

    void Update()
    {
        if (questEnded) return;

        if (isFilling.Value)
        {
            progressBar.value += fillSpeed * Time.deltaTime;
        }

        if (progressBar.value >= progressBar.maxValue)
        {
            questEnded = true;
            linkedQuest.CheckQuest();
            progressBar.value = progressBar.maxValue;
            canInteract = false;
            Debug.Log("Quest completed !");
        }

        if (!isFilling.Value && !questEnded)
        {
            progressBar.value = 0;
        }
    }
}
