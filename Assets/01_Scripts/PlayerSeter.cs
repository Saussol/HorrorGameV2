using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Netcode;
using System;

public class PlayerSeter : NetworkBehaviour
{
    public Camera playerCamera;
    public Camera playerHandCamera;
    public Camera playerMinimapCamera;
    public Canvas playerCanvas;
    public GameObject hudPanel, loadingPanel;
    public CharacterMovement playerControls; // Par exemple, un GameObject qui contient le script de contrôle du joueur
    public GameObject playerBody;
    public GameObject pirateModel, ratModel;

    private void Start()
    {
        DisablePlayerComponents();
    }

 //   private void OnEnable()
 //   {
 //       NetworkManager.Singleton.SceneManager.OnLoadComplete += OnSceneLoadCompleted;
 //   }

	//private void OnDisable()
 //   {
 //       NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnSceneLoadCompleted;
 //   }

    //private void OnSceneLoadCompleted(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    //{
    //    Debug.Log($"Scene {sceneName} loaded for client {clientId}");
    //    if (sceneName == "MultiScene 2")
    //    {
    //        // Call your custom function here
    //        ReSetPlayerComponents();
    //    }
    //}

    public void DisablePlayerComponents()
    {
        if (playerCamera != null)
        {
            playerCamera.enabled = false;
        }

        if (playerHandCamera != null)
        {
            playerHandCamera.enabled = false;
        }

        if (playerMinimapCamera != null)
        {
            playerMinimapCamera.enabled = false;
        }

        if (playerCanvas != null)
        {
            playerCanvas.enabled = false;
        }

        if (playerControls != null)
        {
            playerControls.enabled = false;
        }
    }

    public void SetWaitingPlayers()
	{
        if (playerCanvas != null && IsOwner)
        {
            playerCanvas.enabled = true;
            hudPanel.SetActive(false);
            loadingPanel.SetActive(true);
        }
    }

    [ClientRpc]
    public void ReSetPlayerComponentsClientRpc()
    {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		if (playerCamera != null && IsOwner)
        {
            playerCamera.enabled = true;
        }


        if (playerHandCamera != null && IsOwner)
        {
            playerHandCamera.enabled = true;
        }


        if (playerMinimapCamera != null && IsOwner)
        {
            playerMinimapCamera.enabled = true;
        }


        if (playerCanvas != null && IsOwner)
		{
			playerCanvas.enabled = true;
            hudPanel.SetActive(true);
            loadingPanel.SetActive(false);
        }


		if (playerControls != null && IsOwner)
        {
            playerControls.enabled = true;
        }

        if (playerBody != null && IsOwner)
        {
            playerBody.SetActive(false);
        }

        if (IsOwner)
        {
            // Find the SpawnManager in the scene
            SpawnManager spawnManager = FindObjectOfType<SpawnManager>();

            if (spawnManager != null)
            {
                Transform spawnPoint = spawnManager.GetSpawnPoint();
                if (spawnPoint != null)
                    StartCoroutine(playerControls.TeleportPlayer(spawnPoint.position, false));
                else
                    Debug.LogError("No valid spawn point found!");
            }
            else
                Debug.LogError("SpawnManager not found in the scene!");
        }

        playerCamera.gameObject.GetComponent<AudioListener>().enabled = IsLocalPlayer;
    }

    public void SetRatVisual()
	{
        Debug.Log("Set rat visual from originating client");
        SetPlayerRatVisualServerRpc(OwnerClientId);
	}

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerRatVisualServerRpc(ulong playerId)
	{
        Debug.Log("Set rat visual from server");
        SetPlayerRatVisualClientRpc(playerId);
	}

    [ClientRpc]
    private void SetPlayerRatVisualClientRpc(ulong playerId)
	{
        Debug.Log("Set rat visual from all client");

        

        //if (OwnerClientId == playerId) return;

        Debug.Log("Set rat visual from other client");

        PlayerSeter[] seters = FindObjectsOfType<PlayerSeter>();

		foreach (var s in seters)
		{
            if(s.OwnerClientId == playerId)
			{
                Debug.Log(OwnerClientId + ":" + playerId);
                s.SetPlayerRatVisual();
                break;
			}
		}
	}

    public void SetPlayerRatVisual()
	{
        pirateModel.SetActive(false);
        ratModel.SetActive(true);
	}
}
