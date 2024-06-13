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
    public Canvas playerCanvas;
    public CharacterMovement playerControls; // Par exemple, un GameObject qui contient le script de contrôle du joueur

    private void Start()
    {
        DisablePlayerComponents();
    }

    private void OnEnable()
    {
        NetworkManager.Singleton.SceneManager.OnLoad += OnSceneLoaded;
    }

    private void OnDisable()
    {
        NetworkManager.Singleton.SceneManager.OnLoad -= OnSceneLoaded;
    }

    private void OnSceneLoaded(ulong clientId, string sceneName, LoadSceneMode loadSceneMode, AsyncOperation asyncOperation)
	{
        Debug.Log($"Scene {sceneName} loaded for client {clientId}");
        if (sceneName == "MultiScene 2")
        {
            // Call your custom function here
            ReSetPlayerComponents();
        }
    }

	public void DisablePlayerComponents()
    {
        if (playerCamera != null)
        {
            playerCamera.enabled = false;
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

    public void ReSetPlayerComponents()
    {
        if (playerCamera != null && IsOwner)
        {
            playerCamera.enabled = true;
        }


        if (playerCanvas != null && IsOwner)
        {
            playerCanvas.enabled = true;
        }


        if (playerControls != null && IsOwner)
        {
            playerControls.enabled = true;
        }


        //playerCamera.gameObject.GetComponent<AudioListener>().enabled = IsLocalPlayer;
        //playerCamera.SetActive(IsLocalPlayer);

    }



}
