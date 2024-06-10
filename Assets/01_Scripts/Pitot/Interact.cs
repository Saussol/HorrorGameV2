using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    public Slider progressBar; // Référence au Slider UI
    public float fillSpeed = 0.5f; // Vitesse de remplissage de la barre

    private bool isPlayerInRange = false;
    private bool isFilling = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKey(KeyCode.E))
        {
            isFilling = true;
        }
        else
        {
            isFilling = false;
        }

        if (isFilling)
        {
            progressBar.value += fillSpeed * Time.deltaTime;
            if (progressBar.value >= progressBar.maxValue)
            {
                Debug.Log("Quest completed !");
            }
        }
        else if (!isFilling && progressBar.value < progressBar.maxValue)
        {
            progressBar.value = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            isFilling = false;
            progressBar.value = 0; // Réinitialiser la barre si le joueur sort de la zone
        }
    }
}
