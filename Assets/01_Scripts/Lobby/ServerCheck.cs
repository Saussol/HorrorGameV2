using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerCheck : NetworkBehaviour
{
    void Start()
    {
        //CheckAndDisconnectServer();
    }

    private void CheckAndDisconnectServer()
    {
        if (NetworkManager.Singleton != null)
        {
            if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
            {
                Debug.Log("Server is running, disconnecting...");
                NetworkManager.Singleton.Shutdown();

                StartCoroutine(RealoadScene());
                
            }
            else
            {
                Debug.Log("No server is running.");
            }
        }
        else
        {
            Debug.LogError("NetworkManager.Singleton is null. Please ensure NetworkManager is properly set up.");
        }
    }

    public IEnumerator RealoadScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("LobbytestScene");
    } 
}
