using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CamPlayer : NetworkBehaviour
{
    public Camera playerCam;
    public GameObject canvas;

    public void OnEnable()
    {
        Debug.Log("test");
    }

    public void Start()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "SampleScene")
        {
            if (this.IsLocalPlayer)
            {
                Camera playerCamera = this.GetComponentInChildren<Camera>();
                if (playerCamera != null)
                    playerCamera.enabled = false;

                canvas.SetActive(false);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

            }
            else
            {
                Camera playerCamera = this.GetComponentInChildren<Camera>();
                if (playerCamera != null)
                    playerCamera.enabled = false;

                canvas.SetActive(false);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

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

            }
            else
            {
                // Disable camera for other players
                Camera playerCamera = this.GetComponentInChildren<Camera>();
                if (playerCamera != null)
                    playerCamera.enabled = false;
        
            }
        }

    }

    public void SetCam()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "SampleScene")
        {
            if (this.IsLocalPlayer)
            {
                Camera playerCamera = this.GetComponentInChildren<Camera>();
                if (playerCamera != null)
                    playerCamera.enabled = false;

                canvas.SetActive(false);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

               
            }
            else
            {
                Camera playerCamera = this.GetComponentInChildren<Camera>();
                if (playerCamera != null)
                    playerCamera.enabled = false;

                canvas.SetActive(false);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

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

              
            }
            else
            {
                // Disable camera for other players
                Camera playerCamera = this.GetComponentInChildren<Camera>();
                if (playerCamera != null)
                    playerCamera.enabled = false;

               
            }
        }

    }
}
