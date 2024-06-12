using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCamera : NetworkBehaviour
{
    public Camera playerCam;
    public GameObject canvas;

    public CharacterMovement characterMovement;
    public CharacterTarget characterTarget;
    public GameObject canvasName;

    public void Start()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "MultiOffline")
        {
            if (this.IsLocalPlayer)
            {
                Camera playerCamera = this.GetComponentInChildren<Camera>();
                if (playerCamera != null)
                    playerCamera.enabled = false;

                canvas.SetActive(false);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                characterMovement.enabled = false;
                characterTarget.enabled = false;
            }
            else
            {
                Camera playerCamera = this.GetComponentInChildren<Camera>();
                if (playerCamera != null)
                    playerCamera.enabled = false;

                canvas.SetActive(false);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                characterMovement.enabled = false;
                characterTarget.enabled = false;
            }

        }
        else
        {
            if (this.IsLocalPlayer)
            {
                // Activate the camera attached to the player object
                Camera playerCamera = this.GetComponentInChildren<Camera>();
                if (playerCamera != null)
                    playerCamera.enabled = true;

                playerCam = playerCamera;
                canvas.SetActive(true);

                //roleScripts.enabled = true;
                characterMovement.enabled = true;
                characterTarget.enabled = true;
            }
            else
            {
                // Disable camera for other players
                Camera playerCamera = this.GetComponentInChildren<Camera>();
                if (playerCamera != null)
                    playerCamera.enabled = false;

                //roleScripts.enabled = true;
                characterMovement.enabled = true;
                characterTarget.enabled = true;
            }
        }

    }


    
}

