using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class LobbyPlayers : NetworkBehaviour
{
    public static LobbyPlayers Instance;

    public GameObject[] players; //0 == host    1 2 3 == client
    public TextMeshProUGUI[] playersName;

    public List<GameObject> playersObj = new List<GameObject>();

    public void Awake()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetActive(false);
        }
    }

    public void FixedUpdate()
    {
        // Récupérer tous les objets avec le tag "Player"
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        // Ajouter les objets récupérés dans la liste
        foreach (GameObject playerObject in playerObjects)
        {
            if (!playersObj.Contains(playerObject))
            {
                playersObj.Add(playerObject);
            }
        }

        for (int i = 0; i < playersObj.Count; i++)
        {
            players[i].SetActive(true);
            playersName[i].text = playersObj[i].GetComponent<PlayerNameSync>().displayName.ToString();
        }
    }

    public void RefontName()
    {
        for (int i = 0; i < playersObj.Count; i++)
        {
            players[i].SetActive(true);
            playersName[i].text = playersObj[i].GetComponent<PlayerNameSync>().displayName.ToString();
        }
    }
}