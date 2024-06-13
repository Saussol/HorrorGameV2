using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class PlayerConnectionManager : NetworkBehaviour
{
    private NetworkVariable<int> playersReady = new NetworkVariable<int>(0);

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            playersReady.Value = 0;
        }
    }

    private void OnEnable()
    {
        NetworkManager.Singleton.SceneManager.OnLoadComplete += OnSceneLoadCompleted;
    }

    private void OnDisable()
    {
        NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnSceneLoadCompleted;
    }

    private void OnSceneLoadCompleted(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        if (sceneName == "MultiScene 2")
        {
            if (IsClient)
            {
                PlayerLoadedSceneServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerLoadedSceneServerRpc(ServerRpcParams rpcParams = default)
    {
        playersReady.Value++;
        CheckAllPlayersReady();
    }

    private void CheckAllPlayersReady()
    {
        if (playersReady.Value >= NetworkManager.Singleton.ConnectedClientsList.Count)
        {
            Debug.Log("All players have joined the scene!");
            // Perform actions when all players are ready, e.g., start the game
            QuestSpawner.Instance.SpawnAll();
        }
    }
}
