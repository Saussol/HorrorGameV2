using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInGame : NetworkBehaviour
{
    public void exitGame()
    {        
        Application.Quit();
    }

    public Scene scene;

    public void HostGoMainMenu()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.SceneManager.UnloadScene(scene);

            NetworkManager.Singleton.SceneManager.LoadScene("LobbytestScene", LoadSceneMode.Single);

            //NetworkManager.Singleton.SceneManager.UnloadScene(scene);

            //if (NetworkManager.Singleton == null) return;

            //NetworkManager.Singleton.Shutdown();
        }
    }

    // Méthode RPC pour informer les clients de charger la scène de lobby
    [ClientRpc]
    private void SetLoadingScreenForLobbyClientRpc()
    {
        
    }

    public void ReturnToLobbyButton()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            SetLoadingScreenForLobbyClientRpc();
            ReturnToLobby();
        }
        else
        {
            // Pour les clients, simplement charger la scène de lobby
            NetworkManager.Singleton.SceneManager.LoadScene("LobbyScene", LoadSceneMode.Single);
        }
    }

    private void ReturnToLobby()
    {
        StartCoroutine(ReturnWithLoading());
    }

    private IEnumerator ReturnWithLoading()
    {
        //SetLoadingScreenClientRpc();
        yield return new WaitForSeconds(.5f);
        NetworkManager.Singleton.SceneManager.LoadScene("LobbyScene", LoadSceneMode.Single);
    }
}
