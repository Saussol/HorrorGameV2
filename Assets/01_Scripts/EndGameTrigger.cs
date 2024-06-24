using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
	int maxPlayer;
	int currentPlayer;

	bool endGame;

	[SerializeField] private GameObject winPanel, loosePanel;

	private void Start()
	{
		maxPlayer = FindObjectsOfType<CharacterMovement>().Length;
	}

	public void LooseGame()
	{
		loosePanel.SetActive(true);
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

			if(currentPlayer >= maxPlayer)// & if - rat
			{
				endGame = true;
				Debug.Log("<color=green>YOU WIN</color>");
				winPanel.SetActive(true);

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
