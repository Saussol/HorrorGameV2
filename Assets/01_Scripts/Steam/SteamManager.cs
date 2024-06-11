using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Steamworks.Data;
using System;
using Unity.Netcode;
using Netcode.Transports.Facepunch;
using UnityEngine.SceneManagement;

public class SteamManager : MonoBehaviour
{
    
    [SerializeField]
    private TMP_InputField LobbyIDInputField;

    [SerializeField]
    private TextMeshProUGUI LobbyID;

    [SerializeField]
    private GameObject MainMenu;

    [SerializeField]
    private GameObject InLobbyMenu;

    [SerializeField]
    private GameObject[] playerprefTag;

    private void OnEnable()
    {
        SteamMatchmaking.OnLobbyCreated += LobbyCreated;
        SteamMatchmaking.OnLobbyEntered += LobbyEntered;
        SteamFriends.OnGameLobbyJoinRequested += GameLobbyJoinReques;
    }

    

    private void OnDisable()
    {
        SteamMatchmaking.OnLobbyCreated -= LobbyCreated;
        SteamMatchmaking.OnLobbyEntered -= LobbyEntered;
        SteamFriends.OnGameLobbyJoinRequested -= GameLobbyJoinReques;
    }

    private async void GameLobbyJoinReques(Lobby lobby, SteamId id)
    {
        await lobby.Join();
    }

    private void LobbyEntered(Lobby lobby)
    {
        LobbySaver.instance.currentLobby = lobby;
        LobbyID.text = lobby.Id.ToString();
        MainMenu.SetActive(false);
        InLobbyMenu.SetActive(true);

        Debug.Log("We entered");

        // Attendre que les informations des joueurs soient récupérées
        foreach (var member in lobby.Members)
        {
            SteamId steamId = member.Id;
            string playerName = member.Name;
            Debug.Log("Player " + playerName + " joined the lobby.");

            // Afficher le pseudo du client pour le host
            if (NetworkManager.Singleton.IsHost && steamId != SteamClient.SteamId)
            {
                Debug.Log("Client " + playerName + " joined the lobby.");
            }
        }

        // Affichage pour le host
        if (NetworkManager.Singleton.IsHost)
        {
            Debug.Log("Host joined the lobby.");
        }

        if (!NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.gameObject.GetComponent<FacepunchTransport>().targetSteamId = lobby.Owner.Id;
            NetworkManager.Singleton.StartClient();
        }
        else
        {
            // Si c'est le host, il ne doit pas rejoindre en tant que client
            // Vous pouvez éventuellement démarrer le serveur ici
            // NetworkManager.Singleton.StartHost();
        }
    }

    private void LobbyCreated(Result result, Lobby lobby)
    {
        if (result == Result.OK)
        {
            lobby.SetPublic();
            lobby.SetJoinable(true);
            NetworkManager.Singleton.StartHost();
        }
    }

    public async void HostLobby()
    {
        await SteamMatchmaking.CreateLobbyAsync(4);
    }

    public async void JoinLobbyWithID()
    {
        ulong ID;
        if (!ulong.TryParse(LobbyIDInputField.text, out ID))
            return;

        Lobby[] lobbies = await SteamMatchmaking.LobbyList.WithSlotsAvailable(1).RequestAsync();

        foreach(Lobby lobby in lobbies)
        {
            if(lobby.Id == ID)
            {
                await lobby.Join();
                return;
            }
        }
    }

    public void CopyID()
    {
        TextEditor textEditor = new TextEditor();
        textEditor.text = LobbyID.text;
        textEditor.SelectAll();
        textEditor.Copy();
    }

    public void LeaveLobby()
    {
       // LobbySaver.instance.currentLobby?.Leave();
       // LobbySaver.instance.currentLobby = null;
       //  NetworkManager.Singleton.Shutdown();

    }

    public void StartGameServer()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("MultiLobby", LoadSceneMode.Single);
        }
    }

    
   

}
