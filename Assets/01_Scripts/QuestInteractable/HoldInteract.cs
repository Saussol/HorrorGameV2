using UnityEngine;
using UnityEngine.UI;

public class HoldInteract : QuestInteractable
{
    public Slider progressBar; // Référence au Slider UI
    public float fillSpeed = 0.5f; // Vitesse de remplissage de la barre

    private bool isFilling = false;
    private bool questEnded = false;

    private void Awake()
    {
        canInteract = true;
    }

    public override void Interact()
	{
		base.Interact();
        isFilling = true;
	}

	public override void StopInteract()
	{
		base.StopInteract();
        isFilling = false;

        if(!questEnded)
            progressBar.value = 0;
    }

    void Update()
    {
        if (questEnded) return;

        if (isFilling)
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

        if (!isFilling && !questEnded)
        {
            progressBar.value = 0;
        }
    }
}
