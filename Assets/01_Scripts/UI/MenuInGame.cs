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

    public void HostGoMainMenu()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("LobbytestScene", LoadSceneMode.Single);

            if (NetworkManager.Singleton == null) return;

            NetworkManager.Singleton.Shutdown();
        }
    }

    


}
