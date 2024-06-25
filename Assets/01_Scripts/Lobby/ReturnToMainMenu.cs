using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : NetworkBehaviour
{
    public float checkInterval = 5f; // Intervalle de vérification en secondes

    void Start()
    {
        // Démarrer la vérification régulière des caméras
        StartCoroutine(CheckCameraStatus());
    }

    IEnumerator CheckCameraStatus()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);
            // Vérifiez le nombre de caméras dans la scène
            if (Camera.allCamerasCount == 0)
            {
                LoadMainMenu();
            }
        }
    }   

    void LoadMainMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene("LobbytestScene");
    }
}
