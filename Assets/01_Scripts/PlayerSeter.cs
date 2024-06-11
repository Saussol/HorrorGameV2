using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSeter : NetworkBehaviour
{
    public GameObject playerCamera;
    public GameObject playerCanvas;
    public CharacterMovement playerControls; // Par exemple, un GameObject qui contient le script de contrôle du joueur

    public void Start()
    {
        DisablePlayerComponents();
        //Debug.Log("tesssssssst");
    }

    public void DisablePlayerComponents()
    {
        if (playerCamera != null)
        {
            playerCamera.SetActive(false);
        }

        if (playerCanvas != null)
        {
            playerCanvas.SetActive(false);
        }

        if (playerControls != null)
        {
            playerControls.enabled = false;
        }
    }

    public void ReSetPlayerComponents()
    {
        if (playerCamera != null && IsLocalPlayer)
        {
            playerCamera.SetActive(true);
        }

        if (playerCanvas != null && IsLocalPlayer)
        {
            playerCanvas.SetActive(true);
        }

        if (playerControls != null && IsLocalPlayer)
        {
            playerControls.enabled = true;
        }
    }

}
