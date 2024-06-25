using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : NetworkBehaviour
{
    public float checkInterval = 5f; // Intervalle de v�rification en secondes

    void Start()
    {
        // D�marrer la v�rification r�guli�re des cam�ras
        StartCoroutine(CheckCameraStatus());
    }

    IEnumerator CheckCameraStatus()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);
            // V�rifiez le nombre de cam�ras dans la sc�ne
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
