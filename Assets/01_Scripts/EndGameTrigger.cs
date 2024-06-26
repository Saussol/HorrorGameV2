using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EndGameTrigger : NetworkBehaviour
{
	int maxPlayer;
	int currentPlayer;

	bool endGame;

	[SerializeField] private GameObject winPanel, loosePanel, endUi, buttonHost;

	private void Start()
	{
		maxPlayer = FindObjectsOfType<CharacterMovement>().Length;
	}

	public void LooseGame()
	{
		loosePanel.SetActive(true);
        endUi.SetActive(true);
        buttonHost.SetActive(true);



        CharacterMovement[] players = FindObjectsOfType<CharacterMovement>();
		foreach (CharacterMovement player in players)
		{
			player.StopPlayer();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!QuestSpawner.Instance.allQuestEnded) return;

		if (other.CompareTag("Player"))
		{
			currentPlayer++;

			PlayerSeter[] seters = FindObjectsOfType<PlayerSeter>();

			maxPlayer = 0;

            foreach (var p in seters)
            {
				if (!p.isRat) maxPlayer++;
            }


			if(currentPlayer >= maxPlayer)
			{
				endGame = true;
				Debug.Log("<color=green>YOU WIN</color>");
				winPanel.SetActive(true);
                endUi.SetActive(true);
                buttonHost.SetActive(true);


                CharacterMovement[] players = FindObjectsOfType<CharacterMovement>();
				foreach(CharacterMovement player in players)
				{
					player.StopPlayer();
				}
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (!QuestSpawner.Instance.allQuestEnded || endGame) return;

		if (other.CompareTag("Player"))
		{
			currentPlayer--;
		}
	}
}
