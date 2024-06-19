using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
	[SerializeField] private GameObject hostPanel, startPanel, loadingPanel;

    public void HostGame()
	{
		hostPanel.SetActive(false);
		GameNetworkManager.Instance.StartHost(4);
		startPanel.SetActive(true);
	}

	public void JoinGame()
	{
		hostPanel.SetActive(false);
	}

	public void StartGame()
	{
		if (NetworkManager.Singleton.IsHost)
		{
			SetLoadingScreenClientRpc();
			NetworkManager.Singleton.SceneManager.LoadScene("MultiScene 2", LoadSceneMode.Single);
		}
	}

	[ClientRpc]
	private void SetLoadingScreenClientRpc()
	{
		hostPanel.SetActive(false);
		startPanel.SetActive(false);
		loadingPanel.SetActive(true);
	}
}
