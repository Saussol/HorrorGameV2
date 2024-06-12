using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
	[SerializeField] private GameObject hostPanel;

    public void HostGame()
	{
		hostPanel.SetActive(false);
		GameNetworkManager.Instance.StartHost(4);
	}

	public void JoinGame()
	{
		hostPanel.SetActive(false);
	}
}
